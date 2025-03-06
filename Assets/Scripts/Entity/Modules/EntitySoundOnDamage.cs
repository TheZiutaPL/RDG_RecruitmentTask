using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EntitySoundOnDamage : MonoBehaviour
{
    [SerializeField] private AudioClip sound;

    private void Start()
    {
        GetComponent<Entity>().OnDamage += () => AudioManager.Instance.PlaySFX(sound); 
    }
}
