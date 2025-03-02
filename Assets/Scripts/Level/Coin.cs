using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private AudioClip pickSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //Add coins
            PlayerStats.Instance.AddCoins(value);

            //Plays sound
            AudioManager.Instance.PlaySFX(pickSound);

            //Destroys
            Destroy(gameObject);
        }
    }
}
