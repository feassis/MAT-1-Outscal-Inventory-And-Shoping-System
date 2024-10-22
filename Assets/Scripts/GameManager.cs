using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CurrencyService currencyService;
    private EventService eventService;
    private ItemManagementSystemService itemManagementSystemService;

    [SerializeField] private UIService uiService;

    [SerializeField] private WalletConfigSO walletConfigSO;
    [SerializeField] private QAToolView qaToolView;
    [SerializeField] private ShopSO shopSO;
    [SerializeField] private ShopView shopViewPrefab;
    [SerializeField] private ItemSlotView itemSlotViewPrefab;

    [SerializeField] private InventorySO inventorySO;
    [SerializeField] private InventoryView inventoryViewPrefab;
    [SerializeField] private ResourceGatherConfig resourceSO;

    private WalletController walletController;
    private QAToolController qaToolController;
    private ShopController shopController;
    private InventoryController inventoryController;

    private void Awake()
    {
        currencyService = new CurrencyService();
        eventService = new EventService();
        itemManagementSystemService = new ItemManagementSystemService();

        InitializeServices();
        InstantiateUI();
    }

    private void InitializeServices()
    {
        eventService.Init();
        currencyService.Init(eventService);
        itemManagementSystemService.Init(currencyService, uiService, resourceSO, eventService);
        uiService.Init(eventService);
    }

    private void InstantiateUI()
    {
        WalletModel walletModel = new WalletModel(currencyService.GetCurrency());
        walletController = new WalletController(walletConfigSO.walletViewPrefab, walletModel, uiService, eventService);

        qaToolController = new QAToolController(qaToolView, uiService, currencyService);

        ShopModel shopModel = new ShopModel(shopSO, itemSlotViewPrefab);
        shopController = new ShopController(shopViewPrefab, shopModel, uiService, eventService, itemManagementSystemService);

        InventoryModel inventoryModel = new InventoryModel(inventorySO);
        inventoryController = new InventoryController(eventService, uiService, inventoryModel, inventoryViewPrefab, itemManagementSystemService);
        
        itemManagementSystemService.SetControllers(shopController, inventoryController);
    
    }
}
