using OpenQA.Selenium;

namespace CarRentingSystem.SeleniumTests.Pages.Shipments;

public class ShipmentDetailsPage
{
    private readonly IWebDriver _driver;
    public ShipmentDetailsPage(IWebDriver driver) => _driver = driver;

    public string TitleText =>
        _driver.FindElement(By.XPath("//p/u | //h2 | //p[contains(@style,'font-size')]/u")).Text;

    public bool HasEdit => _driver.FindElements(By.XPath("//a[normalize-space()='Edit']")).Any();
    public bool HasDelete => _driver.FindElements(By.XPath("//a[normalize-space()='Delete']")).Any();
    public bool HasRent => _driver.FindElements(By.XPath("//input[@type='submit' and @value='Rent']")).Any();
    public bool HasLeave => _driver.FindElements(By.XPath("//input[@type='submit' and @value='Leave']")).Any();

    public void ClickEdit() => _driver.FindElement(By.XPath("//a[normalize-space()='Edit']")).Click();
    public void ClickDelete() => _driver.FindElement(By.XPath("//a[normalize-space()='Delete']")).Click();

    public void Rent() => _driver.FindElement(By.XPath("//form//input[@type='submit' and @value='Rent']")).Click();
    public void Leave() => _driver.FindElement(By.XPath("//form//input[@type='submit' and @value='Leave']")).Click();
}
