using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ShoppingList.Client.Tests.Pages;

public class ShoppingListPage(IWebDriver webDriver, WebDriverWait wait)
{
    private readonly IWebDriver _webDriver = webDriver;
    private readonly WebDriverWait _wait = wait;

    IWebElement NewItemModal => _webDriver.FindElement(By.ClassName("modal-box"));
    IWebElement ItemsTableModal => _webDriver.FindElement(By.ClassName("table"));
    IWebElement Header => _webDriver.FindElement(
        By.XPath("//*[contains(@class, 'font-twinkle-star') and text()='Shopping List']"));
    IWebElement NewItemButton => _webDriver.FindElement(By.XPath("//button[text()='New Item']"));
    IWebElement FieldInput(string inputName) => NewItemModal.FindElement(By.Name(inputName));
    IWebElement TableData(string text) => ItemsTableModal.FindElement(By.XPath($"//td[text()='{text}']"));
    IWebElement TableRowForItem(string itemName) => ItemsTableModal.FindElement(
        By.XPath($"//td[text()='{itemName}']/parent::tr[contains(@class,'line-through')]"));
    IWebElement StatusButtonForItem(string itemName) => ItemsTableModal.FindElement(
        By.XPath($"//td[text()='{itemName}']/parent::tr//button[contains(text(),'Purchased')]"));
    IWebElement DeleteButtonForItem(string itemName) => ItemsTableModal.FindElement(
        By.XPath($"//td[text()='{itemName}']/parent::tr//button[contains(text(),'Delete')]"));
    IWebElement SaveButton => _webDriver.FindElement(By.XPath("//button[@type='submit' and text()='Save']"));
    IWebElement SuccessToast => _webDriver.FindElement(By.XPath("//*[contains(@class,'alert-success')]"));

    public bool IsHeaderDisplayed() => _wait.Until(_ => Header.Displayed);
    public bool IsTableDataDisplayed(string text) => _wait.Until(_ => TableData(text).Displayed);
    public bool IsLineCrossedThroughForItem(string itemName) => _wait.Until(_ => TableRowForItem(itemName).Displayed);
    public string GetStatusButtonTextForItem(string itemName)
    {
        _wait.Until(_ => StatusButtonForItem(itemName).Displayed);
        return StatusButtonForItem(itemName).Text;
    }
    public void ToggleStatusButtonTextForItem(string itemName)
    {
        _wait.Until(_ => StatusButtonForItem(itemName).Displayed);
        StatusButtonForItem(itemName).Click();
    }
    public void ClickDeleteButtonTextForItem(string itemName)
    {
        _wait.Until(_ => DeleteButtonForItem(itemName).Displayed);
        DeleteButtonForItem(itemName).Click();
    }
    public void ClickNewItem()
    {
        _wait.Until(_ => NewItemButton.Displayed);
        NewItemButton.Click();
    }
    public void ClickSaveButton() => SaveButton.Click();
    public ShoppingListPage EnterItemName(string name)
    {
        FieldInput("name").SendKeys(name);
        return this;
    }
    public ShoppingListPage EnterItemDescription(string description)
    {
        FieldInput("description").SendKeys(description);
        return this;
    }
    public ShoppingListPage EnterItemPrice(string price)
    {
        FieldInput("price").SendKeys(price);
        return this;
    }
    public string GetSuccessToastText()
    {
        _wait.Until(_ => SuccessToast.Displayed);
        return SuccessToast.Text;
    }
}