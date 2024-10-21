using System.Collections.Generic;

public class InventoryModel : CategoryInventoryModel
{
    public float CurrentWeight { get; private set; }
    public float MaxWeight { get; private set; }

    private InventoryController controller;

    public InventoryModel(InventorySO inventorySO)
    {
        foreach (InventoryItemConfig item in inventorySO.Items)
        {
            Items.Add(new InventoryItem(new Item(item.ItemSO), item.Amount));
        }

        this.slotViewPrefab = inventorySO.SlotViewPrefab;
        this.MaxWeight = inventorySO.MaxWeight;

        CalculateCurrentWeight();
    }

    public void CalculateCurrentWeight()
    {
        float weight = 0;

        foreach(var slot in Items)
        {
            weight += slot.Amount * slot.Item.Weight;
        }

        CurrentWeight = weight;
    }

    public void SetController(InventoryController controller)
    {
        this.controller = controller;
    }
}
