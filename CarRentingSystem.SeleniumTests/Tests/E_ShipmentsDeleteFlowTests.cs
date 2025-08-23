using CarRentingSystem.SeleniumTests.Fixtures;
using CarRentingSystem.SeleniumTests.Pages.Layout;
using CarRentingSystem.SeleniumTests.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Globalization;
using System.Linq;

namespace CarRentingSystem.SeleniumTests.Tests
{
    [TestFixture]
    public sealed class E_ShipmentsDeleteFlowTests : TestBase
    {

        [Test]
        public void AddNewShipment_Then_Delete_ShowsNotFoundInAllSearch()
        {
            EnsureLoggedIn();

            // Use one title everywhere
            var originalTitle = "Plovdiv-Valence-DEL";
            AddShipment(
                title: originalTitle,
                from: "Plovdiv",
                to: "Valence",
                desc: $"Charter {originalTitle}",
                imageUrl: "https://www.hispaviacion.es/wp-content/uploads/2022/05/Falcon2000.jpeg",
                price: 7500m,
                preferCategoryText: "Charter"
            );

            // Find it in All
            NavigateToAll();
            SearchAllByTitle(originalTitle);
            Assert.That(Driver.PageSource, Does.Contain(originalTitle).IgnoreCase, "New shipment should appear before deletion.");

            EnsureLoggedIn();
            // Delete it
            ClickDeleteOnCard(originalTitle);

            ConfirmDelete();      // clicks the red Delete on the confirmation page
          
            // Re-search and assert it's gone
            NavigateToAll();
            SearchAllByTitle(originalTitle);
            WaitUntil(d => !d.PageSource.Contains(originalTitle, StringComparison.OrdinalIgnoreCase), seconds: 12);
            Assert.That(Driver.PageSource.Contains(originalTitle, StringComparison.OrdinalIgnoreCase), Is.False,
                "Shipment should NOT be found after deletion.");

            Logout();
        }

        // ───────────────────────── helpers ─────────────────────────

        /// <summary>Log in only if not already authenticated (looks for “Logout” in navbar).</summary>
        private void EnsureLoggedIn()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/");
            if (Driver.PageSource.Contains("Logout", StringComparison.OrdinalIgnoreCase))
                return;

            Driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");
            WaitUntil(_ => Driver.Url.Contains("/Identity/Account/Login", StringComparison.OrdinalIgnoreCase));

            ClearAndType(By.Id("Input_Email"), TestUsers.Email);
            ClearAndType(By.Id("Input_Password"), TestUsers.Password);
            Driver.FindElement(By.CssSelector("form button[type='submit'], form input[type='submit']")).Click();

            WaitUntil(_ => Driver.PageSource.Contains("Logout", StringComparison.OrdinalIgnoreCase));
        }

        private void AddShipment(string title, string from, string to, string desc, string imageUrl,
                                 decimal price, string? preferCategoryText = null)
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Shipments/Add");
            WaitUntil(d => d.Url.Contains("/Shipments/Add", StringComparison.OrdinalIgnoreCase));

            ClearAndType(By.Id("Title"), title);
            ClearAndType(By.Id("LoadingAddress"), from);
            ClearAndType(By.Id("DeliveryAddress"), to);
            ClearAndType(By.Id("Description"), desc);
            ClearAndType(By.Id("ImageUrlShipmentGoogleMaps"), imageUrl);
            ClearAndType(By.Id("Price"), price.ToString(CultureInfo.InvariantCulture));

            var cat = new SelectElement(Driver.FindElement(By.Id("CategoryId")));
            if (!string.IsNullOrWhiteSpace(preferCategoryText))
            {
                var option = cat.Options.FirstOrDefault(o =>
                    string.Equals(o.Text?.Trim(), preferCategoryText, StringComparison.OrdinalIgnoreCase));
                if (option != null) cat.SelectByText(option.Text);
                else SelectFirstRealOption(cat);
            }
            else
            {
                SelectFirstRealOption(cat);
            }

            ClickSaveButton(); // 👈 this submits the form

            // Success = left /Shipments/Add (Details or Home are both fine)
            var ok = WaitUrl(u =>
                   u.Contains("/Shipments/Details", StringComparison.OrdinalIgnoreCase)
                || u.TrimEnd('/').Equals(BaseUrl.TrimEnd('/'), StringComparison.OrdinalIgnoreCase),
                timeoutSeconds: 12);

