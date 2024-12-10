using System.Collections.Generic;
using GameStates;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private Transform meshTransform;
    [SerializeField] private Transform moveOrientation;
    [SerializeField] private Animator animator;
    [SerializeField] private float interpolationTime;
    
    
    private int isRunningHash;
    private int isSprinting;
    private int isJumpingHash;
    private int isInAirHash;
    private int isAimingHash;
    
    
    private Vector3 desiredLocalRotation = Vector3.zero;
    private DirectionStates oldDirectionState = DirectionStates.None;
    
    private Dictionary<DirectionStates, Vector3> DirectionStatesToEulerAngles = new Dictionary<DirectionStates, Vector3>()
    {
        { DirectionStates.Forward, new Vector3(0f,0f,0f) },
        { DirectionStates.ForwardRight, new Vector3(0f,45f,0f) },
        { DirectionStates.Right, new Vector3(0f,90f,0f) },
        { DirectionStates.BackwardRight, new Vector3(0f,135f,0f) },
        { DirectionStates.Backward, new Vector3(0f,180f,0f) },
        { DirectionStates.BackwardLeft, new Vector3(0f,-135f,0f)},
        { DirectionStates.Left, new Vector3(0f,-90f,0f) },
        { DirectionStates.ForwardLeft, new Vector3(0f,-45f,0f) }
    };
    
    
    private ICharacterStateInterface characterStateInterface;
    private IMovementInterface movementInterface;

    private void Awake()
    {
        TryGetComponent(out characterStateInterface);
        TryGetComponent(out movementInterface);
        
        isRunningHash = Animator.StringToHash("IsRunning");
        isSprinting = Animator.StringToHash("IsSprinting");
        isJumpingHash = Animator.StringToHash("IsJumping");
        isInAirHash = Animator.StringToHash("IsInAir");
        isAimingHash = Animator.StringToHash("IsAiming");
    }

    private void Update()
    {
        UpdateAnimationStates();
    }

    private void LateUpdate()
    {
        UpdateMeshRotation();
    }

    private void UpdateAnimationStates()
    {
        animator.SetBool(isRunningHash, (characterStateInterface.CurrentMovementState == MovementStates.Running));
        animator.SetBool(isSprinting, (characterStateInterface.CurrentMovementState == MovementStates.Sprinting));
        animator.SetBool(isJumpingHash,(movementInterface.IsJumping));
        animator.SetBool(isInAirHash,(characterStateInterface.CurrentMovementState == MovementStates.InAir));
        animator.SetBool(isAimingHash, (characterStateInterface.CurrentActionState == ActionStates.AimActive));
    }
    
    
    private void UpdateMeshRotation()
    {
        if (characterStateInterface.CurrentActionState == ActionStates.AimActive)
        {
            meshTransform.rotation = moveOrientation.rotation;
            
            return;
        }
        
        if (characterStateInterface.CurrentDirectionState == DirectionStates.None || characterStateInterface.CurrentActionState != ActionStates.None && characterStateInterface.CurrentActionState != ActionStates.Blocked)
        {
            return;
        }
        

        if (oldDirectionState != characterStateInterface.CurrentDirectionState)
        {
            oldDirectionState = characterStateInterface.CurrentDirectionState;
            desiredLocalRotation = DirectionStatesToEulerAngles[oldDirectionState];
        }

       
        Quaternion desiredRotation = Quaternion.Euler(desiredLocalRotation + moveOrientation.rotation.eulerAngles);
        Quaternion interpolatedRotation = Quaternion.Lerp(meshTransform.rotation, desiredRotation, interpolationTime * Time.deltaTime);
        meshTransform.rotation = interpolatedRotation;
    }
}
