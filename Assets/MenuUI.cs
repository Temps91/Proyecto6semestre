using UnityEngine;
using UnityEngine.UI;

public class MenuUI : UIWindow
{
    [SerializeField] private Button _buttonGreen;  // Botón que abrirá SettingsUI

    public override void Initialize()
    {
        base.Initialize();

        if (_buttonGreen != null)
            _buttonGreen.onClick.AddListener(OpenSettings);
    }

    private void OpenSettings()
    {
        Debug.Log("Botón verde presionado"); // <-- verifica que aparezca
        UIManager.Instance.HideUI(WindowID);
        UIManager.Instance.ShowUI(WindowsIDs.Settings);
    }
}