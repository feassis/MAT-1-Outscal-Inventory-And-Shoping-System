using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIService : MonoBehaviour
{
    [field:  SerializeField] public Transform UIRoot {  get; private set; }
    [SerializeField] private GenericPopup genericPopup;


    public void OpenGenericPopup(string description, Action onConfirmation = null)
    {
        GenericPopup popup = Instantiate<GenericPopup>(genericPopup, UIRoot);
        popup.Setup(description, onConfirmation);
    }
}
