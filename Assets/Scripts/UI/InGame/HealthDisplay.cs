using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private HealthHeart healthHeartPrefab;
    private HealthHeart[] healthHearts;

    private void Start()
    {
        SpawnHearts();
        RefreshHearts(PlayerEntity.Instance.GetHealth());

        PlayerEntity.Instance.OnChangeHealth += RefreshHearts;
    }

    private void SpawnHearts()
    {
        healthHearts = new HealthHeart[PlayerEntity.Instance.GetMaxHealth()];

        //Spawn heart for each health
        for (int i = 0; i < healthHearts.Length; i++)
            healthHearts[i] = Instantiate(healthHeartPrefab, transform);
    }

    private void RefreshHearts(int health)
    {
        for (int i = 0; i < healthHearts.Length; i++)
            healthHearts[i].SetHeart(i < health);
    }
}
