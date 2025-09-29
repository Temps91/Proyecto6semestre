using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : UIWindow
{
    [SerializeField] private Button _buttonYes;
    [SerializeField] private Button _buttonNo;

    public override void Initialize()
    {
        base.Initialize();

        if (_buttonYes != null)
            _buttonYes.onClick.AddListener(YesClick);

        if (_buttonNo != null)
            _buttonNo.onClick.AddListener(NoClick);
    }

    private void YesClick()
    {
        Debug.Log("Yes Clicked Abriendo MenuUI");
        UIManager.Instance.HideUI(WindowID);
        UIManager.Instance.ShowUI(WindowsIDs.Menu);
    }

    private void NoClick()
    {
        Debug.Log("No Clicked Cerrando SettingsUI");
        UIManager.Instance.HideUI(WindowID);
    }
}