            if (!ok)
            {
                var errors = Driver
                    .FindElements(By.CssSelector(".validation-summary-errors li, [asp-validation-summary] li, .text-danger"))
                    .Select(e => e.Text?.Trim())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct();
                Assert.Fail("Add failed; still on Add. Validation: " + string.Join(" | ", errors));
            }
        }

        private static void SelectFirstRealOption(SelectElement cat)
        {
            var firstReal = cat.Options.FirstOrDefault(o =>
                !string.IsNullOrWhiteSpace(o.GetAttribute("value")));
            if (firstReal != null) cat.SelectByValue(firstReal.GetAttribute("value"));
            else cat.SelectByIndex(0);
        }

        private void NavigateToAll()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Shipments/All");
            WaitUntil(d => d.Url.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase));
        }

        private void SearchAllByTitle(string title)
        {
            var box = Driver.FindElement(By.Id("SearchTerm"));
            box.Clear();
            box.SendKeys(title);

            var btn = Driver.FindElements(By.XPath("//button[normalize-space()='Search']")).FirstOrDefault()
                     ?? Driver.FindElements(By.CssSelector("input[type='submit'][value='Search']")).FirstOrDefault()
                     ?? Driver.FindElements(By.CssSelector("form button[type='submit'], form input[type='submit']")).FirstOrDefault();
            Assert.That(btn, Is.Not.Null, "Could not locate the Search button.");
            btn!.Click();

            // Wait for server-side search to apply
            WaitUntil(d => d.Url.Contains("SearchTerm=", StringComparison.OrdinalIgnoreCase));
        }

        private void ClickDeleteOnCard(string title)
        {
            EnsureLoggedIn();

            string xTitle = XPathLiteral(title);

            Func<IWebElement?> locate = () =>
                Driver.FindElements(By.XPath($"//a[normalize-space()='Delete' and ancestor::*[contains(., {xTitle})]]"))
                      .FirstOrDefault();

            var link = new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(_ => locate());
            Assert.That(link, Is.Not.Null, $"Could not find Delete button for '{title}'.");

            try { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", link); } catch { }
            try { link!.Click(); }
            catch (WebDriverException) { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", link); }

            WaitUntil(d => d.Url.Contains("/Shipments/Delete", StringComparison.OrdinalIgnoreCase), seconds: 12);
        }

        private void ConfirmDelete()
        {
            EnsureLoggedIn();

            var form = Driver.FindElements(By.TagName("form")).FirstOrDefault();
            Assert.That(form, Is.Not.Null, "Delete confirmation form not found.");

            var btn = form!.FindElements(By.CssSelector("input[type='submit'][value='Delete']")).FirstOrDefault()
                   ?? form.FindElements(By.XPath(".//button[normalize-space()='Delete']")).FirstOrDefault()
                   ?? form.FindElements(By.CssSelector("button[type='submit'], input[type='submit']")).FirstOrDefault();

            Assert.That(btn, Is.Not.Null, "Delete submit button not found on confirmation page.");

            try { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btn); } catch { }
            try { btn!.Click(); }
            catch (WebDriverException) { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", btn); }

            // Back to All or Home after delete
            WaitUrl(u =>
                   u.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase)
                || u.TrimEnd('/').Equals(BaseUrl.TrimEnd('/'), StringComparison.OrdinalIgnoreCase),
                timeoutSeconds: 12);
        }

        // low-level utilities

        private void ClearAndType(By by, string value)
        {
            var el = Driver.FindElement(by);
            el.Clear();
            el.SendKeys(value);
        }

        private void ClickSaveButton()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var form = wait.Until(d => d.FindElements(By.TagName("form"))
                                        .FirstOrDefault(f => f.FindElements(By.Id("Title")).Any()));
            Assert.That(form, Is.Not.Null, "Could not locate the shipment form.");

            IWebElement? save = null;
            foreach (var css in new[] { "input[type='submit'][value='Save']", "button[type='submit']", "input[type='submit']" })
            {
                save = form!.FindElements(By.CssSelector(css)).FirstOrDefault();
                if (save != null) break;
            }
            Assert.That(save, Is.Not.Null, "Could not locate the Save button within the form.");

            try { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", save); } catch { }
            try { save!.Click(); }
            catch (WebDriverException) { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", save); }

            // Last resort
            System.Threading.Thread.Sleep(150);
            if (Driver.Url.Contains("/Shipments/Add", StringComparison.OrdinalIgnoreCase) ||
                Driver.Url.Contains("/Shipments/Edit", StringComparison.OrdinalIgnoreCase))
            {
                try { form!.Submit(); }
                catch { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].submit();", form); }
            }
        }

        private bool WaitUrl(Func<string, bool> predicate, int timeoutSeconds)
        {
            try
            {
                var w = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                return w.Until(d => predicate(d.Url));
            }
            catch { return false; }
        }

        private static string XPathLiteral(string value)
        {
            if (!value.Contains("'")) return $"'{value}'";
            if (!value.Contains("\"")) return $"\"{value}\"";
            return "concat('" + value.Replace("'", "',\"'\",'") + "')";
        }


        // ---- helper ----
        private void Logout()
        {
            // Go to a page that has the navbar
            Driver.Navigate().GoToUrl(BaseUrl + "/");

            var nav = new Navbar(Driver);
            if (!nav.IsLoggedIn) return;   // already logged out

            // Try page-object logout first
            try
            {
                nav.Logout();
            }
            finally
            {
                // Wait until navbar reflects logged-out state
                WaitUntil(_ => !new Navbar(Driver).IsLoggedIn);
            }
        }
    }
}
