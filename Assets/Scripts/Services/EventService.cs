using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventService
{
    public EventController<int> OnCurrencyAdd { get; private set; }
    public EventController<int> OnCurrencySubtract { get; private set; }
    public EventController<int> OnCurrencyUpdated { get; private set; }

    public EventController<ItemSlotController> OnItemSlotClicked { get; private set; }

    public void Init()
    {
        OnCurrencyAdd = new EventController<int>();
        OnCurrencySubtract = new EventController<int>();
        OnCurrencyUpdated = new EventController<int>();
        OnItemSlotClicked = new EventController<ItemSlotController>();
    }
}
