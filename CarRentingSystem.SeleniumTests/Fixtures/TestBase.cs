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
        // Keep your existing server fixture; falls back if you replace it.
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

            try { Driver?.Quit(); } catch { /* ignore */ }
            try { Driver?.Dispose(); } catch { /* ignore */ }
        }

        /// <summary>
        /// Robust wait that ignores common transient Selenium exceptions while evaluating <paramref name="condition"/>.
        /// </summary>
        protected void WaitUntil(Func<IWebDriver, bool> condition, int seconds = 10, string? onTimeout = null)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(seconds))
            {
                PollingInterval = TimeSpan.FromMilliseconds(250)
            };

            wait.IgnoreExceptionTypes(
                typeof(NoSuchElementException),
                typeof(StaleElementReferenceException),
                typeof(ElementNotInteractableException),
                typeof(ElementClickInterceptedException)
            );

            try
            {
                wait.Until(d =>
                {
                    try { return condition(d); }
                    catch (NoSuchElementException) { return false; }
                    catch (StaleElementReferenceException) { return false; }
                    catch (ElementNotInteractableException) { return false; }
                    catch (WebDriverException) { return false; }
                });
            }
            catch (WebDriverTimeoutException)
            {
                onTimeout ??= "WaitUntil timed out.";
                Assert.Fail($"{onTimeout}\nURL: {Driver.Url}");
            }
        }

        /// <summary>Wait for a URL predicate; returns true on success (no Assert).</summary>
        protected bool WaitUrl(Func<string, bool> predicate, int timeoutSeconds)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(d => predicate(d.Url));
            }
            catch { return false; }
        }

        // ---------- diagnostics ----------

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
                    File.WriteAllBytes(png, shot.AsByteArray); // works across Selenium versions
                    TestContext.AddTestAttachment(png, "Screenshot on failure");
                }

                // HTML
                var htmlPath = Path.Combine(dir, $"{Sanitize(TestContext.CurrentContext.Test.Name)}.html");
                File.WriteAllText(htmlPath, Driver.PageSource);
                TestContext.AddTestAttachment(htmlPath, "HTML on failure");

                // URL to console
                TestContext.WriteLine("Failing URL: " + Driver.Url);
            }
            catch { /* best effort only */ }
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
            // opts.AddArgument("--headless=new"); // enable if desired
            opts.AddArgument("--window-size=1280,900");
            opts.AddArgument("--ignore-certificate-errors");
            opts.AddArgument("--no-sandbox");
            opts.AddArgument("--disable-dev-shm-usage");
            return new ChromeDriver(opts);
        }
    }
}
