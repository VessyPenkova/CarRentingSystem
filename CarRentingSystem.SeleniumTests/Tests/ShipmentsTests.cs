using CarRentingSystem.SeleniumTests.Fixtures;
using CarRentingSystem.SeleniumTests.Pages.Identity;
using CarRentingSystem.SeleniumTests.Pages.Layout;
using CarRentingSystem.SeleniumTests.Pages.Shipments;
using CarRentingSystem.SeleniumTests.TestData;
using CarRentingSystem.SeleniumTests.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CarRentingSystem.SeleniumTests.Tests;

public sealed class ShipmentsTests : TestBase
{
    /// <summary>Registers a user and waits until the navbar shows "Logout".</summary>
    private void RegisterAndLandOnHome()
    {
        var reg = new RegisterPage(Driver, BaseUrl);
        reg.Open(); // /Identity/Account/Register
        var data = RegisterTestData.Valid();

        reg.Fill(data.Email, data.FirstName, data.LastName, data.Password, data.ConfirmPassword);
        reg.SubmitNow();
        

        var nav = new Navbar(Driver);

        Assert.That(nav.WaitForLoggedIn(20), Is.True, "Expected to be logged-in after register, but navbar did not show a Manage/Logout control in time.");

        WaitUntil(_ => nav.IsLoggedIn);     // should land on "/"
        Assert.That(nav.IsLoggedIn, Is.True);    
    }
    [Test]
    public void AddNewShipmentWithValidData_ShowsUpInAllSearch()
    {
        // 1) register & sign in
        RegisterAndLandOnHome();

        // 2) open Add Shipment
        new Navbar(Driver).ClickAddShipment();
        WaitUntil(_ => Driver.Url.Contains("/Shipments/Add", StringComparison.OrdinalIgnoreCase));

        // unique title so the search is deterministic
        var unique = Guid.NewGuid().ToString("N")[..6];
        var title = $"Plovdiv-Valence-{unique}";
        var image = "https://www.hispaviacion.es/wp-content/uploads/2022/05/Falcon2000.jpeg";

        // 3) fill the form (ids come from asp-for: Title, LoadingAddress, DeliveryAddress, etc.)
        Driver.FindElement(By.Id("Title")).SendKeys(title);
        Driver.FindElement(By.Id("LoadingAddress")).SendKeys("Plovdiv");
        Driver.FindElement(By.Id("DeliveryAddress")).SendKeys("Valence");
        Driver.FindElement(By.Id("Description")).SendKeys("Charter " + title);
        Driver.FindElement(By.Id("ImageUrlShipmentGoogleMaps")).SendKeys(image);

        var price = Driver.FindElement(By.Id("Price"));
        price.Clear();
        price.SendKeys("7500");

        // Category: prefer visible text; fall back to known seed value (Charter is usually id=5)
        var cat = new SelectElement(Driver.FindElement(By.Id("CategoryId")));
        try { cat.SelectByText("Charter"); }
        catch { cat.SelectByValue("5"); }

        // submit (Save)
        Driver.FindElement(By.CssSelector("input[type='submit'][value='Save'],button[type='submit']")).Click();

        // 4) redirected to Home
        var expectedHome = BaseUrl.TrimEnd('/');
        WaitUntil(_ =>
            Driver.Url.TrimEnd('/').Equals(expectedHome, StringComparison.OrdinalIgnoreCase) ||
            Driver.Url.Contains("/Home", StringComparison.OrdinalIgnoreCase));

        // 5) open All Shipments and search by the exact title
        new Navbar(Driver).ClickAllShipments();
        WaitUntil(_ => Driver.Url.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase));

        var search = Driver.FindElement(By.Id("SearchTerm"));
        search.Clear();
        search.SendKeys(title);

        Driver.FindElement(By.CssSelector("form input[type='submit'], form button[type='submit']")).Click();

        // assert the results contain our new shipment
        WaitUntil(_ => Driver.PageSource.Contains(title, StringComparison.OrdinalIgnoreCase));
        Assert.That(Driver.PageSource, Does.Contain(title).IgnoreCase);
    }


    [Test]
    public void EditShipment_SearchFromAll_UpdatesAllFields()
    {
        // 1) Register and log in
        RegisterAndLandOnHome();

        // 2) Add the shipment: Plovdiv-Valence
        var nav = new Navbar(Driver);
        nav.ClickAddShipment();
        WaitUntil(_ => Driver.Url.Contains("/Shipments/Add"));

        Driver.FindElement(By.Id("Title")).SendKeys("Plovdiv-Valence");
        Driver.FindElement(By.Id("LoadingAddress")).SendKeys("Plovdiv");
        Driver.FindElement(By.Id("DeliveryAddress")).SendKeys("Valence");
        Driver.FindElement(By.Id("Description")).SendKeys("Plovdiv-Valence-Charter");

        // any valid https image is fine
        Driver.FindElement(By.Id("ImageUrlShipmentGoogleMaps"))
              .SendKeys("https://picsum.photos/1200/600");

        var price = Driver.FindElement(By.Id("Price"));
        price.Clear(); price.SendKeys("7500");

        var cat = new SelectElement(Driver.FindElement(By.Id("CategoryId")));
        try { cat.SelectByText("Charter"); }
        catch { cat.SelectByValue("5"); } // fallback if text changes

        // Save (submit)
        Driver.FindElement(By.CssSelector("input[type='submit'][value='Save'],button[type='submit']")).Click();

        // App redirects to Home after create
        WaitUntil(_ => Driver.Url.StartsWith(BaseUrl, StringComparison.OrdinalIgnoreCase));

        // 3) Open All Shipments and search by title
        nav.ClickAllShipments();
        WaitUntil(_ => Driver.Url.Contains("/Shipments/All"));
        var search = Driver.FindElement(By.Id("SearchTerm"));
        search.Clear(); search.SendKeys("Plovdiv-Valence");
        Driver.FindElement(By.CssSelector("form input[type='submit'], form button[type='submit']")).Click();

        // 4) Click the Edit of the card that contains our title
        // Robust XPath: an <a>Edit</a> whose ancestor block contains the title text
        WaitUntil(_ => Driver.PageSource.Contains("Plovdiv-Valence", StringComparison.OrdinalIgnoreCase) &&
                       Driver.FindElements(By.LinkText("Edit")).Count > 0);

        var editLink = Driver.FindElement(By.XPath(
            "//a[normalize-space()='Edit' and ancestor::*[contains(normalize-space(.), 'Plovdiv-Valence')]]"));
        editLink.Click();

        // 5) On Edit page, change LoadingAddress from Plovdiv -> Sofia and Save
        WaitUntil(_ => Driver.Url.Contains("/Shipments/Edit"));

        var loading = Driver.FindElement(By.Id("LoadingAddress"));
        loading.Clear();
        loading.SendKeys("Sofia");

        Driver.FindElement(By.CssSelector("input[type='submit'][value='Save'],button[type='submit']")).Click();

        // 6) Verify on Details that LoadingAddress is Sofia
        WaitUntil(_ => Driver.Url.Contains("/Shipments/Details"));
        Assert.That(Driver.PageSource, Does.Contain("Sofia").IgnoreCase,
            "Details page should show updated LoadingAddress 'Sofia'.");
    }
}



