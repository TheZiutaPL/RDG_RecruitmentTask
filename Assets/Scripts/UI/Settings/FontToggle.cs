using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private int falseFontIndex = 0;
    [SerializeField] private int trueFontIndex = 1;

    private void Awake()
    {
        toggle.onValueChanged.AddListener((x) => SettingsProfile.Instance.SetFont(x ? trueFontIndex : falseFontIndex));
    }

    private void OnEnable()
    {
        SetSettingToToggle();
    }

    private void SetSettingToToggle() => toggle.SetIsOnWithoutNotify(SettingsProfile.Instance.GetFontIndex() == trueFontIndex);
}
