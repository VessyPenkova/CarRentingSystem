using OpenQA.Selenium;

namespace CarRentingSystem.SeleniumTests.Pages.Identity;

public class RegisterPage
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;
    public RegisterPage(IWebDriver driver, string baseUrl)
    {
        _driver = driver; _baseUrl = baseUrl;
    }

    public void Navigate(string? returnUrl = null)
    {
        var url = $"{_baseUrl}/Identity/Account/Register";
        if (!string.IsNullOrWhiteSpace(returnUrl))
            url += $"?returnUrl={Uri.EscapeDataString(returnUrl)}";
        _driver.Navigate().GoToUrl(url);
    }

    public void Fill(string email, string first, string last, string pwd, string confirm)
    {
        _driver.FindElement(By.Id("Input_Email")).SendKeys(email);
        _driver.FindElement(By.Id("Input_FirstName")).SendKeys(first);
        _driver.FindElement(By.Id("Input_LastName")).SendKeys(last);
        _driver.FindElement(By.Id("Input_Password")).SendKeys(pwd);
        _driver.FindElement(By.Id("Input_ConfirmPassword")).SendKeys(confirm);
    }

    public void Submit() =>
        _driver.FindElement(By.CssSelector("button[type='submit'], #registerSubmit")).Click();
}
