using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.Progress;

public class ShopController
{
    private ShopView view;
    private ShopModel model;
    private UIService uiService;
    private EventService eventService = new EventService();

    private ItemSlotController selectedSlot;
    private int chosenAmount = 0;

    private List<ItemSlotController> materialSlotItems = new List<ItemSlotController>();
    private List<ItemSlotController> equipmentSlotItems = new List<ItemSlotController>();
    private List<ItemSlotController> consumableSlotItems = new List<ItemSlotController>();
    private List<ItemSlotController> treasureSlotItems = new List<ItemSlotController>();

    public ShopController(ShopView shopViewPrefab, ShopModel shopModel, UIService uiService, EventService eventService)
    {
        this.model = shopModel;
        this.uiService = uiService;
        this.eventService = eventService;

        this.eventService.OnItemSlotClicked.AddListener(OnSlotSelected);

        view = GameObject.Instantiate<ShopView>(shopViewPrefab, uiService.UIRoot);
        view.SetController(this);
        SetupInventory();
    }

    ~ShopController() 
    {
        this.eventService.OnItemSlotClicked.RemoveListener(OnSlotSelected);
    }

    private void SetupInventory()
    {
        foreach(var item in model.Items)
        {
            ItemSlotModel slotModel = new ItemSlotModel(item);
            ItemSlotController itemSlotController = new ItemSlotController(model.slotViewPrefab, slotModel, view.ItemGridRoot, eventService);

            switch (item.Item.Type)
            {
                case ItemType.None:
                    break;
                case ItemType.Materials:
                    materialSlotItems.Add(itemSlotController);
                    break;
                case ItemType.Equipment:
                    equipmentSlotItems.Add(itemSlotController);
                    break;
                case ItemType.Consumables:
                    consumableSlotItems.Add(itemSlotController);
                    break;
                case ItemType.Treasure:
                    treasureSlotItems.Add(itemSlotController);
                    break;
            }
        }
    }

    private void OnSlotSelected(ItemSlotController controller)
    {
        selectedSlot = controller;
        chosenAmount = 1;
        UpdateDetailsPanel();
    }

    private void UpdateDetailsPanel()
    {
        if(selectedSlot == null)
        {
            view.HideDescriptionPanel();
            return;
        }

        var item = selectedSlot.GetItem();

        var inventoryItem = model.GetInventoryItem(item);

        view.SetupDescriptionPanel(item.Icon, item.Name, item.Description, item.Weight*chosenAmount, chosenAmount, item.BuyingPrice * chosenAmount, inventoryItem.Amount);
    }

    public void DecreaseSelectedItemAmount()
    {
        var inventoryItem = model.GetInventoryItem(selectedSlot.GetItem());
        chosenAmount = Math.Max(chosenAmount - 1, 0);

        view.SetupAmountButtons(chosenAmount, inventoryItem.Amount);
        UpdateDetailsPanel();
    }

    public void IncreaseSelectedItemAmount()
    {
        var inventoryItem = model.GetInventoryItem(selectedSlot.GetItem());
        chosenAmount = Math.Min(chosenAmount + 1, inventoryItem.Amount);

        view.SetupAmountButtons(chosenAmount, inventoryItem.Amount);
        UpdateDetailsPanel();
    }

    public void AddItem(Item item, int amount)
    {
        model.AddItem(item, amount);
        ItemSlotController itemSlotControler = null;

        if (model.HasItem(item))
        {
            switch (item.Type)
            {
                case ItemType.None:
                    break;
                case ItemType.Materials:
                    itemSlotControler = materialSlotItems.Find(i => i.GetItem().Name.Equals(item.Name));
                    break;
                case ItemType.Equipment:
                    itemSlotControler = equipmentSlotItems.Find(i => i.GetItem().Name.Equals(item.Name));
                    break;
                case ItemType.Consumables:
                    itemSlotControler = consumableSlotItems.Find(i => i.GetItem().Name.Equals(item.Name));
                    break;
                case ItemType.Treasure:
                    itemSlotControler = treasureSlotItems.Find(i => i.GetItem().Name.Equals(item.Name));
                    break;
            }

            itemSlotControler.AddItem(amount);
        }
        else
        {
            ItemSlotModel itemSlotModel = new ItemSlotModel(new InventoryItem(item, amount));
            itemSlotControler = new ItemSlotController(model.slotViewPrefab, itemSlotModel, view.ItemGridRoot, eventService);
            switch (item.Type)
            {
                case ItemType.None:
                    break;
                case ItemType.Materials:
                    materialSlotItems.Add(itemSlotControler);
                    break;
                case ItemType.Equipment:
                    equipmentSlotItems.Add(itemSlotControler);
                    break;
                case ItemType.Consumables:
                    consumableSlotItems.Add(itemSlotControler);
                    break;
                case ItemType.Treasure:
                    treasureSlotItems.Add(itemSlotControler);
                    break;
            }
        }
    }

