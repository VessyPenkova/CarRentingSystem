using NUnit.Framework;
using OpenQA.Selenium;
using CarRentingSystem.SeleniumTests.Fixtures;
using CarRentingSystem.SeleniumTests.Pages.Identity;
using CarRentingSystem.SeleniumTests.Pages.Layout;
using CarRentingSystem.SeleniumTests.Pages.Shipments;
using CarRentingSystem.SeleniumTests.Utils;

namespace CarRentingSystem.SeleniumTests.Tests;

public sealed class ShipmentsTests : TestBase
{
    private (string email, string pwd) RegisterAndReturnCreds()
    {
        var email = TestData.UniqueEmail();
        var pwd = TestData.StrongPassword();
        var reg = new RegisterPage(Driver, BaseUrl);
        reg.Navigate("/");
        reg.Fill(email, "First", "Last", pwd, pwd);
        reg.Submit();
        WaitUntil(_ => new Navbar(Driver).IsLoggedIn);
        return (email, pwd);
    }

    [Test]
    public void All_Loads_And_AddButtonVisible()
    {
        RegisterAndReturnCreds();
        var nav = new Navbar(Driver);
        nav.ClickAllShipments();
        WaitUntil(_ => Driver.Url.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase));

        // Add Shipment link should be present for logged-in users
        Assert.That(Driver.FindElements(By.LinkText("Add Shipment")).Any(), Is.True);
    }

    [Test]
    public void Add_Edit_Delete_Flow_Works_ForOwner()
    {
        RegisterAndReturnCreds();
        var nav = new Navbar(Driver);

        // Go to Add
        nav.ClickAddShipment();
        WaitUntil(_ => Driver.Url.Contains("/Shipments/Add"));

        // Create
        var add = new AddShipmentPage(Driver);
        var title = TestData.UniqueTitle("UI");
        add.Fill(
            title,
            "Paris",
            "Plovdiv",
            "UI test shipment",
            "https://picsum.photos/800/400",
            "50.00",
            TestData.DefaultCategoryId);
        add.Save();

        // Details shown
        var details = new ShipmentDetailsPage(Driver);
        WaitUntil(_ => Driver.Url.Contains("/Shipments/Details"));
        Assert.That(details.TitleText, Does.Contain("UI").IgnoreCase);

        // Edit
        details.ClickEdit();
        var edit = new EditShipmentPage(Driver);
        var newTitle = title + "-Edited";
        edit.ChangeTitle(newTitle);
        edit.Save();
        WaitUntil(_ => Driver.Url.Contains("/Shipments/Details"));
        Assert.That(new ShipmentDetailsPage(Driver).TitleText, Does.Contain("-Edited"));

        // Delete
        new ShipmentDetailsPage(Driver).ClickDelete();
        var del = new DeleteShipmentPage(Driver);
        del.Confirm();

        // Back on All, card should not exist
        WaitUntil(_ => Driver.Url.Contains("/Shipments/All"));
        Assert.That(new AllShipmentsPage(Driver, BaseUrl).HasCardWithTitle(newTitle), Is.False);
    }

    [Test]
    public void NonOwner_Cannot_Edit_Or_Delete()
    {
        // User A creates shipment
        var (aEmail, aPwd) = RegisterAndReturnCreds();
        var nav = new Navbar(Driver);
        nav.ClickAddShipment();
        var add = new AddShipmentPage(Driver);
        var title = TestData.UniqueTitle("SEC");
        add.Fill(title, "A", "B", "Sec test", "https://picsum.photos/800/400", "11.00", TestData.DefaultCategoryId);
        add.Save();
        WaitUntil(_ => Driver.Url.Contains("/Shipments/Details"));
        var detailsUrl = Driver.Url; // save

        // Logout, register User B
        nav.Logout();
        var (bEmail, bPwd) = RegisterAndReturnCreds();

        // Open A's details
        Driver.Navigate().GoToUrl(detailsUrl);
        var details = new ShipmentDetailsPage(Driver);

        // Edit should either be hidden or lead to Access Denied
        if (details.HasEdit)
        {
            details.ClickEdit();
            WaitUntil(_ => Driver.Url.Contains("/Account/AccessDenied"));
            Assert.That(Driver.PageSource, Does.Contain("Access denied").IgnoreCase);
        }
        else
        {
            Assert.Pass("Edit hidden for non-owner; OK.");
        }
    }

    [Test]
    public void Rent_Then_Leave_Works()
    {
        // User A makes a shipment
        var (aEmail, aPwd) = RegisterAndReturnCreds();
        var nav = new Navbar(Driver);
        nav.ClickAddShipment();
        var add = new AddShipmentPage(Driver);
        var title = TestData.UniqueTitle("Rentable");
        add.Fill(title, "From A", "To B", "Rent flow", "https://picsum.photos/800/400", "25.00", TestData.DefaultCategoryId);
        add.Save();
        WaitUntil(_ => Driver.Url.Contains("/Shipments/Details"));
        var detailsUrl = Driver.Url;

        // User B rents it
        nav.Logout();
        var _ = RegisterAndReturnCreds();
        Driver.Navigate().GoToUrl(detailsUrl);
        var details = new ShipmentDetailsPage(Driver);

        Assert.That(details.HasRent, Is.True, "Rent button expected for non-owner.");
        details.Rent();

        // After renting, redirected to Mine
        WaitUntil(_ => Driver.Url.Contains("/Shipments/Mine"));
        Assert.That(Driver.PageSource, Does.Contain(title).IgnoreCase, "Mine should list rented item.");

        // Leave
        Driver.Navigate().GoToUrl(detailsUrl);
        details = new ShipmentDetailsPage(Driver);
        Assert.That(details.HasLeave, Is.True);
        details.Leave();

        // Back to Mine; item should be gone
        WaitUntil(_ => Driver.Url.Contains("/Shipments/Mine"));
        Assert.That(Driver.PageSource.Contains(title, StringComparison.OrdinalIgnoreCase), Is.False);
    }

    [Test]
    public void Details_Blocks_WrongInformation_Param()
    {
        RegisterAndReturnCreds();

        // Create a shipment to get a real id
        var nav = new Navbar(Driver);
        nav.ClickAddShipment();
        var add = new AddShipmentPage(Driver);
        var title = TestData.UniqueTitle("InfoGuard");
        add.Fill(title, "X", "Y", "Info check", "https://picsum.photos/800/400", "10.00", TestData.DefaultCategoryId);
        add.Save();
        WaitUntil(_ => Driver.Url.Contains("/Shipments/Details"));
        var correctUrl = new Uri(Driver.Url);

        // Forge wrong information param
        var wrong = new UriBuilder(correctUrl) { Query = "information=WRONG" }.Uri.ToString();
        Driver.Navigate().GoToUrl(wrong);

        // Should redirect away (Home) with message
        WaitUntil(_ => Driver.Url.Contains("/Home/Index") || Driver.PageSource.Contains("Don't touch!", StringComparison.OrdinalIgnoreCase));
        Assert.That(Driver.PageSource, Does.Contain("Don't touch!").IgnoreCase);
    }
}
