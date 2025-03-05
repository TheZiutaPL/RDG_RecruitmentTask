using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    //Should game pause on this screen
    [SerializeField] private bool pauseOnScreen;
    public bool GetScreenPause() => pauseOnScreen;

    [SerializeField] private UnityEvent onStartChangingToScreen;
    public void InvokeEventOnChangingToScreen() => onStartChangingToScreen?.Invoke();

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(false);
    }

    public void SetUIScreenGroupAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
    }

    public void TurnUIScreenGroup(bool turn)
    {
        canvasGroup.interactable = turn;
        canvasGroup.blocksRaycasts = turn;
    }
}
