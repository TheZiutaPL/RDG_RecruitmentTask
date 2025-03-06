using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Entity))]
public class EntityDestroyOnDeath : MonoBehaviour
{
    [SerializeField] private float destroyAfterSeconds = 2;
    [SerializeField] private float tweenTime = .5f;

    private void Start()
    {
        GetComponent<Entity>().OnDeath += HandleDestroy;
    }

    private void HandleDestroy()
    {
        //Starts tween that after destroyAfterSeconds amount of time starts to scale down transform over tweenTime amount of time
        transform.DOScale(0, tweenTime).SetEase(Ease.OutSine).SetDelay(destroyAfterSeconds).OnComplete(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        //Kills all tweens if an object is destroyed first
        transform.DOKill();
    }
}
