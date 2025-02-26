using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float acceleration = 4000;
    [SerializeField] private float maxSpeed = 6.5f;

    private const string MOVEMENT_DIRECTION_ACTION = "Move";
    private Vector2 movementDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
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
