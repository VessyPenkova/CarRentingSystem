using System;
using System.Globalization;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CarRentingSystem.SeleniumTests.Pages.Shipments
{
    public sealed class AddShipmentPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _baseUrl;

        // Inputs (ids come from asp-for fields in your partial)
        private static readonly By Title = By.Id("Title");
        private static readonly By LoadingAddress = By.Id("LoadingAddress");
        private static readonly By DeliveryAddress = By.Id("DeliveryAddress");
        private static readonly By Description = By.Id("Description");
        private static readonly By ImageUrl = By.Id("ImageUrlShipmentGoogleMaps");
        private static readonly By Price = By.Id("Price");
        private static readonly By CategoryId = By.Id("CategoryId");

        // Save submit in your form
        private static readonly By SaveButton =
            By.CssSelector("form[method='post'] input[type='submit'][value='Save'], input[type='submit'][value='Save']");

        public AddShipmentPage(IWebDriver driver, string baseUrl, int waitSeconds = 10)
        {
            _driver = driver;
            _baseUrl = baseUrl.TrimEnd('/');
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitSeconds));
        }

        public void Open()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Shipments/Add");
            _wait.Until(d => d.FindElement(Title).Displayed);
        }

        public void Fill(
            string title,
            string from,
            string to,
            string desc,
            string imageUrl,
            decimal price,
            string categoryVisibleTextOrValue)
        {
            Type(Title, title);
            Type(LoadingAddress, from);
            Type(DeliveryAddress, to);
            Type(Description, desc);
            Type(ImageUrl, imageUrl);

            var priceBox = _driver.FindElement(Price);
            priceBox.SendKeys(Keys.Control + "a");
            priceBox.SendKeys(Keys.Delete);
            priceBox.SendKeys(price.ToString(CultureInfo.InvariantCulture));

            var select = new SelectElement(_driver.FindElement(CategoryId));

            // Try visible text first (e.g., "Charter"); fallback to value (e.g., "5"); then fallback to first option
            var matchedByText = select.Options.FirstOrDefault(o =>
                string.Equals(o.Text?.Trim(), categoryVisibleTextOrValue.Trim(), StringComparison.OrdinalIgnoreCase));
            if (matchedByText != null)
            {
                select.SelectByText(matchedByText.Text);
            }
            else
            {
                try { select.SelectByValue(categoryVisibleTextOrValue); }
                catch { select.SelectByIndex(0); }
            }
        }

        public void Submit()
        {
            var save = _wait.Until(d => d.FindElement(SaveButton));
            // bring into view to avoid “element not clickable” flakiness
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", save);
            save.Click();
        }

        private void Type(By by, string value)
        {
            var el = _wait.Until(d => d.FindElement(by));
            el.Clear();
            el.SendKeys(value);
        }
    }
}
