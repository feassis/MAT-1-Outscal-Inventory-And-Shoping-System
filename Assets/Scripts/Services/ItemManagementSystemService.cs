using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManagementSystemService
{
    private ShopController shopController;
    private InventoryController inventoryController;
    private CurrencyService currencyService;
    private UIService uiService;

    public void Init(CurrencyService currencyService, UIService uiService)
    {
        this.currencyService = currencyService;
        this.uiService = uiService;
    }

    public void SetControllers(ShopController shopController, InventoryController inventoryController)
    {
        this.shopController = shopController;
        this.inventoryController = inventoryController;

    }

    public void TryBuyItem(Item item, int amount, AudioSource failed, AudioSource success)
    {
        int currencyNeeded = item.BuyingPrice * amount;
        float weightOfTheItems = item.Weight * amount;

        if(currencyNeeded > currencyService.GetCurrency())
        {
            uiService.OpenGenericPopup("Sorry! You don't have enough Gold, try sell some stuff.");
            failed.Play();
            return;
        }

        if(weightOfTheItems > inventoryController.GetRemainingWeight())
        {
            uiService.OpenGenericPopup("Sorry! You are carrying too much, try sell some stuff.");
            failed.Play();
            return;
        }

        uiService.OpenGenericPopup($"You want to buy {amount} {item.Name} for {currencyNeeded}",
            () =>
            {
                success.Play();
                currencyService.SubtractCurrency(currencyNeeded);
                shopController.RemoveItem(item, amount);    
                AddItemToInventory(item, amount);
            });
    }

    public void AddItemToInventory(Item item, int amount)
    {
        inventoryController.AddItem(item, amount, false);
    }

    public void SellItem(Item item, int amount)
    {
        int sellingPrice = item.SellingPrice * amount;

        currencyService.AddCurrency(sellingPrice);
        inventoryController.RemoveItem(item,amount);
        shopController.AddItem(item,amount, true);
    }
}
