using OpenQA.Selenium;

namespace CarRentingSystem.SeleniumTests.Pages.Shipments;

public class AllShipmentsPage
{
    private readonly IWebDriver _driver;
    private readonly string _baseUrl;
    public AllShipmentsPage(IWebDriver driver, string baseUrl)
    {
        _driver = driver; _baseUrl = baseUrl;
    }

    public void Navigate() => _driver.Navigate().GoToUrl($"{_baseUrl}/Shipments/All");

    public bool HasCardWithTitle(string title) =>
        _driver.PageSource.Contains(title, StringComparison.OrdinalIgnoreCase);

    public void OpenDetailsFromCard(string title)
    {
        // “Details” button next to the card that contains the title
        var card = _driver.FindElements(By.CssSelector(".row .card, .row"))
                          .FirstOrDefault(e => e.Text.Contains(title, StringComparison.OrdinalIgnoreCase))
                   ?? _driver.FindElement(By.TagName("body")); // fallback
        card.FindElement(By.XPath(".//a[normalize-space()='Details']")).Click();
    }
}
