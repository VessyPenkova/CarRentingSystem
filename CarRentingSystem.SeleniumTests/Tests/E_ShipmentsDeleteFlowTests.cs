using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using CarRentingSystem.SeleniumTests.Fixtures;
using CarRentingSystem.SeleniumTests.Utils;

namespace CarRentingSystem.SeleniumTests.Tests
{
    [TestFixture]
    public sealed class E_ShipmentsDeleteFlowTests : TestBase
    {
        [Test]
        public void Add_Then_Search_Then_Delete_Then_Search_NotFound()
        {
            // 1) login (Delete button is visible only for authenticated users)
            LoginWith(TestUsers.Email, TestUsers.Password);

            // 2) add a shipment and remember its title
            var title = AddShipmentAndReturnTitle();

            // 3) search for it – it must exist before deletion
            SearchInAllShipments(title);
            Assert.That(Driver.PageSource.Contains(title, StringComparison.OrdinalIgnoreCase),
                $"Expected to find '{title}' before deletion.");

            // 4) click Delete on that card
            ClickDeleteOnCard(title);

            // 5) confirm Delete on the confirmation page
            ConfirmDelete();

            // 6) search again and WAIT FOR ABSENCE (this was the bug)
            SearchInAllShipments(title);

            WaitUntil(
                d => !d.PageSource.Contains(title, StringComparison.OrdinalIgnoreCase),
                seconds: 12,
                onTimeout: $"Deleted shipment '{title}' still visible in All Shipments after re-search.");

            Assert.That(Driver.PageSource.Contains(title, StringComparison.OrdinalIgnoreCase), Is.False,
                "Shipment should not be found after deletion.");
        }

        // ───────────────────────── helpers ─────────────────────────

        private void LoginWith(string email, string password)
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");
            WaitUntil(_ => Driver.Url.Contains("/Identity/Account/Login", StringComparison.OrdinalIgnoreCase));

            Type(By.Id("Input_Email"), email);
            Type(By.Id("Input_Password"), password);
            Driver.FindElement(By.CssSelector("form button[type='submit'], form input[type='submit']")).Click();

            // proof of auth
            WaitUntil(_ => Driver.PageSource.Contains("Logout", StringComparison.OrdinalIgnoreCase));
        }

        private string AddShipmentAndReturnTitle()
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Shipments/Add");
            WaitUntil(_ => Driver.Url.Contains("/Shipments/Add", StringComparison.OrdinalIgnoreCase));

            var title = $"Plovdiv-Valenc{DateTime.UtcNow:HHmmss}";

            Type(By.Id("Title"), title);
            Type(By.Id("LoadingAddress"), "Plovdiv");
            Type(By.Id("DeliveryAddress"), "Valence");
            Type(By.Id("Description"), $"Charter {title}");
            Type(By.Id("ImageUrlShipmentGoogleMaps"),
                 "https://www.hispaviacion.es/wp-content/uploads/2022/05/Falcon2000.jpeg");

            // price
            ClearAndType(By.Id("Price"), 7500m.ToString(CultureInfo.InvariantCulture));

            // category
            var cat = new SelectElement(Driver.FindElement(By.Id("CategoryId")));
            var charter = cat.Options.FirstOrDefault(o =>
                string.Equals(o.Text?.Trim(), "Charter", StringComparison.OrdinalIgnoreCase));
            if (charter != null) cat.SelectByText(charter.Text);
            else
            {
                var firstReal = cat.Options.FirstOrDefault(o => !string.IsNullOrWhiteSpace(o.GetAttribute("value")));
                if (firstReal != null) cat.SelectByValue(firstReal.GetAttribute("value"));
                else cat.SelectByIndex(0);
            }

            ClickSaveButton();

