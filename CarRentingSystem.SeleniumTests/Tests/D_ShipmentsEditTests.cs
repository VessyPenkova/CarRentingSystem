using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using CarRentingSystem.SeleniumTests.Fixtures;
using CarRentingSystem.SeleniumTests.Utils;

namespace CarRentingSystem.SeleniumTests.Tests
{
    [TestFixture]
    public sealed class D_EditShipmentTests : TestBase
    {
        [Test] // [Retry(2)] // uncomment if your env is occasionally slow
        public void AddNewShipment_Then_EditTitle_ShowsEditedInAllSearch()
        {
            // 1) Login with your fixed user
            LoginWith(TestUsers.Email, TestUsers.Password);

            // 2) Add a new shipment
            Driver.Navigate().GoToUrl($"{BaseUrl}/Shipments/Add");
            WaitUntil(d => d.Url.Contains("/Shipments/Add", StringComparison.OrdinalIgnoreCase), seconds: 15);

            //var suffix = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            var title = $"Plovdiv-Valence-02";
            var image = "https://www.hispaviacion.es/wp-content/uploads/2022/05/Falcon2000.jpeg";

            Type(By.Id("Title"), title);
            Type(By.Id("LoadingAddress"), "Plovdiv");
            Type(By.Id("DeliveryAddress"), "Valence");
            Type(By.Id("Description"), $"Charter {title}");
            Type(By.Id("ImageUrlShipmentGoogleMaps"), image);
            ClearAndType(By.Id("Price"), 7500m.ToString(CultureInfo.InvariantCulture));

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

            ClickSaveButton(); // robust save

            // Leave Add page (Details or Home)
            var leftAdd = WaitUrl(url =>
                   url.Contains("/Shipments/Details", StringComparison.OrdinalIgnoreCase)
                || url.TrimEnd('/').Equals(BaseUrl.TrimEnd('/'), StringComparison.OrdinalIgnoreCase)
                || url.Contains("/Home", StringComparison.OrdinalIgnoreCase),
                timeoutSeconds: 12);
            Assert.That(leftAdd, Is.True, "Did not leave /Shipments/Add after saving.");

            // 3) Search the new shipment on All Shipments
            OpenAllAndSearch(title);
            var card = WaitForCardByTitle(title, timeoutSeconds: 15);
            Assert.That(card, Is.Not.Null, $"Expected to find a card with title '{title}'");

            // 4) Open Details first (more stable than clicking Edit on the grid)
            ClickLinkInCard(card!, "Details");
            WaitUntil(d => d.Url.Contains("/Shipments/Details", StringComparison.OrdinalIgnoreCase), seconds: 12);

            // 5) Click Edit from Details page
            ClickEditFromDetails();
            WaitUntil(d => d.Url.Contains("/Shipments/Edit", StringComparison.OrdinalIgnoreCase), seconds: 12);

            // 6) Edit the title and Save
            var editedTitle = title + " Just Edited-02";
            ClearAndType(By.Id("Title"), editedTitle);
            ClickSaveButton();

            // Back on Details after save
            WaitUntil(d => d.Url.Contains("/Shipments/Details", StringComparison.OrdinalIgnoreCase), seconds: 12);

            // 7) Verify with a fresh search on All Shipments
            OpenAllAndSearch(editedTitle);
            var editedCard = WaitForCardByTitle(editedTitle, timeoutSeconds: 15);
            Assert.That(editedCard, Is.Not.Null, $"Expected edited title '{editedTitle}' to appear in All search.");
        }

        // ---------- helpers ----------

        private void LoginWith(string email, string password)
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");
            WaitUntil(_ => Driver.Url.Contains("/Identity/Account/Login", StringComparison.OrdinalIgnoreCase), seconds: 12);

            Type(By.Id("Input_Email"), email);
            Type(By.Id("Input_Password"), password);

            SafeClick(Driver.FindElement(By.CssSelector("form button[type='submit'], form input[type='submit']")));
            WaitUntil(_ => Driver.PageSource.Contains("Logout", StringComparison.OrdinalIgnoreCase), seconds: 12);
        }

