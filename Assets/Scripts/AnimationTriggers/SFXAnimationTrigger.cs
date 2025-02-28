using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXAnimationTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds = new AudioClip[0];

    public void PlaySound(int index)
    {
        if (index < 0 || index >= sounds.Length)
            return;

        AudioManager.Instance.PlaySFX(sounds[index]);
    }
}
