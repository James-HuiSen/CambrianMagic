using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The player's Transform
    public float smoothSpeed = 0.125f; // The smoothness of the camera's movement

    private Vector3 offset; // This will be calculated automatically on start

    // Use Start() to set up initial values.
    void Start()
    {
        // Check if a target has been assigned.
        if (target != null)
        {
            // Calculate the initial offset between the camera and the target.
            // This captures the distance and angle you set in the Scene Editor.
            offset = transform.position - target.position;
        }
    }

    // LateUpdate runs after all other Update() calls, which is ideal for cameras.
    void LateUpdate()
    {
        if (target != null)
        {
            // The desired position is always the player's current position plus the initial offset.
            Vector3 desiredPosition = target.position + offset;
            
            // Smoothly move the camera towards the desired position.
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            // Update the camera's position.
            transform.position = smoothedPosition;
        }
    }
}