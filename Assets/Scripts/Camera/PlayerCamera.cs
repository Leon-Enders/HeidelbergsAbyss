using GameStates;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Vector2 lookSpeed;
    [SerializeField] private Transform moveOrientation;
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 aimCameraPosition = new Vector3(0.5f,0f,-1f);
    [SerializeField] private float lerpSpeed = 1f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform aimTransform;
    [SerializeField] private LayerMask cameraLayer;

   
    private float curLookSmoothingSpeed;
    private float maxCameraDistance;
    private Vector2 desiredCameraRotation;
    private Vector2 mouseXY;
    private Vector2 rotation;
    private Vector2 rotationVelocity;
    private Vector3 baseCameraPosition;
    private Vector3 desiredCameraPosition;
    
    private ICharacterStateInterface characterStateInterface;
    private CastleAdventureInputActions controls;
    
    
    private void Awake()
    {
        characterStateInterface = GetComponent<ICharacterStateInterface>();
      
        controls = new CastleAdventureInputActions();
        controls.PlayerControls.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        controls.PlayerControls.Look.canceled += ctx => Look(Vector2.zero);
        
    }
    
    private void OnEnable()
    {
        if (controls != null)
        {
            controls.PlayerControls.Enable();
        }
    }

    private void OnDisable()
    {
        controls.PlayerControls.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        baseCameraPosition = playerCamera.transform.localPosition;
        
        maxCameraDistance = Vector3.Distance(playerCamera.transform.position, followTarget.position);
    }

    void LateUpdate()
    {
        CheckCameraCollision();
        if (characterStateInterface.CurrentActionState == ActionStates.AimActive)
        {
            HandleCrossbowCameraRotation();
        }
        else
        {
            HandleCameraRotation();
        }
    }
    
    private void Look(Vector2 mouseInput)
    {
        desiredCameraRotation.y = mouseInput.x* lookSpeed.y * Time.deltaTime * -1f;
        desiredCameraRotation.x = mouseInput.y * lookSpeed.y * Time.deltaTime * -1f;
        
    }

    private void HandleCameraRotation()
    {
        
        Vector3 cameraPosition = playerCamera.transform.localPosition;
        cameraPosition = Vector3.Lerp(cameraPosition, desiredCameraPosition, lerpSpeed * Time.deltaTime);
        playerCamera.transform.localPosition = cameraPosition;
        

        followTarget.Rotate(0f, desiredCameraRotation.y, 0f, Space.World);
        followTarget.Rotate(desiredCameraRotation.x, 0f, 0f, Space.Self);

        var currentRotationEuler = followTarget.rotation.eulerAngles;
        
        float normalizedX = currentRotationEuler.x > 180f ? currentRotationEuler.x - 360f : currentRotationEuler.x;

        
        normalizedX = Mathf.Clamp(normalizedX, -60f, 60f);
        
        var newRotationEuler = new Vector3(normalizedX, currentRotationEuler.y, 0f);
        followTarget.transform.SetPositionAndRotation( followTarget.position,Quaternion.Euler(newRotationEuler));
        
        moveOrientation.rotation = Quaternion.Euler(0,newRotationEuler.y,0);
        
    }

    private void HandleCrossbowCameraRotation()
    {
        Vector3 cameraPosition = playerCamera.transform.localPosition;
        cameraPosition = Vector3.Lerp(cameraPosition, aimCameraPosition, lerpSpeed * Time.deltaTime);
        
        playerCamera.transform.localPosition = cameraPosition;
        
        followTarget.Rotate(0f, desiredCameraRotation.y, 0f, Space.World);
        followTarget.Rotate(desiredCameraRotation.x, 0f, 0f, Space.Self);

        var currentRotationEuler = followTarget.rotation.eulerAngles;
        
        float clampedX = currentRotationEuler.x > 180f ? currentRotationEuler.x - 360f : currentRotationEuler.x;
        
        clampedX = Mathf.Clamp(clampedX, -62f, 62f);
        
        
        var newRotationEuler = new Vector3(clampedX, currentRotationEuler.y, 0f);
        followTarget.transform.SetPositionAndRotation( followTarget.position,Quaternion.Euler(newRotationEuler));

        Transform playerTransform = playerCamera.transform;
        aimTransform.rotation = playerTransform.rotation;
        aimTransform.position = playerTransform.forward * 10000f;
        
        moveOrientation.rotation = Quaternion.Euler(0,newRotationEuler.y,0);
        
        
    }


    private void CheckCameraCollision()
    {
        Vector3 cameraTraceStartPosition = followTarget.position;
        
        
       if (Physics.Raycast(cameraTraceStartPosition,playerCamera.transform.forward * -1f,out RaycastHit hitInfo, maxCameraDistance ,cameraLayer))
       {
           desiredCameraPosition = playerCamera.transform.InverseTransformPoint(hitInfo.point);
       }
       else
       {
           desiredCameraPosition = baseCameraPosition;
       }
    }

}
