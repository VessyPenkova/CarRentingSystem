using System;
using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CarRentingSystem.SeleniumTests.Pages.Shipments
{
    public class AllShipmentsPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly string _baseUrl;

        // Filter controls
        private static readonly By CategorySelect = By.Id("Category");     // <select asp-for="Category">
        private static readonly By SearchTermInput = By.Id("SearchTerm");   // <input asp-for="SearchTerm">
        private static readonly By SortingSelect = By.Id("Sorting");      // <select asp-for="Sorting">
        private static readonly By SearchBtn =
            By.XPath("//button[normalize-space()='Search' or @type='submit'] | //input[@type='submit' and @value='Search']");

        // Top bar link
        private static readonly By AddShipmentLink = By.LinkText("Add Shipment");

        // A very tolerant selector set for “cards”
        private static readonly By CardCandidates = By.CssSelector(".card, .col, .col-md-4, .col-lg-4, .row");

        public AllShipmentsPage(IWebDriver driver, string baseUrl)
        {
            _driver = driver;
            _baseUrl = baseUrl.TrimEnd('/');
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void Navigate()
        {
            _driver.Navigate().GoToUrl($"{_baseUrl}/Shipments/All");
            _wait.Until(d => d.FindElement(SearchTermInput).Displayed);
        }

        // ---------- Filters ----------
        public void SetSearchTerm(string term)
        {
            var input = _driver.FindElement(SearchTermInput);
            input.Clear();
            input.SendKeys(term);
        }

        public void SetCategoryByText(string visibleText)
        {
            if (!Exists(CategorySelect)) return;
            new SelectElement(_driver.FindElement(CategorySelect)).SelectByText(visibleText);
        }

        public void SetSortingByText(string visibleText)
        {
            if (!Exists(SortingSelect)) return;
            new SelectElement(_driver.FindElement(SortingSelect)).SelectByText(visibleText);
        }

        public void ClickSearch()
        {
            var btn = TryFind(SearchBtn)
                      ?? TryFind(By.CssSelector("button[type='submit']"))
                      ?? TryFind(By.CssSelector("input[type='submit']"))
                      ?? throw new NoSuchElementException("Search button not found.");
            btn.Click();
            _wait.Until(d => d.Url.Contains("/Shipments/All", StringComparison.OrdinalIgnoreCase));
        }

        public void SearchByTitle(string title, string? category = null, string? sorting = null)
        {
            SetSearchTerm(title);
            if (!string.IsNullOrWhiteSpace(category)) SetCategoryByText(category);
            if (!string.IsNullOrWhiteSpace(sorting)) SetSortingByText(sorting);
            ClickSearch();
        }

        // ---------- Cards ----------
        public bool HasCardWithTitle(string title) =>
            _driver.PageSource.Contains(title, StringComparison.OrdinalIgnoreCase);

        public void OpenAddShipment() => _driver.FindElement(AddShipmentLink).Click();

        public void OpenDetailsForTitle(string title) => ClickOnCardAction(title, "Details");
        public void ClickEditForTitle(string title) => ClickOnCardAction(title, "Edit");
        public void ClickDeleteForTitle(string title) => ClickOnCardAction(title, "Delete");
        public void ClickRentForTitle(string title) => ClickOnCardAction(title, "Rent");
        public void ClickLeaveForTitle(string title) => ClickOnCardAction(title, "Leave");

        public bool HasEditForTitle(string title) => HasActionOnCard(title, "Edit");
        public bool HasDeleteForTitle(string title) => HasActionOnCard(title, "Delete");
        public bool HasRentForTitle(string title) => HasActionOnCard(title, "Rent");
        public bool HasLeaveForTitle(string title) => HasActionOnCard(title, "Leave");

        // ---------- helpers ----------
        private IWebElement GetCardByTitle(string title)
        {
            // Prefer real cards; fall back to any container that includes the text
            IReadOnlyCollection<IWebElement> candidates = _driver.FindElements(CardCandidates);
            var card = candidates.FirstOrDefault(c =>
                            c.Text.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (card != null) return card;

            // Fallback: find an element that contains the title and climb a bit
            var titleNode = TryFind(By.XPath($"//*[contains(normalize-space(), {XpathLiteral(title)})]"))
                         ?? throw new NoSuchElementException($"Card containing '{title}' not found.");
            return Ancestor(titleNode, 3) ?? titleNode;
        }

        private void ClickOnCardAction(string title, string actionText)
        {
            var card = GetCardByTitle(title);

            // Try link buttons first (Details/Edit/Delete typically <a>)
            var link = card.FindElements(By.XPath($".//a[normalize-space()='{actionText}']")).FirstOrDefault();
            if (link != null) { link.Click(); return; }

            // Then form submit inputs (Rent/Leave typically <input type='submit' value='Rent'>)
            var input = card.FindElements(By.XPath($".//input[@type='submit' and @value='{actionText}']")).FirstOrDefault();
            if (input != null) { input.Click(); return; }

            // Finally generic buttons
            var btn = card.FindElements(By.XPath($".//button[normalize-space()='{actionText}']")).FirstOrDefault();
            if (btn != null) { btn.Click(); return; }

            throw new NoSuchElementException($"{actionText} control not found on card '{title}'.");
        }

        private bool HasActionOnCard(string title, string actionText)
        {
            var card = GetCardByTitle(title);
            return card.FindElements(By.XPath($".//a[normalize-space()='{actionText}']")).Any()
                   || card.FindElements(By.XPath($".//input[@type='submit' and @value='{actionText}']")).Any()
                   || card.FindElements(By.XPath($".//button[normalize-space()='{actionText}']")).Any();
        }

        private static string XpathLiteral(string value)
        {
            if (!value.Contains("'")) return $"'{value}'";
            if (!value.Contains("\"")) return $"\"{value}\"";
            // concat for rare strings having both quotes
            var parts = value.Split('\'').Select(p => $"'{p}'");
            return $"concat({string.Join(",\"'\",", parts)})";
        }

        private IWebElement? TryFind(By by)
        {
            try { return _driver.FindElement(by); }
            catch (NoSuchElementException) { return null; }
        }

        private bool Exists(By by)
        {
            try { _ = _driver.FindElement(by); return true; }
            catch (NoSuchElementException) { return false; }
        }

        private static IWebElement? Ancestor(IWebElement el, int up)
        {
            var curr = el;
            for (var i = 0; i < up; i++)
            {
                try { curr = curr.FindElement(By.XPath("..")); }
                catch { return curr; }
            }
            return curr;
        }
    }
}
