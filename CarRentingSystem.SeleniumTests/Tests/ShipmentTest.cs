//using NUnit.Framework;
//using CarRentingSystem.SeleniumTests.Fixtures;
//using CarRentingSystem.SeleniumTests.Pages.Identity;
//using CarRentingSystem.SeleniumTests.Pages.Layout;
//using CarRentingSystem.SeleniumTests.Pages.Shipments;

//namespace CarRentingSystem.SeleniumTests.Tests
//{
//    /// <summary>
//    /// One class for the CRUD flow (Add → Edit → Delete) with ordered tests.
//    /// This keeps your classes separate (LoginTests, RegisterTests, ShipmentsTests),
//    /// but guarantees order within this class.
//    /// </summary>
//    [TestFixture, Order(2)]
//    public sealed class ShipmentTest : TestBase
//    {
//        private const string Email = "TransportCompanyPIMK_Test01@pimk.com";
//        private const string Password = "password123456";
//        private static string _title = string.Empty;
//        private static string _editedTitle = string.Empty;

//        [OneTimeSetUp]
//        public void LoginOnce()
//        {
//            // Always ensure we are logged in for the CRUD flow
//            Driver.Navigate().GoToUrl(BaseUrl + "/");
//            var nav = new Navbar(Driver);
//            if (!nav.IsLoggedIn)
//            {
//                var login = new LoginPage(Driver, BaseUrl);
//                login.Open("/"); // go back home after login
//                login.Fill(Email, Password);
//                login.Submit();
//                WaitUntil(_ => new Navbar(Driver).IsLoggedIn);
//            }
//        }

//        [Test, Order(1), Category("ui"), Category("shipments")]
//        public void Add_NewShipment_WithValidData_Succeeds()
//        {
//            LoginOnce();
//            var nav = new Navbar(Driver);
//            nav.ClickAddShipment();
//            WaitUntil(_ => Driver.Url.Contains("/Shipments/Add"));

//            _title = $"Plovdiv-Valence-{DateTime.UtcNow:yyyyMMddHHmmssfff}";

//            var add = new AddShipmentPage(Driver);
//            add.Fill(
//                title: _title,
//                from: "Plovdiv",
//                to: "Valence",
//                desc: "Plovdiv-Valence-Charter",
//                imageUrl: "https://picsum.photos/900/450",
//                price: "7500",
//                categoryId: "1"         // adjust if your first category id differs
//            );
//            add.Save();

//            // App may redirect to Home or Details; accept either, then verify via All→Search
//            WaitUntil(_ => Driver.Url.Contains("/Shipments/Details") ||
//                           Driver.Url.TrimEnd('/').Equals(BaseUrl.TrimEnd('/'), StringComparison.OrdinalIgnoreCase));

//            nav.ClickAllShipments();
//            WaitUntil(_ => Driver.Url.Contains("/Shipments/All"));

//            var all = new AllShipmentsPage(Driver, BaseUrl);
//            all.SearchByTitle(_title);
//            Assert.That(all.HasCardWithTitle(_title), Is.True, $"Expected to see '{_title}' after adding.");
//        }

//        [Test, Order(2), Category("ui"), Category("shipments")]
//        public void Edit_ExistingShipment_ChangesPersist()
//        {
//            Assert.That(_title, Is.Not.Empty, "Add test did not set title.");

//            var nav = new Navbar(Driver);
//            nav.ClickAllShipments();
//            WaitUntil(_ => Driver.Url.Contains("/Shipments/All"));

//            var all = new AllShipmentsPage(Driver, BaseUrl);
//            all.SearchByTitle(_title);
//            all.OpenDetailsForTitle(_title);
//            WaitUntil(_ => Driver.Url.Contains("/Shipments/Details"));

//            var details = new ShipmentDetailsPage(Driver);
//            details.ClickEdit();
//            WaitUntil(_ => Driver.Url.Contains("/Shipments/Edit"));

//            _editedTitle = _title.Replace("Plovdiv", "Sofia", StringComparison.OrdinalIgnoreCase);
//            if (_editedTitle == _title) _editedTitle += "-Edited";

//            var edit = new EditShipmentPage(Driver);
//            edit.ChangeAllFields(
//                title: _editedTitle,
//                loadingAddress: "Sofia",
//                deliveryAddress: "Valence",
//                description: "Sofia-Valence-Charter (Edited)",
//                imageUrl: "https://picsum.photos/1000/500",
//                price: "7600",
//                categoryValue: "1"
//            );
//            edit.Save();

//            WaitUntil(_ => Driver.Url.Contains("/Shipments/Details"));
//            details = new ShipmentDetailsPage(Driver);
//            Assert.That(details.TitleText, Does.Contain("Sofia").IgnoreCase);

//            nav.ClickAllShipments();
//            WaitUntil(_ => Driver.Url.Contains("/Shipments/All"));
//            all = new AllShipmentsPage(Driver, BaseUrl);
//            all.SearchByTitle(_editedTitle);
//            Assert.That(all.HasCardWithTitle(_editedTitle), Is.True);
//        }

//        [Test, Order(3), Category("ui"), Category("shipments")]
//        public void Delete_Shipment_RemovesFromList()
//        {
//            Assert.That(_editedTitle, Is.Not.Empty, "Edit test did not set edited title.");

//            var nav = new Navbar(Driver);
//            nav.ClickAllShipments();
//            WaitUntil(_ => Driver.Url.Contains("/Shipments/All"));

//            var all = new AllShipmentsPage(Driver, BaseUrl);
//            all.SearchByTitle(_editedTitle);
//            all.OpenDetailsForTitle(_editedTitle);
//            WaitUntil(_ => Driver.Url.Contains("/Shipments/Details"));

//            var details = new ShipmentDetailsPage(Driver);
//            details.ClickDelete();
//            WaitUntil(_ => Driver.Url.Contains("/Shipments/Delete"));

//            var del = new DeleteShipmentPage(Driver);
//            del.Confirm();

//            WaitUntil(_ => Driver.Url.Contains("/Shipments/All"));
//            all = new AllShipmentsPage(Driver, BaseUrl);
//            all.SearchByTitle(_editedTitle);
//            Assert.That(all.HasCardWithTitle(_editedTitle), Is.False, "Deleted shipment should not appear anymore.");
//        }
//    }
//}
