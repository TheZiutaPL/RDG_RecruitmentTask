using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

[CreateAssetMenu(fileName = "New SettingsProfile", menuName = "Settings Profile")]
public class SettingsProfile : ScriptableObject
{
    public static SettingsProfile Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    private const float LOWEST_AUDIO_VALUE = .0001f;

    //Master Volume
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    [SerializeField] private float defaultMasterVolume = .5f;
    public Action<float> OnMasterVolumeChanged;

    //Music Volume
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    [SerializeField] private float defaultMusicVolume = .5f;
    public Action<float> OnMusicVolumeChanged;

    //SFX Volume
    private const string SFX_VOLUME_KEY = "SFXVolume";
    [SerializeField] private float defaultSFXVolume = .5f;
    public Action<float> OnSFXVolumeChanged;

    [Header("Fonts")]
    [SerializeField] private TMP_FontAsset[] fonts = new TMP_FontAsset[0];
    private const string FONT_KEY = "FontIndex";
    [SerializeField] private int defaultFont;
    public Action OnFontChanged;

    public void InitializeSettingsProfile()
    {
        if (Instance != null)
            return;

        Instance = this;

        Debug.Log("Settings Initialized");
    }

    #region Audio
    public void RefreshAudioMixerVolumes()
    {
        SetMasterVolume(GetMasterVolume());
        SetMusicVolume(GetMusicVolume());
        SetSFXVolume(GetSFXVolume());
    }

    public float GetMasterVolume() => PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, defaultMasterVolume);
    public void SetMasterVolume(float volume)
    {
        //Clamps value between 0 and 1
        volume = Mathf.Clamp(volume, LOWEST_AUDIO_VALUE, 1);

        //Sets volume
        audioMixer.SetFloat(MASTER_VOLUME_KEY, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);

        //Callback event
        OnMasterVolumeChanged?.Invoke(volume);
    }

    public float GetMusicVolume() => PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, defaultMusicVolume);
    public void SetMusicVolume(float volume)
    {
        //Clamps value between 0 and 1
        volume = Mathf.Clamp(volume, LOWEST_AUDIO_VALUE, 1);

        //Sets volume
        audioMixer.SetFloat(MUSIC_VOLUME_KEY, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);

        //Callback event
        OnMusicVolumeChanged?.Invoke(volume);
    }

    public float GetSFXVolume() => PlayerPrefs.GetFloat(SFX_VOLUME_KEY, defaultSFXVolume);
    public void SetSFXVolume(float volume)
    {
        //Clamps value between 0 and 1
        volume = Mathf.Clamp(volume, LOWEST_AUDIO_VALUE, 1);

        //Sets volume
        audioMixer.SetFloat(SFX_VOLUME_KEY, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);

        //Callback event
        OnSFXVolumeChanged?.Invoke(volume);
    }
    #endregion

    #region Fonts
    public int GetFontIndex() => PlayerPrefs.GetInt(FONT_KEY, defaultFont);
    public TMP_FontAsset GetFont()
    {
        int index = GetFontIndex();
        if (index < 0 && index >= fonts.Length)
            return null;

        return fonts[index];
    }
    public void SetFont(int fontIndex)
    {
        PlayerPrefs.SetInt(FONT_KEY, fontIndex);
    }
    #endregion
}
