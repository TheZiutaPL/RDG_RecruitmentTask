using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //Add coins
            PlayerStats.Instance.AddCoins(value);

            //Destroys itself
            Destroy(gameObject);
        }
    }
}
