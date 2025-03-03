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

    [SerializeField] private int requiredCoins;
    public int GetRequiredCoins() => requiredCoins;
    public bool HasRequiredCoins() => PlayerStats.Instance.Coins >= requiredCoins;
    private void CheckRequiredCoins()
    {
        if (PlayerStats.Instance.Coins < GetRequiredCoins())
            return;

        OnRequiredCoinsGathered?.Invoke();
    }
    public Action OnRequiredCoinsGathered;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    IEnumerator Start()
    {
        PlayerStats.Instance.OnStatsChanged += CheckRequiredCoins;

        //Wait for all other events to set up before starting
        yield return new WaitForEndOfFrame();
        StartGame();
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
