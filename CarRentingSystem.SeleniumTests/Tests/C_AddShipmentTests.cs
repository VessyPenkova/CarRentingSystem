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
    public sealed class C_AddShipmentTests : TestBase
    {
        [Test]
        public void AddNewShipmentWithValidData_ShowsUpInAllSearch()
        {
            // 1) Log in with the fixed user
            LoginWith(TestUsers.Email, TestUsers.Password);

            // 2) Go to /Shipments/Add
            Driver.Navigate().GoToUrl($"{BaseUrl}/Shipments/Add");
            WaitUntil(d => d.Url.Contains("/Shipments/Add", StringComparison.OrdinalIgnoreCase));

            // Use a unique title so search is deterministic
            var title = "Plovdiv-Valence-01";
            var image = "https://www.hispaviacion.es/wp-content/uploads/2022/05/Falcon2000.jpeg";

            // 3) Fill the form (ids from asp-for in your partial)
            Type(By.Id("Title"), title);
            Type(By.Id("LoadingAddress"), "Plovdiv");
            Type(By.Id("DeliveryAddress"), "Valence");
            Type(By.Id("Description"), $"Charter {title}");
            Type(By.Id("ImageUrlShipmentGoogleMaps"), image);

            // Price (use invariant dot separator)
            ClearAndType(By.Id("Price"), 7500m.ToString(CultureInfo.InvariantCulture));

            // Category: prefer "Charter"; else pick the first with a non-empty value
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

            // 4) Click the "Save" button (robust)
            ClickSaveButton();

            // If save succeeded we should leave /Shipments/Add (either Details or Home)
            var leftAddPage = WaitUrl(url =>
                   url.Contains("/Shipments/Details", StringComparison.OrdinalIgnoreCase)
                || url.TrimEnd('/').Equals(BaseUrl.TrimEnd('/'), StringComparison.OrdinalIgnoreCase)
                || url.Contains("/Home", StringComparison.OrdinalIgnoreCase),
                timeoutSeconds: 8);

            // If still on Add, dump validation errors so you can see why it didn’t persist
            if (!leftAddPage)
            {
                var errors = Driver
                    .FindElements(By.CssSelector(".validation-summary-errors li, [asp-validation-summary] li, .text-danger"))
                    .Select(e => e.Text?.Trim())
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .Distinct()
                    .ToArray();

                Assert.Fail("Add failed; still on /Shipments/Add. Validation: " + string.Join(" | ", errors));
            }

            // 5) Open All Shipments and search by the title we just created
            Driver.Navigate().GoToUrl($"{BaseUrl}/Shipments/All");
            WaitUntil(d => d.Url.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase));

            var searchBox = Driver.FindElement(By.Id("SearchTerm"));
            searchBox.Clear();
            searchBox.SendKeys(title);

            // Click the Search button
            ClickSearchButton();

            // Assert the results contain the new shipment
            WaitUntil(d => d.PageSource.Contains(title, StringComparison.OrdinalIgnoreCase));
            Assert.That(Driver.PageSource, Does.Contain(title).IgnoreCase,
                "Newly added shipment should appear in All Shipments search results.");
        }

        // ---------- helpers ----------

        private void LoginWith(string email, string password)
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");
            WaitUntil(_ => Driver.Url.Contains("/Identity/Account/Login", StringComparison.OrdinalIgnoreCase));

            Type(By.Id("Input_Email"), email);
            Type(By.Id("Input_Password"), password);

            var submit = Driver.FindElement(By.CssSelector("form button[type='submit'], form input[type='submit']"));
            submit.Click();

            WaitUntil(_ => Driver.PageSource.Contains("Logout", StringComparison.OrdinalIgnoreCase));
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

        private void ClickSearchButton()
        {
            var btn = Driver.FindElements(By.XPath("//button[normalize-space()='Search']")).FirstOrDefault()
                     ?? Driver.FindElements(By.CssSelector("input[type='submit'][value='Search']")).FirstOrDefault()
                     ?? Driver.FindElements(By.CssSelector("form button[type='submit'], form input[type='submit']")).FirstOrDefault();

            Assert.That(btn, Is.Not.Null, "Could not locate the Search button on All Shipments.");
            btn.Click();
        }

        private void ClickSaveButton()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

            // 1) Find the shipment form (the one that contains the Title input)
            var form = wait.Until(d =>
                d.FindElements(By.TagName("form"))
                 .FirstOrDefault(f => f.FindElements(By.Id("Title")).Any())
            );
            Assert.That(form, Is.Not.Null, "Could not locate the Add/Edit Shipment form (containing #Title).");

            // 2) Find a Save submit control INSIDE that form
            IWebElement? save = null;
            var candidates = new[]
            {
                "input[type='submit'][value='Save']",
                "button[type='submit']",
                "input[type='submit']"
            };

            foreach (var css in candidates)
            {
                var found = form!.FindElements(By.CssSelector(css)).FirstOrDefault();
                if (found != null) { save = found; break; }
            }
            Assert.That(save, Is.Not.Null, "Could not locate the Save button within the shipment form.");

            // 3) Scroll & click (with JS-click fallback)
            wait.Until(_ => save!.Displayed && save.Enabled);
            try { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", save); } catch { /* ignore */ }

            try
            {
                save!.Click();
            }
            catch (WebDriverException)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", save);
            }

            // 4) Final fallback: if still on Add/Edit, submit the form directly
            System.Threading.Thread.Sleep(200);
            if (Driver.Url.Contains("/Shipments/Add", StringComparison.OrdinalIgnoreCase) ||
                Driver.Url.Contains("/Shipments/Edit", StringComparison.OrdinalIgnoreCase))
            {
                try { form!.Submit(); }
                catch
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].submit();", form);
                }
            }
        }

        private bool WaitUrl(Func<string, bool> predicate, int timeoutSeconds)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(d => predicate(d.Url));
            }
            catch { return false; }
        }
    }
}
