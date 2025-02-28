using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsInitializer : MonoBehaviour
{
    [SerializeField] private SettingsProfile settingsProfile;

    private void Awake()
    {
        settingsProfile.InitializeSettingsProfile();
    }

    private void Start()
    {
        SettingsProfile.Instance.RefreshAudioMixerVolumes();
    }
}
