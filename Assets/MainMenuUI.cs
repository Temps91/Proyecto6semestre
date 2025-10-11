using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class MainMenuUI : UIWindow
{
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Button _buttonSettings;
    [SerializeField] private Button _buttonCredits;

    public override void Initialize()
    {
        base.Initialize();

        if (_buttonPlay != null)
            _buttonPlay.onClick.AddListener(OnPlayClicked);

        if (_buttonSettings != null)
            _buttonSettings.onClick.AddListener(OnSettingsClicked);

        if (_buttonCredits != null)
            _buttonCredits.onClick.AddListener(OnCreditsClicked);
    }

    private void OnPlayClicked()
    {

        SceneManager.LoadScene("SampleScene");
    }

    private void OnSettingsClicked()
    {

        UIManager.Instance.ShowUI(WindowsIDs.Settings);
    }

    private void OnCreditsClicked()
    {
        UIManager.Instance.ShowUI(WindowsIDs.Credits);
    }
}
