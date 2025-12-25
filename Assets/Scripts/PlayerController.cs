using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody rb;
    private InputSystem_Actions controls;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new InputSystem_Actions();
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
        // 1. Handle WASD movement by applying force
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        rb.AddForce(movement * moveSpeed);

        // 2. Handle mouse aiming by rotating the player
        HandleMouseAiming();
    }

    void HandleMouseAiming()
    {
        // Use the main camera to cast a ray to a plane at the player's height
        Ray cameraRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        
        if (groundPlane.Raycast(cameraRay, out float rayLength))
        {
            Vector3 pointToLookAt = cameraRay.GetPoint(rayLength);
            
            // Ensure the player looks at the point without tilting up or down
            Vector3 direction = new Vector3(pointToLookAt.x, transform.position.y, pointToLookAt.z);
            
            transform.LookAt(direction);
        }
    }
}
