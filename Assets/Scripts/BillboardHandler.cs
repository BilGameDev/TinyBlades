using UnityEngine;

public class BillboardHandler : MonoBehaviour
{
   public Camera mainCamera;

    void Update()
    {
        // find the main camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Make the text face the camera
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
