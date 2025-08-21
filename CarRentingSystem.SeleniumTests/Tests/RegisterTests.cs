using NUnit.Framework;
using OpenQA.Selenium;
using CarRentingSystem.SeleniumTests.Fixtures;
using CarRentingSystem.SeleniumTests.Pages.Identity;
using CarRentingSystem.SeleniumTests.Pages.Layout;
using CarRentingSystem.SeleniumTests.Utils;

namespace CarRentingSystem.SeleniumTests.Tests;

public sealed class RegisterTests : TestBase
{
    [Test, Category("ui"), Category("smoke")]
    public void Register_WithValidData_SignsInAndLandsOnHome()
    {
        var reg = new RegisterPage(Driver, BaseUrl);
        reg.Navigate("/");
        var email = TestData.UniqueEmail();
        var pwd = TestData.StrongPassword();

        reg.Fill(email, "Test", "User", pwd, pwd);
        reg.Submit();

        // User should be logged in (Logout visible)
        var nav = new Navbar(Driver);
        WaitUntil(d => nav.IsLoggedIn);
        Assert.That(nav.IsLoggedIn, Is.True);
    }
}
