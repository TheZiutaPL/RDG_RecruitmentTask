using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowAI : MonoBehaviour
{
    public void ToggleAI(bool toggle) => enabled = toggle;

    protected Transform playerTransform;
    private Rigidbody2D rb;
    protected virtual Vector2 GetMovementDirection() => rb.velocity.normalized;

    protected float playerDistance;
    protected float GetDistanceFromPlayer() => Vector2.Distance(transform.position, playerTransform.position);

    #region Override Behavior
    protected bool overrideBehavior;
    public bool IsBehaviorOverriden() => overrideBehavior;
    public void SetOverrideBehavior(bool set)
    {
        if (overrideBehavior == set)
            return;

        overrideBehavior = set;
        OnOverrideBehavior?.Invoke(set);
    }
    public Action<bool> OnOverrideBehavior;
    #endregion

    [SerializeField] protected float aiAcceleration;
    [SerializeField] protected float aiSpeed = 5;
    [SerializeField] protected float targetReachDistance = .75f;
    [SerializeField] private bool movesOnGround;

    [Header("Flipping and Moving Animation")]
    [SerializeField] private Transform aiVisualTransform;
    [SerializeField] private float flippingDirectionMargin = .2f;
    [SerializeField, Tooltip("Leave empty if you don't want moving animation")]
    private Animator animator;
    private const string MOVE_ANIMATION_BOOL = "moving";

    [Header("Detection and chasing")]
    [SerializeField] private float playerDetectionRange;
    [SerializeField] private float chaseRetainTime;
    private float chaseTimer;
    protected bool chase;
    private bool focusedChase;
    private void SetChase() => chaseTimer = chaseRetainTime;

    [Header("Free roaming")]
    [SerializeField] private float roamingRadius;
    [SerializeField] private float roamingMinTime, roamingMaxTime;
    private float roamingTimer;
    private Vector3 roamPosition;

    public Action<Vector3> OnRoamingPositionChanged;

    /// <summary>
    /// When enters a chase without losing chase retain
    /// </summary>
    public Action OnChaseRefresh;
    public Action OnChaseLost;
    public Action OnChaseTargetReached;

    private void Start()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        playerTransform = PlayerEntity.Instance.transform;

        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0;
    }

    private void Update()
    {
        RefreshAIState();

        HandleVisual();

        //Makes space for adding custom states, for example - attack state
        if (overrideBehavior)
            return;

        if (chase)
        {
            if (playerDistance < targetReachDistance)
                OnChaseTargetReached?.Invoke();

            HandleChase();
        }
        else
        {
            HandleFreeRoam();
            HandleRoamingTimer();
        }
    }

    private void HandleVisual()
    {
        Vector2 movementDirection = GetMovementDirection();

        //Movement animation
        if (animator != null)
            animator.SetBool(MOVE_ANIMATION_BOOL, movementDirection != Vector2.zero);

        //Flipping
        bool flip = Mathf.Abs(movementDirection.x) > flippingDirectionMargin;
        if (flip)
            aiVisualTransform.localScale = new Vector3(movementDirection.x > 0 ? 1 : -1, 1, 1);
    }

    private void RefreshAIState()
    {
        playerDistance = GetDistanceFromPlayer();
        bool wasChasing = chaseTimer > 0;

        if (!PlayerEntity.Instance.IsDead)
        {
            bool playerInRange = playerDistance <= playerDetectionRange;
            if (playerInRange)
            {
                SetChase();

                //If just entered a focused chase state
                if (!focusedChase)
                {
                    focusedChase = true;
                    OnChaseRefresh?.Invoke();
                }
            }
            else
            {
                focusedChase = false;

                if (wasChasing)
                    chaseTimer -= Time.deltaTime;
            }
        }
        else
        {
            focusedChase = false;
            chaseTimer = 0;
        }

        chase = chaseTimer > 0;

        //If was chasing at the start of the method and now is not
        if (!chase && wasChasing)
            OnChaseLost?.Invoke();
    }

    #region Roaming
    private void HandleRoamingTimer()
    {
        if (roamingTimer > 0)
            roamingTimer -= Time.deltaTime;
        else
        {
            roamingTimer = Random.Range(roamingMinTime, roamingMaxTime);
            roamPosition = GetRoamingPosition();

            OnRoamingPositionChanged?.Invoke(roamPosition);
        }
    }

    protected virtual void HandleFreeRoam()
    {
        MoveTowards(roamPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        //Try to get position on ground (max 5 times)
        for (int i = 0; i < 5; i++)
        {
            Vector3 pos = transform.position + (Vector3)(roamingRadius * Random.insideUnitCircle);

            if (!movesOnGround || LevelGenerator.IsGround(pos))
                return pos;
        }

        return transform.position;
    }
    #endregion

    #region Chase
    protected virtual void HandleChase()
    {
        MoveTowards(playerTransform.position);
    }
    #endregion

    private void MoveTowards(Vector3 position)
    {
        float distance = Vector3.Distance(position, transform.position);
        if (distance < targetReachDistance)
            return;

        Vector3 movementDirection = (position - transform.position).normalized;
        rb.AddForce(aiAcceleration * Time.deltaTime * movementDirection);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, aiSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, playerDetectionRange);
    }
}
