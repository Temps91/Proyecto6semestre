using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsUI : UIWindow
{

    [SerializeField] private Button _buttonYes;
    [SerializeField] private Button _buttonNo;

    public override void Initialize()
    {
        base.Initialize();
        _buttonNo.onClick.AddListener(NoClick);
        _buttonYes.onClick.AddListener(YesClick);
    }
    private void YesClick()
    {
        Debug.Log("Yes Clicked");
    }
    private void NoClick()
    {
        Debug.Log("No clicked");
    }

}