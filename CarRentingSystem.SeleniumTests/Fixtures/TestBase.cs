using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CarRentingSystem.SeleniumTests.Fixtures;

[TestFixture, Parallelizable(ParallelScope.Self)]
public abstract class TestBase
{
    protected ChromeDriver Driver = null!;
    protected WebDriverWait Wait = null!;
    protected string BaseUrl = null!;

    [SetUp]
    public void SetUp()
    {
        BaseUrl = Environment.GetEnvironmentVariable("TEST_BASE_URL")
                  ?? "https://localhost:7237";

        var opts = new ChromeOptions();
        opts.AddArgument("--headless=new");
        opts.AddArgument("--window-size=1920,1200");
        opts.AddArgument("--ignore-certificate-errors");

        Driver = new ChromeDriver(opts);
        Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
    }

    [TearDown]
    public void TearDown()
    {
        try { Driver?.Quit(); } catch { /* ignore */ }
        Driver?.Dispose();
    }

    protected void Go(string relative) => Driver.Navigate().GoToUrl($"{BaseUrl}{relative}");
    protected void WaitUntil(Func<IWebDriver, bool> cond) => Wait.Until(cond);
    protected bool Exists(By by) => Driver.FindElements(by).Any();
}
