//using CarRentingSystem.SeleniumTests.Fixtures;
//using CarRentingSystem.SeleniumTests.Pages.Identity;
//using CarRentingSystem.SeleniumTests.Pages.Layout;
//using CarRentingSystem.SeleniumTests.TestData;
//using CarRentingSystem.SeleniumTests.Utils;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Support.UI;

//namespace CarRentingSystem.SeleniumTests.Tests;

//public sealed class ShipmentsTests : TestBase
//{
//    /// <summary>Registers a user and waits until the navbar shows "Logout".</summary>
//    private void RegisterAndLandOnHome()
//    {
//        var reg = new RegisterPage(Driver, BaseUrl);
//        reg.Open(); // /Identity/Account/Register
//        var data = RegisterTestData.Valid();

//        reg.Fill(data.Email, data.FirstName, data.LastName, data.Password, data.ConfirmPassword);
//        reg.SubmitNow();
        

//        var nav = new Navbar(Driver);

//        Assert.That(nav.WaitForLoggedIn(20), Is.True, "Expected to be logged-in after register, but navbar did not show a Manage/Logout control in time.");

//        WaitUntil(_ => nav.IsLoggedIn);     // should land on "/"
//        Assert.That(nav.IsLoggedIn, Is.True);    
//    }
//    [Test]
//    public void AddNewShipmentWithValidData_ShowsUpInAllSearch()
//    {
//        // 0) sign in with the shared test user
//        Driver.Navigate().GoToUrl(BaseUrl + "/");
//        var nav = new Navbar(Driver);
//        if (nav.IsLoggedIn)
//        {
//            nav.Logout();
//            WaitUntil(_ => !new Navbar(Driver).IsLoggedIn);
//        }

//        var login = new LoginPage(Driver, BaseUrl);
//        login.Open("/");                           // return to Home after successful login
//        login.Fill(TestUsers.Email, TestUsers.Password);
//        login.Submit();
//        nav = new Navbar(Driver);
//        WaitUntil(_ => nav.IsLoggedIn);

//        // 1) open Add Shipment
//        new Navbar(Driver).ClickAddShipment();
//        WaitUntil(_ => Driver.Url.Contains("/Shipments/Add", StringComparison.OrdinalIgnoreCase));

//        // unique-ish title so search is deterministic
//        var title = "Plovdiv-Valence-Test01";
//        var image = "https://www.hispaviacion.es/wp-content/uploads/2022/05/Falcon2000.jpeg";

//        // 2) fill the form
//        Driver.FindElement(By.Id("Title")).SendKeys(title);
//        Driver.FindElement(By.Id("LoadingAddress")).SendKeys("Plovdiv");
//        Driver.FindElement(By.Id("DeliveryAddress")).SendKeys("Valence");
//        Driver.FindElement(By.Id("Description")).SendKeys("Charter " + title);
//        Driver.FindElement(By.Id("ImageUrlShipmentGoogleMaps")).SendKeys(image);

//        var price = Driver.FindElement(By.Id("Price"));
//        price.Clear();
//        price.SendKeys("7500");

//        var cat = new SelectElement(Driver.FindElement(By.Id("CategoryId")));
//        try { cat.SelectByText("Charter"); }       // preferred
//        catch { cat.SelectByValue("5"); }          // fallback if texts differ

//        // 3) submit (Save)
//        Driver.FindElement(By.CssSelector("input[type='submit'][value='Save'],button[type='submit']")).Click();
        
//        // 4) redirected to Home
//        var expectedHome = BaseUrl.TrimEnd('/');
//        WaitUntil(_ =>
//            Driver.Url.TrimEnd('/').Equals(expectedHome, StringComparison.OrdinalIgnoreCase) ||
//            Driver.Url.Contains("/Home", StringComparison.OrdinalIgnoreCase));

//        // 5) open All Shipments and search by the exact title
//        new Navbar(Driver).ClickAllShipments();
//        WaitUntil(_ => Driver.Url.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase));

//        var search = Driver.FindElement(By.Id("SearchTerm"));
//        search.Clear();
//        search.SendKeys(title);
//        Driver.FindElement(By.CssSelector("form input[type='submit'], form button[type='submit']")).Click();

//        // 6) assert the results contain our new shipment
//        WaitUntil(_ => Driver.PageSource.Contains(title, StringComparison.OrdinalIgnoreCase));
//        Assert.That(Driver.PageSource, Does.Contain(title).IgnoreCase);
//    }

//}



