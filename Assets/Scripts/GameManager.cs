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

    #region Player Goals
    private List<Transform> playerGoals = new List<Transform>();
    public void SetPlayerGoals(List<Transform> goals) => playerGoals = new List<Transform>(goals);
    public Vector3 GetClosestPlayerGoalPosition(Vector3 playerPosition)
    {
        Vector3 noGoalReturn = Vector3.zero;

        if (playerGoals.Count == 0)
            return noGoalReturn;
        else
        {
            int closestIndex = -1;
            float closestDistance = 50000;
            for (int i = 0; i < playerGoals.Count; i++)
            {
                Transform currentTransform = playerGoals[i];
                if (currentTransform == null)
                    continue;

                float distance = Vector3.Distance(playerPosition, currentTransform.position);
                if (distance < closestDistance)
                {
                    closestIndex = i;
                    closestDistance = distance;
                }
            }

            return closestIndex < 0 ? noGoalReturn : playerGoals[closestIndex].position;
        }
    }
    #endregion

    #region Coins
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
    #endregion

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
