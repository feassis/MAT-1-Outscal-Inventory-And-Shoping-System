using UnityEngine;

public class ItemSlotController
{
    private ItemSlotView view;
    private ItemSlotModel model;

    private EventService eventService;

    public ItemSlotController(ItemSlotView viewPrefab, ItemSlotModel itemSlotModel, Transform root, EventService eventService)
    {
        this.eventService = eventService;

        model = itemSlotModel;
        model.SetController(this);

        view = GameObject.Instantiate<ItemSlotView>(viewPrefab, root);
        view.SetController(this);
        view.SetRarityBorder(model.InventoryItem.Item.Rarity);
        view.SetIcon(model.InventoryItem.Item.Icon);
        view.SetAmount(model.InventoryItem.Amount);
        view.SetWeight(model.InventoryItem.Item.Weight);
    }

    public void SelectItem()
    {
        eventService.OnItemSlotClicked.InvokeEvent(this);
    }

    public Item GetItem()
    {
        return model.InventoryItem.Item;
    }

    public int GetItemAmout()
    {
        return model.InventoryItem.Amount;
    }

    public void ToggleView(bool state)
    {
        view.gameObject.SetActive(state);
    }

    public void AddItem(int amount)
    {
        model.InventoryItem.Amount += amount;
    }

    public void RemoveItem(int amount)
    {
        model.InventoryItem.Amount -= amount;

        if(model.InventoryItem.Amount <= 0)
        {
            GameObject.Destroy(view.gameObject);
        }
    }
}
