using NUnit.Framework;
using CarRentingSystem.SeleniumTests.Fixtures;
using CarRentingSystem.SeleniumTests.Pages.Identity;
using CarRentingSystem.SeleniumTests.Pages.Layout;

namespace CarRentingSystem.SeleniumTests.Tests
{
    [TestFixture, Order(1)]
    public sealed class B_LoginTests : TestBase
    {
        // Your existing, already-registered account:
        private const string ExistingEmail = "TransportCompanyPIMK_Test01@pimk.com";
        private const string ExistingPassword = "123456";

        [Test, Category("ui"), Category("auth")]
        public void Login_WithValidCredentials_Succeeds()
        {
            // Ensure we start from logged-out state
            Driver.Navigate().GoToUrl(BaseUrl + "/");
            var nav = new Navbar(Driver);
            if (nav.IsLoggedIn)
            {
                nav.Logout();
                WaitUntil(_ => !new Navbar(Driver).IsLoggedIn);
            }

            // Go to login and sign in
            var login = new LoginPage(Driver, BaseUrl);
            login.Open("/"); // return to Home after login
            login.Fill(ExistingEmail, ExistingPassword);
            login.Submit();

            // Assert logged in (Logout visible in navbar)
            nav = new Navbar(Driver);
            WaitUntil(_ => nav.IsLoggedIn);
            Assert.That(nav.IsLoggedIn, Is.True, "User should be logged in and see Logout in the navbar.");
        }
    }
}
