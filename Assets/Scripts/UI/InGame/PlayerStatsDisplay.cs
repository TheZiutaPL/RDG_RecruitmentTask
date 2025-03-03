using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Color coinsGatheredColor;
    private Color defaultCoinsColor;

    [SerializeField] private TextMeshProUGUI deathsText;

    private void Awake()
    {
        defaultCoinsColor = coinsText.color;
    }

    private void Start()
    {
        PlayerStats.Instance.OnStatsChanged += RefreshStatsDisplay;
        RefreshStatsDisplay();
    }

    private void Update()
    {
        timeText.SetText(PlayerStats.Instance.TimePassed.ToString("0"));
    }

    private void RefreshStatsDisplay()
    {
        coinsText.color = GameManager.Instance.HasRequiredCoins() ? coinsGatheredColor : defaultCoinsColor;
        coinsText.SetText($"{PlayerStats.Instance.Coins}/{GameManager.Instance.GetRequiredCoins()}");

        deathsText.SetText(PlayerStats.Instance.Deaths.ToString());
    }
}
