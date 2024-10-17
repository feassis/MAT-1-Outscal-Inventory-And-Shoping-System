using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CurrencyService currencyService;
    private EventService eventService;
    [SerializeField] private UIService uiService;

    [SerializeField] private WalletConfigSO walletConfigSO;
    [SerializeField] private QAToolView qaToolView;
    [SerializeField] private ShopSO shopSO;
    [SerializeField] private ShopView shopViewPrefab;
    [SerializeField] private ItemSlotView itemSlotViewPrefab;

    private WalletController walletController;
    private QAToolController qaToolController;
    private ShopController shopController;

    private void Awake()
    {
        currencyService = new CurrencyService();
        eventService = new EventService();

        InitializeServices();
        InstantiateUI();
    }

    private void InitializeServices()
    {
        eventService.Init();
        currencyService.Init(eventService);
    }

    private void InstantiateUI()
    {
        WalletModel walletModel = new WalletModel(currencyService.GetCurrency());
        walletController = new WalletController(walletConfigSO.walletViewPrefab, walletModel, uiService, eventService);

        qaToolController = new QAToolController(qaToolView, uiService, currencyService);

        ShopModel shopModel = new ShopModel(shopSO, itemSlotViewPrefab);
        shopController = new ShopController(shopViewPrefab, shopModel, uiService, eventService);
    }
}
