using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssalAttack : MonoBehaviour
{
    private Transform followedPlayer;
    private bool followPlayer = true;

    [Header("Visuals")]
    [SerializeField] private Transform lurkingShadowTransform;
    [SerializeField] private Vector3 endLurkingScale;
    [Space(5)]
    [SerializeField] private SpriteRenderer lurkingShadowRenderer;
    [SerializeField] private Color startLurkingColor;
    [SerializeField] private Color endLurkingColor;
    [SerializeField] private AnimationCurve lurkingColorCurve;
    [Space(5)]
    [SerializeField] private Animator abyssalAnimation;

    private const string ABYSSAL_ATTACK_TRIGGER = "Attack";

    [Header("Setting")]
    [SerializeField] private float maxDistanceFromCenter = 150;
    private float distance;

    [SerializeField] private float lurkingTime;
    private float lurkingTimer;
    private bool isLurking;

    [SerializeField] private float attackCooldown;
    private Coroutine attackCoroutine;

    private void Start()
    {
        followedPlayer = PlayerEntity.Instance.transform;
    }

    private void Update()
    {
        if (followPlayer)
            transform.position = followedPlayer.position;

        if (attackCoroutine != null)
            return;


        distance = Vector3.Distance(followedPlayer.position, Vector3.zero);

        //Changes lurking status
        if (isLurking != distance > maxDistanceFromCenter)
        {
            isLurking = !isLurking;

            if (lurkingTimer < 0)
                lurkingTimer = 0;

            return;
        }

        //Adds time to timer
        lurkingTimer += isLurking ? Time.deltaTime : -Time.deltaTime;

        float blend = Mathf.Clamp01(lurkingTimer / lurkingTime);

        //Updates visuals
        lurkingShadowTransform.localScale = Vector3.Lerp(Vector3.zero, endLurkingScale, blend);
        lurkingShadowRenderer.color = Color.Lerp(startLurkingColor, endLurkingColor, lurkingColorCurve.Evaluate(blend));

        if (lurkingTimer > lurkingTime)
            Attack();
    }

    public void Attack()
    {
        attackCoroutine = StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        //Animation
        abyssalAnimation.SetTrigger(ABYSSAL_ATTACK_TRIGGER);

        yield return new WaitForSeconds(.2f);
        followPlayer = false;

        yield return new WaitForSeconds(2f);
        followPlayer = true;

        lurkingTimer = 0;
        attackCoroutine = null;
    }
}
