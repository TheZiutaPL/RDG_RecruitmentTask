using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerEntity : Entity
{
    public static PlayerEntity Instance { get; private set; }

    public Action OnRespawn;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        OnDeath += Respawn;
    }

    private void Respawn()
    {
        SetHealthToMax();

        transform.position = Vector2.zero;

        OnRespawn?.Invoke();
    }
}
