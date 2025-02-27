using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDieByAnimation : MonoBehaviour
{
    public void PlayerDie()
    {
        PlayerEntity.Instance.Die();
    }
}
