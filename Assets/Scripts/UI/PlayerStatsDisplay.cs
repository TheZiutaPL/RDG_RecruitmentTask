using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI deathsText;

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
        coinsText.SetText(PlayerStats.Instance.Coins.ToString());
        deathsText.SetText(PlayerStats.Instance.Deaths.ToString());
    }
}
