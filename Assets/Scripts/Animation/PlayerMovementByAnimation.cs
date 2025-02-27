using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementByAnimation : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = PlayerEntity.Instance.GetComponent<PlayerMovement>();
    }

    public void EnableMovement()
    {
        playerMovement.ToggleMovement(true);
    }

    public void DisableMovement()
    {
        playerMovement.ToggleMovement(false);
    }
}
