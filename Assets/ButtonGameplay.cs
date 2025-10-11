using UnityEngine;
using UnityEngine.UI;

public class ButtonGameplay : MonoBehaviour
{
    [SerializeField] private Button _buttonGameplay;

    private void Start()
    {
        if (_buttonGameplay != null)
        {
            _buttonGameplay.onClick.AddListener(OpenScriptUI);
        }
    }

    private void OpenScriptUI()
    {
        UIManager.Instance.ShowUI(WindowsIDs.Menu);
    }
}
