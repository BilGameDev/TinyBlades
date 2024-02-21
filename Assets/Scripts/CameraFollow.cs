using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target object to follow
    public Vector3 offset = new Vector3(0, 2, -10); // Offset from the target object
    public float smoothSpeed = 0.125f; // How smoothly the camera catches up with its target
    public float rotationSmoothSpeed = 0.1f; // Smoothness of the camera rotation

    void LateUpdate()
    {
        if (!target) return;

        // Position smoothing
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Rotation smoothing
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed);
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }
}
