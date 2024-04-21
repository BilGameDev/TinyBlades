using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region Fields
    public Transform TargetToFollow; // The target object to follow
    public Vector3 FollowOffset = new Vector3(0, 2, -10); // Offset from the target object
    public float SmoothSpeed = 0.125f; // How smoothly the camera catches up with its target
    public float RotationSmoothSpeed = 0.1f; // Smoothness of the camera rotation

    #endregion

    void LateUpdate()
    {
        if (!TargetToFollow) return;

        // Position smoothing
        Vector3 desiredPosition = TargetToFollow.position + FollowOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed);
        transform.position = smoothedPosition;

        // Rotation smoothing
        Quaternion targetRotation = Quaternion.LookRotation(TargetToFollow.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSmoothSpeed);
    }

    public void SetTarget(Transform targetTransform)
    {
        TargetToFollow = targetTransform;
    }
}
