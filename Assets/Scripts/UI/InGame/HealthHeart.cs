using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealthHeart : MonoBehaviour
{
    [SerializeField] private GameObject heartFillObject;
    [SerializeField] private float animationTime = .15f;
    [SerializeField] private float animationShakeValue = .1f;
    private bool isOn = true;

    public void SetHeart(bool on)
    {
        if (isOn == on)
            return;

        isOn = on;
        transform.DOKill(true);
        transform.DOShakeScale(animationTime, animationShakeValue);

        heartFillObject.SetActive(on);
    }

    private void OnDestroy()
    {
        //Kills all tweens if an object is destroyed first
        transform.DOKill();
    }
}