    public void RemoveItem(Item item, int amount)
    {
        ItemSlotController itemSlotControler = null;

        if (model.HasItem(item))
        {
            switch (item.Type)
            {
                case ItemType.None:
                    break;
                case ItemType.Materials:
                    itemSlotControler = materialSlotItems.Find(i => i.GetItem().Name.Equals(item.Name));
                    itemSlotControler.RemoveItem(amount);
                    
                    if (itemSlotControler.GetItemAmout() <= 0)
                    {
                        materialSlotItems.Remove(itemSlotControler);
                    }
                    break;
                case ItemType.Equipment:
                    itemSlotControler = equipmentSlotItems.Find(i => i.GetItem().Name.Equals(item.Name));
                    itemSlotControler.RemoveItem(amount);

                    if (itemSlotControler.GetItemAmout() <= 0)
                    {
                        equipmentSlotItems.Remove(itemSlotControler);
                    }
                    break;
                case ItemType.Consumables:
                    itemSlotControler = consumableSlotItems.Find(i => i.GetItem().Name.Equals(item.Name));
                    itemSlotControler.RemoveItem(amount);

                    if (itemSlotControler.GetItemAmout() <= 0)
                    {
                        consumableSlotItems.Remove(itemSlotControler);
                    }
                    break;
                case ItemType.Treasure:
                    itemSlotControler = treasureSlotItems.Find(i => i.GetItem().Name.Equals(item.Name));
                    itemSlotControler.RemoveItem(amount);

                    if (itemSlotControler.GetItemAmout() <= 0)
                    {
                        treasureSlotItems.Remove(itemSlotControler);
                    }
                    break;
            }
        }

        model.RemoveItem(item, amount);
    }

    public void SelectAllCategory()
    {
        selectedSlot = null;
        UpdateDetailsPanel();
        ToggleConsumableCategory(true);
        ToggleEquipmentCategory(true);
        ToggleMaterialCategory(true);
        ToggleTreasureCategory(true);
    }

    public void SelectMaterialCategory()
    {
        selectedSlot = null;
        UpdateDetailsPanel();
        ToggleConsumableCategory(false);
        ToggleEquipmentCategory(false);
        ToggleMaterialCategory(true);
        ToggleTreasureCategory(false);
    }

    public void SelectEquipmentCategory()
    {
        selectedSlot = null;
        UpdateDetailsPanel();
        ToggleConsumableCategory(false);
        ToggleEquipmentCategory(true);
        ToggleMaterialCategory(false);
        ToggleTreasureCategory(false);
    }

    public void SelectConsumablesCategory()
    {
        selectedSlot = null;
        UpdateDetailsPanel();
        ToggleConsumableCategory(true);
        ToggleEquipmentCategory(false);
        ToggleMaterialCategory(false);
        ToggleTreasureCategory(false);
    }

    public void SelectTreasureCategory()
    {
        selectedSlot = null;
        UpdateDetailsPanel();
        ToggleConsumableCategory(false);
        ToggleEquipmentCategory(false);
        ToggleMaterialCategory(false);
        ToggleTreasureCategory(true);
    }

    private void ToggleMaterialCategory(bool state)
    {
        foreach (var item in materialSlotItems)
        {
            item.ToggleView(state);
        }
    }

    private void ToggleEquipmentCategory(bool state)
    {
        foreach (var item in equipmentSlotItems)
        {
            item.ToggleView(state);
        }
    }

    private void ToggleConsumableCategory(bool state)
    {
        foreach (var item in consumableSlotItems)
        {
            item.ToggleView(state);
        }
    }

    private void ToggleTreasureCategory(bool state)
    {
        foreach (var item in treasureSlotItems)
        {
            item.ToggleView(state);
        }
    }
}
