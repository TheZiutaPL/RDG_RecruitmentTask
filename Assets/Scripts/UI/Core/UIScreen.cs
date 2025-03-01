using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    //Should game pause on this screen
    [SerializeField] private bool pauseOnScreen;
    public bool GetScreenPause() => pauseOnScreen;

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
