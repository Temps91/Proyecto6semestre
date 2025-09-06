using UnityEngine;
using DG.Tweening;

public class UIWindow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string windowID;
    [SerializeField] private Canvas windowCanvas;
    [SerializeField] private CanvasGroup windowCanvasGroup;

    [Header("Options")]
    [SerializeField] private bool hideOnStart = true;
    [SerializeField] private float animationTime = 0.5f;

    public string WindowID => windowID;

    public virtual void Initialize()
    {
        if (hideOnStart) Hide(instant: true);
    }
    public virtual void Show(bool instant = false)
    {
        if (instant)
        {
            windowCanvasGroup.transform.DOScale(Vector3.one, duration:0f);
        }
        else
        {
            windowCanvasGroup.transform.DOScale(Vector3.one, animationTime);
        }
    }

    public virtual void Hide(bool instant = false)
    {
        if (instant)
        {
            windowCanvasGroup.transform.DOScale(Vector3.zero, duration:0f);
        }
        else
        {
            windowCanvasGroup.transform.DOScale(Vector3.zero, animationTime);
        }
    }

}