        private void OpenAllAndSearch(string term)
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Shipments/All");
            WaitUntil(d => d.Url.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase), seconds: 12);

            var searchBox = Driver.FindElement(By.Id("SearchTerm"));
            searchBox.Clear();
            searchBox.SendKeys(term);

            ClickSearchButton();
            WaitUntil(_ => Driver.Url.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase), seconds: 12); // results page reload
        }

        private IWebElement? WaitForCardByTitle(string title, int timeoutSeconds)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            try
            {
                return wait.Until(d =>
                    d.FindElements(By.CssSelector(".card"))
                     .FirstOrDefault(c => c.Text.Contains(title, StringComparison.OrdinalIgnoreCase)));
            }
            catch { return null; }
        }

        private void ClickLinkInCard(IWebElement card, string linkText)
        {
            // re-query inside the card just before clicking to avoid stale refs
            var candidate = card.FindElements(By.XPath($".//a[normalize-space()='{linkText}']")).FirstOrDefault();
            Assert.That(candidate, Is.Not.Null, $"Could not find '{linkText}' link within the card.");
            SafeClick(candidate!);
        }

        private void ClickEditFromDetails()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var edit = wait.Until(d =>
                d.FindElements(By.XPath("//a[normalize-space()='Edit' and contains(@href, '/Shipments/Edit')]")).FirstOrDefault()
                ?? d.FindElements(By.CssSelector("a.btn, a"))
                     .FirstOrDefault(a => a.Text.Trim().Equals("Edit", StringComparison.OrdinalIgnoreCase)));
            Assert.That(edit, Is.Not.Null, "Edit link not found on Details page.");
            SafeClick(edit!);
        }

        private void ClickSearchButton()
        {
            var btn = Driver.FindElements(By.XPath("//button[normalize-space()='Search']")).FirstOrDefault()
                     ?? Driver.FindElements(By.CssSelector("input[type='submit'][value='Search']")).FirstOrDefault()
                     ?? Driver.FindElements(By.CssSelector("form button[type='submit'], form input[type='submit']")).FirstOrDefault();

            Assert.That(btn, Is.Not.Null, "Could not locate the Search button on All Shipments.");
            SafeClick(btn!);
        }

        private void ClickSaveButton()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(12));
            var form = wait.Until(d =>
                d.FindElements(By.TagName("form")).FirstOrDefault(f => f.FindElements(By.Id("Title")).Any()));
            Assert.That(form, Is.Not.Null, "Could not locate the Add/Edit Shipment form (containing #Title).");

            IWebElement? save = null;
            foreach (var css in new[] { "input[type='submit'][value='Save']", "button[type='submit']", "input[type='submit']" })
            {
                var found = form!.FindElements(By.CssSelector(css)).FirstOrDefault();
                if (found != null) { save = found; break; }
            }
            Assert.That(save, Is.Not.Null, "Could not locate the Save button within the shipment form.");

            SafeClick(save!);

            // tiny fallback: if still on Add/Edit, submit the form directly
            System.Threading.Thread.Sleep(150);
            if (Driver.Url.Contains("/Shipments/Add", StringComparison.OrdinalIgnoreCase) ||
                Driver.Url.Contains("/Shipments/Edit", StringComparison.OrdinalIgnoreCase))
            {
                try { form!.Submit(); }
                catch { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].submit();", form); }
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

        private void SafeClick(IWebElement el)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(_ => el.Displayed && el.Enabled);

            try
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript(
                    "const r = arguments[0].getBoundingClientRect();" +
                    "window.scrollTo({top: r.top + window.pageYOffset - 120});", el);
            }
            catch { /* ignore */ }

            try { el.Click(); return; }
            catch (ElementClickInterceptedException) { /* try next */ }
            catch (WebDriverException) { /* try next */ }

            try
            {
                new Actions(Driver).MoveToElement(el, 1, 1).Pause(TimeSpan.FromMilliseconds(60)).Click().Perform();
                return;
            }
            catch { /* try next */ }

            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", el);
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
