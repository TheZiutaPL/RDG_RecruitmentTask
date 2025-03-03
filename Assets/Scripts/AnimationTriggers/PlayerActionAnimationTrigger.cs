using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionAnimationTrigger : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;

    private void Start()
    {
        playerMovement = PlayerEntity.Instance.GetComponent<PlayerMovement>();
        playerAttack = PlayerEntity.Instance.GetComponent<PlayerAttack>();
    }

    public void EnablePlayerAction()
    {
        playerMovement.ToggleMovement(true);
        playerAttack.ToggleAttack(true);
        PlayerInteraction.Instance.ToggleInteraction(true);
    }

    public void DisablePlayerAction()
    {
        playerMovement.ToggleMovement(false);
        playerAttack.ToggleAttack(false);
        PlayerInteraction.Instance.ToggleInteraction(false);
    }
}
