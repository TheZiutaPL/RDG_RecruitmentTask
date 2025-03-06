using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerEntity : Entity
{
    public static PlayerEntity Instance { get; private set; }

    [SerializeField] private AudioClip playerRespawnSound;
    public Action OnRespawn;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        OnEndDeathState += Respawn;
    }

    private void Respawn()
    {
        SetHealthToMax();
        IsDead = false;

        transform.position = Vector2.zero;
        AudioManager.Instance.PlaySFX(playerRespawnSound);

        OnRespawn?.Invoke();
    }
}
