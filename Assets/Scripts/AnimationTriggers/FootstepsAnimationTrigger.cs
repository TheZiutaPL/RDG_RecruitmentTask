using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsAnimationTrigger : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private AudioClip[] footstepsSounds = new AudioClip[0];
    [SerializeField] private AudioClip[] waterFootstepsSounds = new AudioClip[0];

    public void PlayFootstepSound()
    {
        AudioManager.Instance.PlaySFX(playerMovement.IsInWater ? waterFootstepsSounds : footstepsSounds);
    }
}
