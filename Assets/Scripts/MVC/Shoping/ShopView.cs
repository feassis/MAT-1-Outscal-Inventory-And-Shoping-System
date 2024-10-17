using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    [field: SerializeField] public Transform ItemGridRoot { get; private set; }

    [Header("Category Buttons")]
    [SerializeField] private Button allCategoryButton;
    [SerializeField] private Button materialCategoryButton;
    [SerializeField] private Button equipmentCategoryButton;
    [SerializeField] private Button consumableCategoryButton;
    [SerializeField] private Button treasureCategoryButton;


    [Header("Item Description Panel")]
    [SerializeField] private GameObject itemDescriptionPanel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemWeightText;
    [SerializeField] private TextMeshProUGUI itemAmountText;
    [SerializeField] private TextMeshProUGUI itemCostText;
    [SerializeField] private Button decreaseAmoutButtom;
    [SerializeField] private Button increaseAmoutButtom;
    [SerializeField] private Button buyButtom;

    private ShopController controller;

    private void Awake()
    {
        allCategoryButton.onClick.AddListener(OnAllCategoryButtonSelected);
        materialCategoryButton.onClick.AddListener(OnMaterialCategoryButtonSelected);
        equipmentCategoryButton.onClick.AddListener(OnEquipmentCategoryButtonSelected);
        consumableCategoryButton.onClick.AddListener(OnConsumableCategoryButtonSelected);
        treasureCategoryButton.onClick.AddListener(OnTreasureCategoryButtonSelected);

        decreaseAmoutButtom.onClick.AddListener(OnDecreaseAmountButtomClicked);
        increaseAmoutButtom.onClick.AddListener(OnIncreaseAmountButtomClicked);
    }

    private void OnIncreaseAmountButtomClicked()
    {
        controller.IncreaseSelectedItemAmount();
    }

    private void OnDecreaseAmountButtomClicked()
    {
        controller.DecreaseSelectedItemAmount();
    }

    public void SetController(ShopController controller)
    {
        this.controller = controller;
    }

    public void HideDescriptionPanel()
    {
        itemDescriptionPanel.SetActive(false);
    }

    public void SetupDescriptionPanel(Sprite icon, string name, string description, float weight, int amount, int cost, int itemMaxAmount)
    {
        itemDescriptionPanel.SetActive(true);
        itemIcon.sprite = icon;
        itemNameText.text = name;
        itemDescriptionText.text = description;
        itemWeightText.text = $"{weight} KG";
        itemAmountText.text = amount.ToString();
        itemCostText.text = $"{cost}  GOLD";

        SetupAmountButtons(amount, itemMaxAmount);
    }

    public void SetupAmountButtons(int itemAmount, int itemMaxAmount)
    {
        itemAmountText.text = itemAmount.ToString();
        if(itemAmount <= 1)
        {
            decreaseAmoutButtom.gameObject.SetActive(false);
        }
        else
        {
            decreaseAmoutButtom.gameObject.SetActive(true);
        }

        if(itemAmount >= itemMaxAmount)
        {
            increaseAmoutButtom.gameObject.SetActive(false);
        }
        else
        {
            increaseAmoutButtom.gameObject.SetActive(true);
        }
    }

    private void OnTreasureCategoryButtonSelected()
    {
        controller.SelectTreasureCategory();
    }

    private void OnConsumableCategoryButtonSelected()
    {
        controller.SelectConsumablesCategory();
    }

    private void OnEquipmentCategoryButtonSelected()
    {
        controller.SelectEquipmentCategory();
    }

    private void OnMaterialCategoryButtonSelected()
    {
        controller.SelectMaterialCategory();
    }

    private void OnAllCategoryButtonSelected()
    {
        controller.SelectAllCategory();
    }
}
