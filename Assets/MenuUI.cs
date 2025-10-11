using UnityEngine;
using UnityEngine.UI;

public class MenuUI : UIWindow
{
    [SerializeField] private Button _buttonGreen;
    [SerializeField] private Button _buttonContinue;


    public override void Initialize()
    {
        base.Initialize();

        if (_buttonGreen != null)
            _buttonGreen.onClick.AddListener(OpenSettings);
        if (_buttonContinue != null)
            _buttonContinue.onClick.AddListener(CloeSettings);
    }

    private void OpenSettings()
    {
        UIManager.Instance.HideUI(WindowID);
        UIManager.Instance.ShowUI(WindowsIDs.Settings);
    }
    private void CloeSettings() 
    {
        UIManager.Instance.HideUI(WindowID);
    }
}