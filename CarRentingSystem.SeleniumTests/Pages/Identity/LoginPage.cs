using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CarRentingSystem.SeleniumTests.Pages.Identity
{
    public sealed class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _baseUrl;

        // Default ASP.NET Identity ids
        private static readonly By Email = By.Id("Input_Email");
        private static readonly By Password = By.Id("Input_Password");
        private static readonly By Remember = By.Id("Input_RememberMe");

        // Common submit button patterns for the Identity login page
        private static readonly By[] SubmitCandidates =
        {
            By.CssSelector("#login-submit"),
            By.CssSelector("form button[type='submit']"),
            By.CssSelector("form input[type='submit']")
        };

        // Validation summary (for negative tests if you add them later)
        private static readonly By ValidationSummary =
            By.CssSelector(".validation-summary-errors, div[asp-validation-summary='All']");

        public LoginPage(IWebDriver driver, string baseUrl, int waitSeconds = 10)
        {
            _driver = driver;
            _baseUrl = baseUrl.TrimEnd('/');
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitSeconds));
        }

        /// <summary>Open /Identity/Account/Login (optionally with returnUrl).</summary>
        public void Open(string? returnUrl = "/")
        {
            var url = $"{_baseUrl}/Identity/Account/Login";
            if (!string.IsNullOrWhiteSpace(returnUrl))
                url += $"?returnUrl={Uri.EscapeDataString(returnUrl)}";

            _driver.Navigate().GoToUrl(url);
            _wait.Until(d => d.FindElement(Email).Displayed);
        }

        public void Fill(string email, string password, bool rememberMe = false)
        {
            Type(Email, email);
            Type(Password, password);
            if (rememberMe)
            {
                var el = _driver.FindElement(Remember);
                if (!el.Selected) el.Click();
            }
        }

        public void Submit()
        {
            foreach (var by in SubmitCandidates)
            {
                var btn = _driver.FindElements(by).FirstOrDefault();
                if (btn != null) { btn.Click(); return; }
            }
            throw new NoSuchElementException("Login submit button not found.");
        }

        public string? SummaryErrorText()
            => _driver.FindElements(ValidationSummary).FirstOrDefault()?.Text;

        // helpers
        private void Type(By locator, string value)
        {
            var el = _wait.Until(d => d.FindElement(locator));
            el.Clear();
            el.SendKeys(value);
        }
    }
}