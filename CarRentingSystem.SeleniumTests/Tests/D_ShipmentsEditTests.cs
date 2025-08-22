using CarRentingSystem.SeleniumTests.Fixtures;
using CarRentingSystem.SeleniumTests.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Globalization;

namespace CarRentingSystem.SeleniumTests.Tests
{
    [TestFixture]
    public sealed class D_EditShipmentTests : TestBase
    {
        [Test]
        public void AddNewShipment_Then_EditTitle_ShowsEditedInAllSearch()
        {
            // 1) login
            LoginWith(TestUsers.Email, TestUsers.Password);

            // 2) add a shipment
            var originalTitle = "Plovdiv-Valence";

            AddShipment(originalTitle,
                        "Plovdiv",
                        "Valence",
                        $"Charter {originalTitle}",
                        "https://www.hispaviacion.es/wp-content/uploads/2022/05/Falcon2000.jpeg",
                        7500m,
                        preferCategoryText: "Charter");

            // 3) go to All and search for {originalTitle} it
            Driver.Navigate().GoToUrl($"{BaseUrl}/Shipments/All");
            WaitUntil(d => d.Url.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase));
            SearchAllByTitle(originalTitle);

            // sanity: we can see the card
            var card = FindCardByTitle(originalTitle);
            Assert.That(card, Is.Not.Null, $"Expected to find a card with title '{originalTitle}'");

            // 4) click Edit on the right card
            ClickEditOnCard(originalTitle);

            // 5) edit the title only (append 01) and Save
            var editedTitle = originalTitle + "01";
            ClearAndType(By.Id("Title"), editedTitle);
            ClickSaveButton();

            // after save you may land on Details or Home – accept both
            WaitUrl(u =>
                   u.Contains("/Shipments/Details", StringComparison.OrdinalIgnoreCase)
                || u.TrimEnd('/').Equals(BaseUrl.TrimEnd('/'), StringComparison.OrdinalIgnoreCase),
                timeoutSeconds: 12);

            // 6) verify via search
            Driver.Navigate().GoToUrl($"{BaseUrl}/Shipments/All");
            WaitUntil(d => d.Url.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase));
            SearchAllByTitle(editedTitle);

            WaitUntil(d => d.PageSource.Contains(editedTitle, StringComparison.OrdinalIgnoreCase));
            Assert.That(Driver.PageSource, Does.Contain(editedTitle).IgnoreCase,
                "Edited title should appear in All Shipments search results.");
        }

        // -------- helpers (only what this test needs) --------

        private void LoginWith(string email, string password)
        {
            Driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");
            WaitUntil(_ => Driver.Url.Contains("/Identity/Account/Login", StringComparison.OrdinalIgnoreCase));
            ClearAndType(By.Id("Input_Email"), email);
            ClearAndType(By.Id("Input_Password"), password);
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

            ClickSaveButton();

            // consider both Details or Home after save
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

        private void SearchAllByTitle(string title)
        {
            var box = Driver.FindElement(By.Id("SearchTerm"));
            box.Clear();
            box.SendKeys(title);

            var btn = Driver.FindElements(By.XPath("//button[normalize-space()='Search']")).FirstOrDefault()
                     ?? Driver.FindElements(By.CssSelector("input[type='submit'][value='Search']")).FirstOrDefault()
                     ?? Driver.FindElements(By.CssSelector("form button[type='submit'], form input[type='submit']")).FirstOrDefault();
            Assert.That(btn, Is.Not.Null, "Could not locate the Search button.");
            btn.Click();
        }

        private IWebElement? FindCardByTitle(string title)
        {
            // find the first container that looks like a card and contains the title text
            var xp = $"//*[self::div or self::article][.//text()[contains(., {XPathLiteral(title)})] and .//a[normalize-space()='Details']]";
            return Driver.FindElements(By.XPath(xp)).FirstOrDefault();
        }

        private void ClickEditOnCard(string title)
        {
            // robust: find an Edit link that belongs to a container which contains the title
            var xp = $"//a[normalize-space()='Edit' and ancestor::*[contains(., {XPathLiteral(title)})]]";
            var link = Driver.FindElements(By.XPath(xp)).FirstOrDefault();
            Assert.That(link, Is.Not.Null, $"Could not find Edit button for '{title}'.");

            try
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", link);
            }
            catch { /* ignore */ }

            try { link!.Click(); }
            catch (WebDriverException) { ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", link); }

            WaitUntil(d => d.Url.Contains("/Shipments/Edit", StringComparison.OrdinalIgnoreCase), seconds: 12);
        }

        private void ClearAndType(By by, string value)
        {
            var el = Driver.FindElement(by);
            el.Clear();
            el.SendKeys(value);
        }

        private void ClickSaveButton()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var form = wait.Until(d => d.FindElements(By.TagName("form")).FirstOrDefault(f => f.FindElements(By.Id("Title")).Any()));
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

            // last resort
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
            // concat with quoted single quotes inside
            return "concat('" + value.Replace("'", "',\"'\",'") + "')";
        }
    }
}
