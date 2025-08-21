using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using CarRentingSystem.SeleniumTests.Pages;

namespace CarRentingSystem.SeleniumTests;

[TestFixture]
public sealed class RegisterTests
{
    private ChromeDriver _driver = null!;
    private WebDriverWait _wait = null!;
    private string _baseUrl = null!;

    [SetUp]
    public void SetUp()
    {
        _baseUrl = Environment.GetEnvironmentVariable("TEST_BASE_URL")
                   ?? "https://localhost:7237";

        var options = new ChromeOptions();
        options.AddArgument("--headless=new");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--window-size=1920,1200");
        options.AddArgument("--ignore-certificate-errors");

        _driver = new ChromeDriver(options);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
    }

    [TearDown]
    public void TearDown()
    {
        try { _driver?.Quit(); }
        catch { /* ignore */ }
        finally { _driver?.Dispose(); }   // <- satisfies NUnit1032
    }

    private static string UniqueEmail() => $"test_{Guid.NewGuid():N}@example.test";
    private static string StrongPassword() => "Passw0rd!123";

    private bool IsLoggedIn() =>
        _driver.FindElements(By.XPath("//a[contains(.,'Logout') or contains(.,'Log out') or contains(.,'Log Out')]")).Any();

    private bool AnyValidationMessageContains(string text)
    {
        var nodes = _driver.FindElements(By.XPath(
            "//*[contains(@class,'text-danger') or contains(@class,'validation-summary-errors') or contains(@class,'field-validation-error')]"));
        return nodes.Any(n => n.Text.Contains(text, StringComparison.OrdinalIgnoreCase));
    }

    [Test]
    public void Register_WithValidData_SignsIn()
    {
        var page = new RegisterPage(_driver, _baseUrl);
        page.Navigate("/");
        var email = UniqueEmail();
        var pwd = StrongPassword();

        page.Fill(email, "Testy", "McTester", pwd, pwd);
        page.Submit();

        _wait.Until(_ => IsLoggedIn());
        Assert.That(IsLoggedIn(), Is.True);
    }

    [Test]
    public void Register_PasswordMismatch_ShowsValidation()
    {
        var page = new RegisterPage(_driver, _baseUrl);
        page.Navigate();
        var email = UniqueEmail();
        var pwd = StrongPassword();

        page.Fill(email, "Mismatch", "Case", pwd, pwd + "X");
        page.Submit();

        _wait.Until(_ => AnyValidationMessageContains("do not match"));
        Assert.That(AnyValidationMessageContains("do not match"), Is.True);
    }

    [Test]
    public void Register_MissingRequired_ShowsValidation()
    {
        var page = new RegisterPage(_driver, _baseUrl);
        page.Navigate();

        page.Fill("", "", "", "", "");
        page.Submit();

        _wait.Until(_ => AnyValidationMessageContains("required") || AnyValidationMessageContains("must be at least"));
        Assert.That(AnyValidationMessageContains("required") || AnyValidationMessageContains("must be at least"), Is.True);
    }
}
