using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : UIWindow
{
    [SerializeField] private Button _buttonYes;  // Botón verde
    [SerializeField] private Button _buttonNo;   // Botón rojo o cerrar

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
        Debug.Log("Botón Sí presionado dentro de SettingsUI");

        // Por ejemplo, abrir otra ventana (opcional)
        // UIManager.Instance.ShowUI(WindowsIDs.OtraVentana);

        // Cerrar esta ventana
        UIManager.Instance.HideUI(WindowID);
    }

    private void OnNoClick()
    {
        Debug.Log("Botón No presionado dentro de SettingsUI");

        // Solo cerrar la ventana
        UIManager.Instance.HideUI(WindowID);
    }
}