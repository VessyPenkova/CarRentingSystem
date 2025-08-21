using OpenQA.Selenium;

namespace CarRentingSystem.SeleniumTests.Pages.Shipments;

public class EditShipmentPage
{
    private readonly IWebDriver _driver;
    public EditShipmentPage(IWebDriver driver) => _driver = driver;

    public void ChangeTitle(string newTitle)
    {
        var t = _driver.FindElement(By.Id("Title"));
        t.Clear(); t.SendKeys(newTitle);
    }

    public void Save() =>
        _driver.FindElement(By.CssSelector("input[type='submit'], .btn-primary")).Click();
}
