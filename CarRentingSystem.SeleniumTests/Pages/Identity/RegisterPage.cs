using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CarRentingSystem.SeleniumTests.Pages.Identity
{
    public sealed class RegisterPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _baseUrl;

        private static readonly By Email = By.Id("Input_Email");
        private static readonly By Password = By.Id("Input_Password");
        private static readonly By Confirm = By.Id("Input_ConfirmPassword");
        private static readonly By FirstName = By.Id("Input_FirstName");
        private static readonly By LastName = By.Id("Input_LastName");
        private static readonly By Submit = By.CssSelector("form button[type='submit']");

        public RegisterPage(IWebDriver driver, string baseUrl, int waitSeconds = 10)
        {
            _driver = driver;
            _baseUrl = baseUrl.TrimEnd('/');
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitSeconds));
        }

        public void Open() => Open(null);

        public void Open(string? returnUrl)
        {
            var url = $"{_baseUrl}/Identity/Account/Register";
            if (!string.IsNullOrWhiteSpace(returnUrl))
                url += $"?returnUrl={Uri.EscapeDataString(returnUrl)}";

            _driver.Navigate().GoToUrl(url);
            _wait.Until(d => d.FindElement(Email).Displayed);
        }

        public void Fill(string email, string first, string last, string password, string confirm)
        {
            Type(Email, email);
            Type(FirstName, first);
            Type(LastName, last);
            Type(Password, password);
            Type(Confirm, confirm);
        }

        public void SubmitNow() => _driver.FindElement(Submit).Click();

        private void Type(By by, string value)
        {
            var el = _wait.Until(d => d.FindElement(by));
            el.Clear();
            el.SendKeys(value);
        }
    }
}