            var ok = WaitUrl(u =>
                   u.Contains("/Shipments/Details", StringComparison.OrdinalIgnoreCase)
                || u.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase)
                || u.Contains("/Home", StringComparison.OrdinalIgnoreCase)
                || u.TrimEnd('/').Equals(BaseUrl.TrimEnd('/'), StringComparison.OrdinalIgnoreCase),
                timeoutSeconds: 10);

            if (!ok)
            {
                var messages = Driver
                    .FindElements(By.CssSelector(".validation-summary-errors li, [asp-validation-summary] li, .text-danger"))
                    .Select(e => e.Text?.Trim())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct();
                Assert.Fail("Save failed, still on Add page. Validation: " + string.Join(" | ", messages));
            }

            return title;
        }

        private void SearchInAllShipments(string title)
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Shipments/All");
            WaitUntil(_ => Driver.Url.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase));

            var search = Driver.FindElement(By.Id("SearchTerm"));
            search.Clear();
            search.Click();
            search.SendKeys(title);

            ClickSearchButton();

            // server-side search updates query string
            WaitUntil(_ => Driver.Url.Contains("SearchTerm=", StringComparison.OrdinalIgnoreCase), seconds: 5);
        }

        private void ClickDeleteOnCard(string title)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var xTitle = XPathLiteral(title);

            IWebElement? delete = null;
            wait.Until(_ =>
            {
                delete = Driver.FindElements(By.XPath(
                           $"//a[normalize-space()='Delete' and ancestor::*[contains(., {xTitle})]]"))
                           .FirstOrDefault();
                return delete != null;
            });

            Assert.That(delete, Is.Not.Null,
                $"Delete link not visible for '{title}'. Are you logged in as an owner?");

            try { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", delete); } catch { }
            try { delete!.Click(); }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", delete);
            }

            WaitUntil(_ => Driver.Url.Contains("/Shipments/Delete", StringComparison.OrdinalIgnoreCase));
        }

        private void ConfirmDelete()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var form = wait.Until(d => d.FindElements(By.TagName("form")).FirstOrDefault());
            Assert.That(form, Is.Not.Null, "Delete confirmation form not found.");

            var btn =
                form!.FindElements(By.CssSelector("input[type='submit'][value='Delete']")).FirstOrDefault()
                ?? form.FindElements(By.XPath(".//button[normalize-space()='Delete']")).FirstOrDefault()
                ?? form.FindElements(By.CssSelector("button[type='submit'], input[type='submit']")).FirstOrDefault();

            Assert.That(btn, Is.Not.Null, "Delete submit button not found on confirmation page.");

            try { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btn); } catch { }
            try { btn!.Click(); }
            catch (WebDriverException)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", btn);
            }

            var ok = WaitUrl(u =>
                   u.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase)
                || u.Contains("/Home", StringComparison.OrdinalIgnoreCase)
                || u.TrimEnd('/').Equals(BaseUrl.TrimEnd('/'), StringComparison.OrdinalIgnoreCase),
                timeoutSeconds: 10);

            Assert.That(ok, Is.True, "Expected redirect to All Shipments or Home after delete.");
        }

        // ----- tiny low-level helpers (same style as your other tests) -----

        private void ClickSaveButton()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

            var form = wait.Until(d =>
                d.FindElements(By.TagName("form"))
                 .FirstOrDefault(f => f.FindElements(By.Id("Title")).Any()));
            Assert.That(form, Is.Not.Null, "Could not locate the Add/Edit form.");

            IWebElement? save =
                form!.FindElements(By.CssSelector("input[type='submit'][value='Save']")).FirstOrDefault()
                ?? form.FindElements(By.CssSelector("button[type='submit']")).FirstOrDefault()
                ?? form.FindElements(By.CssSelector("input[type='submit']")).FirstOrDefault();

            Assert.That(save, Is.Not.Null, "Save button not found in the form.");

            try { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", save); } catch { }
            try { save!.Click(); }
            catch (WebDriverException)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", save);
            }
        }

        private void ClickSearchButton()
        {
            var btn = Driver.FindElements(By.XPath("//button[normalize-space()='Search']")).FirstOrDefault()
                     ?? Driver.FindElements(By.CssSelector("input[type='submit'][value='Search']")).FirstOrDefault()
                     ?? Driver.FindElements(By.CssSelector("form button[type='submit'], form input[type='submit']")).FirstOrDefault();

            Assert.That(btn, Is.Not.Null, "Search button not found.");
            try { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", btn); } catch { }
            try { btn!.Click(); }
            catch (WebDriverException)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", btn);
            }
        }

        private void Type(By by, string value)
        {
            var el = Driver.FindElement(by);
            el.Clear();
            el.SendKeys(value);
        }

        private void ClearAndType(By by, string value)
        {
            var el = Driver.FindElement(by);
            el.Clear();
            el.SendKeys(value);
        }

        private static string XPathLiteral(string text)
        {
            if (!text.Contains("'")) return $"'{text}'";
            if (!text.Contains("\"")) return $"\"{text}\"";
            var parts = text.Split('\'');
            return "concat(" + string.Join(", \"'\", ", parts.Select(p => $"'{p}'")) + ")";
        }
    }
}
