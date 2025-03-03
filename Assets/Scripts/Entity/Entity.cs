using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Entity : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;

    public Action<int> OnChangeHealth;
    public Action OnDeath;

    private void Start()
    {
        SetHealthToMax();
    }

    public void SetHealthToMax()
    {
        health = maxHealth;
    }

    public void Damage()
    {
        health--;
        OnChangeHealth?.Invoke(health);

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        OnDeath?.Invoke();
    }
}
