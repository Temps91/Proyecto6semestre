using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : UIWindow
{
    [SerializeField] private Button _buttonYes;  // Bot�n verde
    [SerializeField] private Button _buttonNo;   // Bot�n rojo o cerrar

    public override void Initialize()
    {
        base.Initialize();

        // Asignar listeners a los botones si existen
        if (_buttonYes != null)
            _buttonYes.onClick.AddListener(OnYesClick);

        if (_buttonNo != null)
            _buttonNo.onClick.AddListener(OnNoClick);
    }

    private void OnYesClick()
    {
        Debug.Log("Bot�n S� presionado dentro de SettingsUI");

        // Por ejemplo, abrir otra ventana (opcional)
        // UIManager.Instance.ShowUI(WindowsIDs.OtraVentana);

        // Cerrar esta ventana
        UIManager.Instance.HideUI(WindowID);
    }

    private void OnNoClick()
    {
        Debug.Log("Bot�n No presionado dentro de SettingsUI");

        // Solo cerrar la ventana
        UIManager.Instance.HideUI(WindowID);
    }
}