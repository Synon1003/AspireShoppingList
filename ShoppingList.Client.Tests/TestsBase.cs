using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ShoppingList.Client.Tests.Pages;

namespace ShoppingList.Client.Tests;

public class TestsBase
{
    protected IWebDriver _driver;
    protected ShoppingListPage ShoppingListPage => field ??
        new ShoppingListPage(_driver, new WebDriverWait(_driver, TimeSpan.FromSeconds(5)));
    

    [OneTimeSetUp] public void GlobalSetup()
    {
        _driver = new ChromeDriver();
        _driver.Navigate().GoToUrl(TestContext.Parameters["BaseUrl"]!);
    }

    [OneTimeTearDown] public void GlobalTeardown()
    {
        _driver.Close();
        _driver.Quit();
    }
}
