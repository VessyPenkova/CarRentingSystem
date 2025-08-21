using OpenQA.Selenium;

namespace CarRentingSystem.SeleniumTests.Pages.Shipments;

public class DeleteShipmentPage
{
    private readonly IWebDriver _driver;
    public DeleteShipmentPage(IWebDriver driver) => _driver = driver;
    public string Title => _driver.FindElement(By.XPath("//h1|//h2|//div[contains(@class,'title')]")).Text;

    public void Confirm() => _driver.FindElement(By.XPath("//button[normalize-space()='Delete']|//input[@type='submit' and @value='Delete']")).Click();
    public void Cancel() => _driver.FindElement(By.LinkText("Cancel")).Click();
}
