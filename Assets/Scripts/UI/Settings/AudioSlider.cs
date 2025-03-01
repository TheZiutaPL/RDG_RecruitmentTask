using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    private enum AudioType
    {
        Master,
        Music,
        SFX
    }

    [SerializeField] private AudioType audioType;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        slider.onValueChanged.AddListener(audioType switch
        {
            AudioType.Master => SettingsProfile.Instance.SetMasterVolume,
            AudioType.Music => SettingsProfile.Instance.SetMusicVolume,
            AudioType.SFX => SettingsProfile.Instance.SetSFXVolume,
            _ => throw new System.NotImplementedException(),
        });
    }

    private void OnEnable()
    {
        SetSettingToSlider();
    }

    private void SetSettingToSlider()
    {
        float value = audioType switch
        {
            AudioType.Master => SettingsProfile.Instance.GetMasterVolume(),
            AudioType.Music => SettingsProfile.Instance.GetMusicVolume(),
            AudioType.SFX => SettingsProfile.Instance.GetSFXVolume(),
            _ => throw new System.NotImplementedException(),
        };

        slider.SetValueWithoutNotify(value);
    }
}
