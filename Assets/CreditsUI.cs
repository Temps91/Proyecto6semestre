using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : UIWindow
{
    [SerializeField] private Button _continueButton;

    public override void Initialize()
    {
        base.Initialize();

        if (_continueButton != null)
            _continueButton.onClick.AddListener(Cerrartodo);
    }

    private void Cerrartodo()
    {
        Debug.Log("Cerrando todas las ventanas desde CreditsUI");
        UIManager.Instance.HideUI(WindowID);
    }
}
