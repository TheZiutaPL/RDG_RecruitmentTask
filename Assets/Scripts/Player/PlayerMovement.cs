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

    [Header("Visuals")]
    [SerializeField] private Transform playerVisualsTransform;
    [SerializeField] private Animator playerAnimator;

    private const string MOVEMENT_ANIMATION_BOOL = "moving";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        PlayerEntity.Instance.OnRespawn += () => ToggleMovement(true);
    }

    private void Update()
    {
        HandleVisuals();

        HandleMovement();
    }

    private void HandleVisuals()
    {
        //Sets moving animation
        bool isMoving = canMove && movementDirection != Vector2.zero;
        playerAnimator.SetBool(MOVEMENT_ANIMATION_BOOL, isMoving);

        if (!canMove)
            return;

        //Flips player visuals towards movement direction
        if ( movementDirection.x != 0)
            playerVisualsTransform.localScale = new Vector3(movementDirection.x > 0 ? 1 : -1, 1, 1);
    }

    private void HandleMovement()
    {
        if (!canMove)
            return;

        rb.AddForce(acceleration * Time.deltaTime * movementDirection.normalized);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    #region Handling Input Events
    public void GetMovementDirection(InputAction.CallbackContext ctx) => movementDirection = ctx.ReadValue<Vector2>();

    private void OnEnable()
    {
        if (InputManager.IsNull())
        {
            Debug.LogError("Couldn't subsribe to InputManager events. InputManager does not exist!");
            return;
        }

        InputManager.GetInputAction(MOVEMENT_DIRECTION_ACTION).performed += GetMovementDirection;
    }

    private void OnDisable()
    {
        if (InputManager.IsNull())
            return;

        InputManager.GetInputAction(MOVEMENT_DIRECTION_ACTION).performed -= GetMovementDirection;
    }
    #endregion
}
