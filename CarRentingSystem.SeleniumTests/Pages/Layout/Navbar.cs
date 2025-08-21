using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CarRentingSystem.SeleniumTests.Pages.Layout
{
    public sealed class Navbar
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        // Common Identity selectors (cover both scaffolded defaults and custom layouts)
        private static readonly By LogoutButtonInForm = By.CssSelector("form[action*='/Identity/Account/Logout'] button[type='submit'], form#logoutForm button[type='submit']");
        private static readonly By LogoutLink = By.LinkText("Logout");
        private static readonly By ManageLink = By.CssSelector("a[href*='/Identity/Account/Manage']");
        private static readonly By LoginLink = By.CssSelector("a[href*='/Identity/Account/Login'], a[href*='/Account/Login']");
        private static readonly By AllShipmentsLink = By.CssSelector("a[href*='/Shipments/All']");
        private static readonly By AddShipmentLink = By.CssSelector("a[href*='/Shipments/Add']");

        public Navbar(IWebDriver driver, int waitSeconds = 10)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitSeconds));
        }

        // Heuristics to infer auth state
        public bool IsLoggedIn =>
            Any(LogoutButtonInForm) || Any(LogoutLink) || Any(ManageLink);

        public bool IsLoggedOut =>
            Any(LoginLink);

        public bool WaitForLoggedIn(int seconds = 20)
        {
            try
            {
                new WebDriverWait(_driver, TimeSpan.FromSeconds(seconds))
                    .Until(d => IsLoggedIn);
                return true;
            }
            catch { return false; }
        }

        public void ClickAllShipments()
        {
            var el = First(AllShipmentsLink) ?? First(By.LinkText("All Shipments"));
            el?.Click();
        }

        public void ClickAddShipment()
        {
            var el = First(AddShipmentLink) ?? First(By.LinkText("Add Shipment"));
            el?.Click();
        }

        public void Logout()
        {
            // Try link first
            var link = First(LogoutLink);
            if (link != null) { link.Click(); return; }

            // Then try the default form post
            var btn = First(LogoutButtonInForm);
            if (btn != null) { btn.Click(); return; }
        }

        private bool Any(By by)
        {
            try { return _driver.FindElements(by).Any(e => e.Displayed); }
            catch { return false; }
        }

        private IWebElement? First(By by)
        {
            try { return _driver.FindElements(by).FirstOrDefault(e => e.Displayed); }
            catch { return null; }
        }
    }
}
