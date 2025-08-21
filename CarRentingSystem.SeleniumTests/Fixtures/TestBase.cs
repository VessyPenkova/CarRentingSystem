using System;
using System.IO;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CarRentingSystem.SeleniumTests.Fixtures
{
    public abstract class TestBase
    {
        protected IWebDriver Driver = null!;
        protected string BaseUrl = ServerFixture.BaseUrl;

        [SetUp]
        public virtual void SetUp()
        {
            Driver = WebDriverFactory.CreateChrome();
        }

        [TearDown]
        public virtual void TearDown()
        {
            // If failed, capture diagnostics
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;
            if (outcome == TestStatus.Failed)
            {
                TryDumpArtifacts();
            }

            try { Driver?.Quit(); } catch { }
            try { Driver?.Dispose(); } catch { }
        }

        protected void WaitUntil(Func<IWebDriver, bool> condition, int seconds = 10)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds));
            wait.Until(d => condition(d));
        }

        private void TryDumpArtifacts()
        {
            try
            {
                var dir = Path.Combine(TestContext.CurrentContext.WorkDirectory, "artifacts");
                Directory.CreateDirectory(dir);

                // Screenshot
                if (Driver is ITakesScreenshot taker)
                {
                    var shot = taker.GetScreenshot();
                    var png = Path.Combine(dir, $"{Sanitize(TestContext.CurrentContext.Test.Name)}.png");

                    // Works across Selenium versions
                    File.WriteAllBytes(png, shot.AsByteArray);

                    TestContext.AddTestAttachment(png, "Screenshot on failure");
                }

                // HTML
                var htmlPath = Path.Combine(dir, $"{Sanitize(TestContext.CurrentContext.Test.Name)}.html");
                File.WriteAllText(htmlPath, Driver.PageSource);
                TestContext.AddTestAttachment(htmlPath, "HTML on failure");

                // URL
                TestContext.WriteLine("Failing URL: " + Driver.Url);
            }
            catch { /* best effort */ }
        }

        private static string Sanitize(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }
    }

    internal static class WebDriverFactory
    {
        public static IWebDriver CreateChrome()
        {
            var opts = new ChromeOptions { AcceptInsecureCertificates = true };
            // opts.AddArgument("--headless=new"); // uncomment for headless runs
            opts.AddArgument("--window-size=1280,900");
            opts.AddArgument("--ignore-certificate-errors");
            opts.AddArgument("--no-sandbox");
            opts.AddArgument("--disable-dev-shm-usage");
            return new ChromeDriver(opts);
        }
    }
}
