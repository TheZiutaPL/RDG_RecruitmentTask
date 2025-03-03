using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [field: SerializeField, Min(1)] public int Value { get; private set; } = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //Add coins
            PlayerStats.Instance.AddCoins(Value);

            //Destroys itself
            Destroy(gameObject);
        }
    }
}
