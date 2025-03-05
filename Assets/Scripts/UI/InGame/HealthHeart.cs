using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeart : MonoBehaviour
{
    [SerializeField] private GameObject heartFillObject;

    public void SetHeart(bool on) => heartFillObject.SetActive(on);
}
