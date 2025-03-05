using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    public int GetMaxHealth() => maxHealth;
    private int health;
    public int GetHealth() => health;
    public bool IsDead { get; protected set; }

    public Action<int> OnChangeHealth;
    public Action OnDamage;
    public Action OnDeath;
    public Action OnEndDeathState;

    private void Start()
    {
        SetHealthToMax();
    }

    public void SetHealthToMax()
    {
        health = maxHealth;
        OnChangeHealth?.Invoke(health);
    }

    public void Damage()
    {
        health--;
        OnChangeHealth?.Invoke(health);

        if (IsDead)
            return;

        IsDead = health <= 0;
        if (IsDead)
            OnDeath?.Invoke();
        else
            OnDamage?.Invoke();
    }


    /// <summary>
    /// It is used through scripts and animations, cleanup after animated death
    /// </summary>
    public void EndDeathState()
    {
        OnEndDeathState?.Invoke();
    }
}
