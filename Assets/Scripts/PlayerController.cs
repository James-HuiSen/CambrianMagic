using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // Defines the different movement physics modes
    public enum MovementMode
    {
        Bilateral, // Fish-like, forward-focused movement
        Radial     // UFO-like, omnidirectional movement
    }

    [Header("Movement Settings")]
    public MovementMode currentMode = MovementMode.Radial; // Default mode
    public float moveSpeed = 10f;

    [Header("Bilateral (Fish) Multipliers")]
    public float forwardMultiplier = 1.2f;
    public float sidewaysMultiplier = 0.6f;
    public float backwardMultiplier = 0.3f;

    [Header("Radial (UFO) Multipliers")]
    public float radialMultiplier = 0.8f;
    
    private Rigidbody rb;
    private InputSystem_Actions controls;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new InputSystem_Actions();
        UpdatePhysicsProperties();
    }

    void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void FixedUpdate()
    {
        // Execute movement and rotation logic based on the current mode
        switch (currentMode)
        {
            case MovementMode.Bilateral:
                HandleBilateralMovement();
                HandleRotation();
                break;
            case MovementMode.Radial:
                HandleRadialMovement();
                // In Radial mode, body rotation is optional and could be handled by a separate turret script.
                // For now, we'll keep the body facing the mouse for consistency.
                HandleRotation();
                break;
        }
    }

    /// <summary>
    /// Handles fish-like movement: strong forward, weak sideways/backward.
    /// </summary>
    private void HandleBilateralMovement()
    {
        // Convert world-space input to local-space direction
        Vector3 localMoveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        // Determine force multiplier based on direction relative to where the player is facing
        float forceMultiplier = 1.0f;
        if (localMoveDirection.z > 0) // Moving forward (W)
        {
            forceMultiplier = forwardMultiplier;
        }
        else if (localMoveDirection.z < 0) // Moving backward (S)
        {
            forceMultiplier = backwardMultiplier;
        }
        
        // Sideways movement (A/D) is handled by the x component
        float sidewaysForce = Mathf.Abs(localMoveDirection.x) * sidewaysMultiplier;

        // Combine forces. We apply forward/backward thrust and sideways thrust separately.
        Vector3 forwardThrust = transform.forward * localMoveDirection.z * moveSpeed * forceMultiplier;
        Vector3 sidewaysThrust = transform.right * localMoveDirection.x * moveSpeed * sidewaysForce;

        rb.AddForce(forwardThrust + sidewaysThrust);
    }

    /// <summary>
    /// Handles UFO-like movement: equal force in all directions.
    /// </summary>
    private void HandleRadialMovement()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.AddForce(movement * moveSpeed * radialMultiplier);
    }

    /// <summary>
    /// Rotates the player's body to face the mouse cursor.
    /// </summary>
    private void HandleRotation()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        
        if (groundPlane.Raycast(cameraRay, out float rayLength))
        {
            Vector3 pointToLookAt = cameraRay.GetPoint(rayLength);
            Vector3 direction = new Vector3(pointToLookAt.x, transform.position.y, pointToLookAt.z);
            transform.LookAt(direction);
        }
    }
    
    /// <summary>
    /// Call this method when switching modes to update Rigidbody properties.
    /// </summary>
    public void UpdatePhysicsProperties()
    {
        switch (currentMode)
        {
            case MovementMode.Bilateral:
                // Lower angular drag allows for "drifting" or "tail-whipping"
                rb.angularDrag = 0.5f; 
                rb.drag = 1f; // Lower linear drag for higher top speed
                break;
            case MovementMode.Radial:
                // Higher angular drag for stable, non-rotating body
                rb.angularDrag = 5f; 
                rb.drag = 2f; // Higher linear drag for quicker stops and precise positioning
                break;
        }
    }

    // Public method to allow external scripts (like a SocketManager) to change the mode
    public void SetMovementMode(MovementMode newMode)
    {
        currentMode = newMode;
        UpdatePhysicsProperties();
    }
}