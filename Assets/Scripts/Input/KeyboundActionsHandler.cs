using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboundActionsHandler : MonoBehaviour
{
    private const string PAUSE_INPUT_NAME = "Pause";
    [SerializeField] private UIScreen pauseMenu;

    private void HandlePauseMenu()
    {
        if (!UIManagerMain.Instance.IsCurrentScreen(pauseMenu))
            UIManagerMain.Instance.SetUIScreen(pauseMenu);
        else
            UIManagerMain.Instance.SetUIScreenToDefault();
    }

    private void OnEnable()
    {
        InputManager.GameInputs.Game.Pause.performed += _ => HandlePauseMenu();
    }

    private void OnDisable()
    {
        InputManager.GameInputs.Game.Pause.performed -= _ => HandlePauseMenu();
    }
}
