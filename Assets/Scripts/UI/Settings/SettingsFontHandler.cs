using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SettingsFontHandler : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void DrawFont() => text.font = SettingsProfile.Instance.GetFont();

    private void OnEnable()
    {
        DrawFont();

        SettingsProfile.Instance.OnFontChanged += DrawFont;
    }

    private void OnDisable()
    {
        SettingsProfile.Instance.OnFontChanged -= DrawFont;
    }
}
