using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDieAnimationTrigger : MonoBehaviour
{
    public void PlayerDie()
    {
        PlayerEntity.Instance.EndDeathState();
    }
}
