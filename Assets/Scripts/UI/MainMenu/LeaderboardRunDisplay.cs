using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardRunDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI runIndexText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI deathsText;

    [Header("New Run Animation")]
    [SerializeField] private float minScale = .95f;
    [SerializeField] private float maxScale = 1.05f;
    [SerializeField] private float animationDuration = 2;
    private bool isNew;

    private void Start()
    {
        if (!isNew)
            return;

        float sineInput = Time.timeSinceLevelLoad % animationDuration / animationDuration * Mathf.PI * 2;
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(sineInput) + 1) / 2);
        SetSineAnimationScale(scale);
    }

    public void SetRunDisplay(SaveUnit displayUnit, int unitIndex, bool isNew = false)
    {
        playerNameText.SetText(displayUnit.name);
        runIndexText.SetText($"#{unitIndex + 1}");
        timeText.SetText($"{displayUnit.time.ToString("0.00")}s");
        coinsText.SetText(displayUnit.coins.ToString());
        deathsText.SetText(displayUnit.deaths.ToString());

        this.isNew = isNew;
        SetSineAnimationScale();
    }

    private void SetSineAnimationScale(float scale = 1) => transform.localScale = new Vector3(scale, scale, 1);
}
