using UnityEngine;

public class FaceCameraLeon : MonoBehaviour
{
        private Camera mainCamera;
        void Start()
        {
            mainCamera = Camera.main;
        }

        void LateUpdate()
        {
            //TODO: expensive check
            if (mainCamera != null)
            {
                transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                    mainCamera.transform.rotation * Vector3.up);
            }
        }
    
}
