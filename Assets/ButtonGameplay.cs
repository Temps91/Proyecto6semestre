using UnityEngine;
using UnityEngine.UI;

public class ButtonGameplay : MonoBehaviour
{
    [SerializeField] private Button _buttonGameplay;

    private void Start()
    {
    }

    private void OpenScriptUI()
    {
        UIManager.Instance.ShowUI(WindowsIDs.Menu);
    }

    private void OnEnable()
    {
        if (_buttonGameplay != null)
        {
            _buttonGameplay.onClick.AddListener(OpenScriptUI);
        }
    }
}
