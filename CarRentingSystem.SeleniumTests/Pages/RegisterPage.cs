using OpenQA.Selenium;

namespace CarRentingSystem.SeleniumTests.Pages;

public sealed class RegisterPage
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;

    public RegisterPage(IWebDriver driver, string baseUrl)
    {
        _driver = driver;
        _baseUrl = baseUrl.TrimEnd('/');
    }

    private By EmailInput => By.Id("Input_Email");
    private By FirstNameInput => By.Id("Input_FirstName");
    private By LastNameInput => By.Id("Input_LastName");
    private By PasswordInput => By.Id("Input_Password");
    private By ConfirmPasswordInput => By.Id("Input_ConfirmPassword");
    private By SubmitButton => By.CssSelector("button[type='submit'], input[type='submit'], #registerSubmit");

    public void Navigate(string? returnUrl = "/")
    {
        var url = $"{_baseUrl}/Identity/Account/Register";
        if (!string.IsNullOrWhiteSpace(returnUrl))
        {
            url += $"?returnUrl={Uri.EscapeDataString(returnUrl)}";
        }
        _driver.Navigate().GoToUrl(url);
    }

    public void Fill(string email, string firstName, string lastName, string password, string confirmPassword)
    {
        _driver.FindElement(EmailInput).Clear();
        _driver.FindElement(EmailInput).SendKeys(email);

        _driver.FindElement(FirstNameInput).Clear();
        _driver.FindElement(FirstNameInput).SendKeys(firstName);

        _driver.FindElement(LastNameInput).Clear();
        _driver.FindElement(LastNameInput).SendKeys(lastName);

        _driver.FindElement(PasswordInput).Clear();
        _driver.FindElement(PasswordInput).SendKeys(password);

        _driver.FindElement(ConfirmPasswordInput).Clear();
        _driver.FindElement(ConfirmPasswordInput).SendKeys(confirmPassword);
    }

    public void Submit() => _driver.FindElement(SubmitButton).Click();
}
