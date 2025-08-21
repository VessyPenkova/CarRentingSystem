using OpenQA.Selenium;

namespace CarRentingSystem.SeleniumTests.Pages.Layout;

public class Navbar
{
    private readonly IWebDriver _driver;
    public Navbar(IWebDriver driver) => _driver = driver;

    public void ClickAllShipments() =>
        _driver.FindElement(By.LinkText("All Shipments")).Click();

    public void ClickAddShipment() =>
        _driver.FindElement(By.LinkText("Add Shipment")).Click();

    public bool IsLoggedIn =>
        _driver.FindElements(By.LinkText("Logout")).Any();

    public void Logout()
    {
        if (IsLoggedIn)
            _driver.FindElement(By.LinkText("Logout")).Click();
    }
}
