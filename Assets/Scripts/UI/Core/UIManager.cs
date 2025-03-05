using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private float screenTransitionTime;
    [SerializeField] private bool instantChangeOnStart;
    [SerializeField] private UIScreen defaultScreen;
    private UIScreen currentScreen;
    public bool IsOnDefaultScreen() => currentScreen == defaultScreen;
    public bool IsCurrentScreen(UIScreen screen) => currentScreen == screen;

    private Coroutine changeCoroutine;
    public bool IsChanging() => changeCoroutine != null;

    public void SetUIScreenToDefaultInstant() => SetUIScreenInstant(defaultScreen);
    public void SetUIScreenToDefault() => SetUIScreen(defaultScreen);

    private void Start()
    {
        if (instantChangeOnStart)
            SetUIScreenToDefaultInstant();
        else
            SetUIScreenToDefault();
    }

    public void SetUIScreenInstant(UIScreen newScreen)
    {
        if (IsChanging() || currentScreen == newScreen)
            return;

        changeCoroutine = StartCoroutine(ChangeUIScreenCoroutine(newScreen, 0));
    }

    public void SetUIScreen(UIScreen newScreen)
    {
        if (IsChanging() || currentScreen == newScreen)
            return;

        changeCoroutine = StartCoroutine(ChangeUIScreenCoroutine(newScreen, screenTransitionTime));
    }

    IEnumerator ChangeUIScreenCoroutine(UIScreen newScreen, float time)
    {
        bool isMainInstance = UIManagerMain.Instance == this;

        bool currentNotNull = currentScreen != null;
        bool newNotNull = newScreen != null;

        bool shouldBePaused = newNotNull && newScreen.GetScreenPause();

        //Turns interactability off
        if (currentNotNull)
            currentScreen.TurnUIScreenGroup(false);

        //Turns new screen on and pauses game if necessary (on main instance)
        if (newNotNull)
        {
            newScreen.gameObject.SetActive(true);
            newScreen.TurnUIScreenGroup(false);

            //Invokes event at the start of changing screen
            newScreen.InvokeEventOnChangingToScreen();

            //Pause work only with main instances
            if (isMainInstance) 
            { 
                if (!GameManager.Instance.GamePaused && shouldBePaused)
                    GameManager.Instance.PauseGame(true);
            }
        }

        //Blends transition between two screens
        if (time > 0)
        {
            float timer = 0;
            while (timer <= time)
            {
                timer += Time.unscaledDeltaTime;
                float blend = Mathf.Clamp01(timer / time);

                //Chages alpha by time passed
                if (currentNotNull) currentScreen.SetUIScreenGroupAlpha(1 - blend);
                if (newNotNull) newScreen.SetUIScreenGroupAlpha(blend);

                yield return null;
            }
        }
        else
        {
            if (currentNotNull) currentScreen.SetUIScreenGroupAlpha(0);
            if (newNotNull) newScreen.SetUIScreenGroupAlpha(1);
        }

        //Turns old screen off
        if (currentNotNull)
            currentScreen.gameObject.SetActive(false);

        //Turns interactability on new screen
        if (newNotNull)
            newScreen.TurnUIScreenGroup(true);

        //Unpauses game if necessary, works only with main instances
        if (isMainInstance)
        {
            if (GameManager.Instance.GamePaused && !shouldBePaused)
                GameManager.Instance.PauseGame(false);
        }

        //Finals sets
        currentScreen = newScreen;
        changeCoroutine = null;
    }
}
