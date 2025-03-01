using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool canMove = true;
    public void ToggleMovement(bool toggle) => canMove = toggle;

    [Header("Movement Settings")]
    [SerializeField] private float acceleration = 4000;
    [SerializeField] private float maxSpeed = 6.5f;

    private const string MOVEMENT_DIRECTION_ACTION = "Move";
    private Vector2 movementDirection;

    [Header("Water Movement")]
    [SerializeField] private float waterMovementSpeedMultiplier = .75f;
    public bool IsInWater { get; private set; }

    [Header("Visuals")]
    [SerializeField] private Transform playerVisualsTransform;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private ParticleSystem waterMovementParticles;
    private ParticleSystem.EmissionModule emitParticles;

    private const string MOVEMENT_ANIMATION_BOOL = "moving";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        emitParticles = waterMovementParticles.emission;
    }

    private void Start()
    {
        PlayerEntity.Instance.OnRespawn += () => ToggleMovement(true);
    }

    private void Update()
    {
        IsInWater = !LevelGenerator.IsGround(transform.position);

        HandleVisuals();

        HandleMovement();
    }

    private void HandleVisuals()
    {
        //Sets moving animation
        bool isMoving = canMove && movementDirection != Vector2.zero;
        playerAnimator.SetBool(MOVEMENT_ANIMATION_BOOL, isMoving);

        //Plays water particles while on water
        emitParticles.enabled = IsInWater && isMoving;

        if (!canMove)
            return;

        //Flips player visuals towards movement direction
        if (movementDirection.x != 0)
            playerVisualsTransform.localScale = new Vector3(movementDirection.x > 0 ? 1 : -1, 1, 1);
    }

    private void HandleMovement()
    {
        if (!canMove)
            return;

        rb.AddForce(acceleration * Time.deltaTime * movementDirection.normalized);

        //Float clamps max speed to correct value
        float finalSpeed = IsInWater ? maxSpeed * waterMovementSpeedMultiplier : maxSpeed;
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, finalSpeed);
    }

    #region Handling Input Events
    public void GetMovementDirection(InputAction.CallbackContext ctx) => movementDirection = ctx.ReadValue<Vector2>();

    private void OnEnable()
    {
        InputManager.GameInputs.Game.Move.performed += GetMovementDirection;
    }

    private void OnDisable()
    {
        InputManager.GameInputs.Game.Move.performed -= GetMovementDirection;
    }
    #endregion
}
