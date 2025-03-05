using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class UITransition : MonoBehaviour
{
    private static UITransition Instance;
    private CanvasGroup canvasGroup;

    [SerializeField] private int waitingFrames = 3;
    [SerializeField] private float transitionTime;
    private Coroutine transition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        ToggleTransition(false);
    }

    public static void ToggleTransition(bool toggle, Action callback = null)
    {
        if (Instance.transition != null)
            return;

        Instance.transition = Instance.StartCoroutine(Instance.Transition(toggle, callback));
    }

    IEnumerator Transition(bool toggle, Action callback)
    {
        canvasGroup.alpha = toggle ? 0f : 1f;

        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        for (int i = 0; i < waitingFrames; i++)
            yield return wait;

        float timer = 0;
        while (timer <= transitionTime)
        {
            timer += Time.unscaledDeltaTime;

            float blend = Mathf.Clamp01(timer / transitionTime);

            if (!toggle) blend = 1 - blend;

            canvasGroup.alpha = blend;

            yield return null;
        }

        callback?.Invoke();
        transition = null;
    }
}
