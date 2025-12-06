using ShoppingList.Client.Tests.Data;

namespace ShoppingList.Client.Tests;

public class ShoppingListPageTests: TestsBase
{
    [Test, Description("Test new item creation, toggle and deletion")]
    public void TestItemActivitiesInShoppingList()
    {
        Assert.That(ShoppingListPage.IsHeaderDisplayed, Is.True);

        ShoppingListPage.ClickNewItem();
        ShoppingListPage.EnterItemName(ItemConstants.Name)
            .EnterItemDescription(ItemConstants.Description)
            .EnterItemPrice(ItemConstants.Price)
            .ClickSaveButton();

        Assert.That(ShoppingListPage.GetSuccessToastText(), Is.EqualTo("The item was added successfully."));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(ShoppingListPage.IsTableDataDisplayed(ItemConstants.Name), Is.True);
            Assert.That(ShoppingListPage.IsTableDataDisplayed(ItemConstants.Description), Is.True);
            Assert.That(ShoppingListPage.IsTableDataDisplayed(ItemConstants.Price), Is.True);
            Assert.That(ShoppingListPage.GetStatusButtonTextForItem(ItemConstants.Name), Is.EqualTo(ItemConstants.NotPurchasedStatus));
        }

        ShoppingListPage.ToggleStatusButtonTextForItem(ItemConstants.Name);
        Assert.That(ShoppingListPage.GetSuccessToastText(), Is.EqualTo("The status of the item changed."));
        Assert.That(ShoppingListPage.GetStatusButtonTextForItem(ItemConstants.Name), Is.EqualTo(ItemConstants.PurchasedStatus));
        Assert.That(ShoppingListPage.IsLineCrossedThroughForItem(ItemConstants.Name), Is.True);

        ShoppingListPage.ClickDeleteButtonTextForItem(ItemConstants.Name);
        Assert.That(ShoppingListPage.GetSuccessToastText(), Is.EqualTo("The item was deleted.").Or.EqualTo("There are no items in the shopping list."));
    }
}
