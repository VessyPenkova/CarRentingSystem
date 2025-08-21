using OpenQA.Selenium;

namespace CarRentingSystem.SeleniumTests.Pages.Identity;

public class LoginPage
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;
    public LoginPage(IWebDriver driver, string baseUrl)
    {
        _driver = driver; _baseUrl = baseUrl;
    }

    public void Navigate(string? returnUrl = null)
    {
        var url = $"{_baseUrl}/Identity/Account/Login";
        if (!string.IsNullOrWhiteSpace(returnUrl))
            url += $"?returnUrl={Uri.EscapeDataString(returnUrl)}";
        _driver.Navigate().GoToUrl(url);
    }

    public void Fill(string email, string pwd)
    {
        _driver.FindElement(By.Id("Input_Email")).SendKeys(email);
        _driver.FindElement(By.Id("Input_Password")).SendKeys(pwd);
    }

    public void Submit() =>
        _driver.FindElement(By.CssSelector("button[type='submit'], #login-submit")).Click();
}
