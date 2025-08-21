using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CarRentingSystem.SeleniumTests.Pages.Shipments
{
    public class EditShipmentPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Field locators (same IDs as the Add view)
        private static readonly By Title = By.Id("Title");
        private static readonly By LoadingAddress = By.Id("LoadingAddress");
        private static readonly By DeliveryAddress = By.Id("DeliveryAddress");
        private static readonly By Description = By.Id("Description");
        private static readonly By ImageUrl = By.Id("ImageUrlShipmentGoogleMaps");
        private static readonly By Price = By.Id("Price");
        private static readonly By Category = By.Id("CategoryId");

        // Save button (input[type=submit] value="Save" or a button with text Save)
        private static readonly By SaveBtn =
            By.XPath("//input[@type='submit' and @value='Save'] | //button[normalize-space()='Save'] | //input[@type='submit']");

        public EditShipmentPage(IWebDriver driver, int waitSeconds = 10)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitSeconds));

            // Ensure the form is visible
            _wait.Until(d => d.FindElement(Title).Displayed);
        }

        // ---- granular setters (fluent) ----
        public EditShipmentPage SetTitle(string value) { Type(Title, value); return this; }
        public EditShipmentPage SetLoadingAddress(string value) { Type(LoadingAddress, value); return this; }
        public EditShipmentPage SetDeliveryAddress(string value) { Type(DeliveryAddress, value); return this; }
        public EditShipmentPage SetDescription(string value) { Type(Description, value); return this; }
        public EditShipmentPage SetImageUrl(string value) { Type(ImageUrl, value); return this; }
        public EditShipmentPage SetPrice(string value) { Type(Price, value); return this; }

        public EditShipmentPage SelectCategoryByText(string text)
        {
            new SelectElement(_driver.FindElement(Category)).SelectByText(text);
            return this;
        }

        public EditShipmentPage SelectCategoryByValue(string value)
        {
            new SelectElement(_driver.FindElement(Category)).SelectByValue(value);
            return this;
        }

        // ---- one-shot change-everything helper ----
        /// <summary>
        /// Change all fields in one call. Pass either the visible text of the category
        /// (e.g., "Charter") or the value (e.g., "1"). When both are provided, value wins.
        /// </summary>
        public EditShipmentPage ChangeAllFields(
            string title,
            string loadingAddress,
            string deliveryAddress,
            string description,
            string imageUrl,
            string price,
            string? categoryText = null,
            string? categoryValue = null)
        {
            return SetTitle(title)
                 .SetLoadingAddress(loadingAddress)
                 .SetDeliveryAddress(deliveryAddress)
                 .SetDescription(description)
                 .SetImageUrl(imageUrl)
                 .SetPrice(price)
                 .ApplyCategory(categoryText, categoryValue);
        }

        private EditShipmentPage ApplyCategory(string? text, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                return SelectCategoryByValue(value);

            if (!string.IsNullOrWhiteSpace(text))
                return SelectCategoryByText(text);

            return this; // leave unchanged if neither provided
        }

        // ---- actions ----
        public void Save()
        {
            _driver.FindElement(SaveBtn).Click();
        }

        public void SaveAndWaitForDetails()
        {
            Save();
            _wait.Until(d =>
                d.Url.Contains("/Shipments/Details", StringComparison.OrdinalIgnoreCase) ||
                d.Url.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase));
        }

        // ---- small helpers ----
        private void Type(By by, string value)
        {
            var el = _wait.Until(d => d.FindElement(by));
            el.Clear();
            el.SendKeys(value);
        }

        // Expose current title if you want to assert on it
        public string CurrentTitle => _driver.FindElement(Title).GetAttribute("value") ?? string.Empty;
    }
}
