using NUnit.Framework;
using CarRentingSystem.SeleniumTests.Fixtures;
using CarRentingSystem.SeleniumTests.Pages.Identity;
using CarRentingSystem.SeleniumTests.Pages.Layout;
using CarRentingSystem.SeleniumTests.TestData;

namespace CarRentingSystem.SeleniumTests.Tests
{
    [TestFixture, Order(0)]
    public sealed class A_RegisterTests : TestBase
    {
        [Test, Category("ui"), Category("smoke")]
        public void Register_WithValidData_SignsInAndLandsOnHome()
        {
            var reg = new RegisterPage(Driver, BaseUrl);
            reg.Open(); // /Identity/Account/Register

            var data = RegisterTestData.Valid();
            reg.Fill(data.Email, data.FirstName, data.LastName, data.Password, data.ConfirmPassword);
            reg.SubmitNow();

            var nav = new Navbar(Driver);
            Assert.That(nav.WaitForLoggedIn(20), Is.True, "Expected to be logged-in after register, but navbar did not show a Manage/Logout control in time.");
        }
    }
}
