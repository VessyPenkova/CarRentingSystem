using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CarRentingSystem.SeleniumTests.Pages.Shipments;

public class AddShipmentPage
{
    private readonly IWebDriver _driver;
    public AddShipmentPage(IWebDriver driver) => _driver = driver;

    public void Fill(string title, string from, string to, string desc, string imageUrl, string price, string categoryId)
    {
        _driver.FindElement(By.Id("Title")).SendKeys(title);
        _driver.FindElement(By.Id("LoadingAddress")).SendKeys(from);
        _driver.FindElement(By.Id("DeliveryAddress")).SendKeys(to);
        _driver.FindElement(By.Id("Description")).SendKeys(desc);
        _driver.FindElement(By.Id("ImageUrlShipmentGoogleMaps")).SendKeys(imageUrl);
        _driver.FindElement(By.Id("Price")).Clear();
        _driver.FindElement(By.Id("Price")).SendKeys(price);

        new SelectElement(_driver.FindElement(By.Id("CategoryId"))).SelectByValue(categoryId);
    }

    public void Save() =>
        _driver.FindElement(By.CssSelector("input[type='submit'], .btn-primary")).Click();
}
