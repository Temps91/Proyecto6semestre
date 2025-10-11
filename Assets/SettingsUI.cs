using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : UIWindow
{
    [SerializeField] private Button _creditsButton;  
    [SerializeField] private Button _continueButton;   

    public override void Initialize()
    {
        base.Initialize();


        if (_creditsButton != null)
            _creditsButton.onClick.AddListener(CreditsButton);

        if (_continueButton != null)
            _continueButton.onClick.AddListener(ContinueButton);
    }

    private void CreditsButton()
    {

        UIManager.Instance.HideUI(WindowID);
        UIManager.Instance.ShowUI(WindowsIDs.Credits);
    }

    private void ContinueButton()
    {
        UIManager.Instance.HideUI(WindowID);
    }
}