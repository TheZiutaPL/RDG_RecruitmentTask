using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;

    /// <summary>
    /// Event invoked whenever health has been changed. Its arguments are NewHealthValue and AddedValue
    /// </summary>
    public Action<int, int> OnChangeHealth;
    public Action OnDeath;

    private void Start()
    {
        SetHealthToMax();
    }

    public void SetHealthToMax()
    {
        health = maxHealth;
    }

    public void AddHP(int add)
    {
        health = Mathf.Clamp(health + add, 0, maxHealth);
        OnChangeHealth?.Invoke(health, add);

        if (health < 0)
            Die();
    }

    public void Die()
    {
        OnDeath?.Invoke();
    }
}
