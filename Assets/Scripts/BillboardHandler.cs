using UnityEngine;

public class BillboardHandler : MonoBehaviour
{

    #region Fields
    public Camera MainCamera;
    #endregion

    void Update()
    {
        // find the main camera
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }

        // Make the text face the camera
        transform.LookAt(transform.position + MainCamera.transform.rotation * Vector3.forward, MainCamera.transform.rotation * Vector3.up);
    }
}
