using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    private bool timerToggled;
    public void ToggleTimer(bool toggle) => timerToggled = toggle;
    public float TimePassed { get; private set; }

    public int Coins { get; private set; }
    public int Deaths { get; private set; }

    public Action OnStatsChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        PlayerEntity.Instance.OnDeath += AddDeath;

        //Temp | there will be custscene before starting
        timerToggled = true;
    }

    private void Update()
    {
        if(timerToggled)
            TimePassed += Time.deltaTime;
    }

    public void AddCoins(int newCoins)
    {
        Coins += newCoins;

        RefreshStarts();
    }

    public void AddDeath()
    {
        Deaths++;
        RefreshStarts();
    }

    private void RefreshStarts() => OnStatsChanged?.Invoke();
}
