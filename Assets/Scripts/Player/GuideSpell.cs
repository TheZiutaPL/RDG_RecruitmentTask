using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GuideSpell : MonoBehaviour
{
    [SerializeField] private ParticleSystem guideParticles;
    [SerializeField] private float particleLifetimeMargin = .5f;
    [SerializeField] private float guideSpeed = 5.5f;

    public void SetGuideSpell(Vector3 position)
    {
        //Makes projectile face its direction
        transform.rotation = Quaternion.LookRotation(position);

        transform.DOMove(position, guideSpeed).SetSpeedBased(true);

        //Destroys projectile after some time
        Destroy(gameObject, guideParticles.main.duration + particleLifetimeMargin);
    }

    private void OnDestroy()
    {
        //Kills all tweens if an object is destroyed first
        transform.DOKill();
    }
}
