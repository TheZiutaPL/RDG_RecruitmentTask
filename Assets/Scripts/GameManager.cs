using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Action OnGameStarted;
    public Action OnGameEnded;

    public bool GamePaused { get; private set; }
    public Action<bool> OnGamePaused;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartGame()
    {
        OnGameStarted?.Invoke();
    }

    public void EndGame()
    {
        OnGameEnded?.Invoke();
    }

    public void PauseGame(bool pause)
    {
        GamePaused = pause;
        Time.timeScale = pause ? 0f : 1f;

        OnGamePaused?.Invoke(pause);
    }
}
