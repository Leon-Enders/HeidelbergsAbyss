using GameStates;
using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour, IMovementInterface
{
    //Getters
    public Transform CharacterOrientation => characterOrientation;
    public Vector2 DirectionalInput => directionalInput;
    public Vector2 GroundVelocity => new(groundVelocity.x, groundVelocity.z);
    public bool GetIsActiveAndEnabled => isActiveAndEnabled;
    public bool MoveAbilityActive{ get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsSliding { get; private set; }
    public bool IsJumping { get; private set; }
    
    [Header("Movement Properties")]
    [SerializeField]private float gravityConstant = 15f;
    [SerializeField] private float friction = 0.05f;
    [SerializeField] private float airFriction = 0.0005f;
    [SerializeField] private float airControl = 0.05f;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float maxRunSpeed = 7f;
    [SerializeField] private float maxSprintSpeed = 14f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] private float groundCheckOffset = 0.3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float slideSpeed = 5f;
    [SerializeField] private int StaminaCostSprint = 5;
    [SerializeField] private float slopeTimeThreshold = 0.1f;
    [SerializeField] private float minGravity = -2f;
    [SerializeField] private float maxGravity = -18f;
    
    
    
    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform moveOrientation;
    [SerializeField] private Transform characterOrientation;


    private Vector3 groundVelocity;
    private Vector2 directionalInput;
    private Vector3 abilityVelocity;
    private Vector3 groundNormal;
    private Vector3 lastVelocity;
    private float currentMaxSpeed;
    
    private float gravity;
    private float slopeTimer = 0f;
    
    
    
    private bool isSprinting = false;

    private CastleAdventureInputActions controls;
    private IPlayableInterface playableInterface;
    private ICharacterStateInterface characterStateInterface;
    private ICombatInterface combatInterface;

    
    private Coroutine sprintCostRoutine;
    private Coroutine staminaRegenRoutine;
    private void Awake()
    {
        TryGetComponent(out playableInterface);
        TryGetComponent(out characterStateInterface);
        TryGetComponent(out combatInterface);
        
        combatInterface.OnDeath += HandleDeath;
        combatInterface.OnRespawn += HandleRespawn;

        controls = new CastleAdventureInputActions();
        controls.PlayerControls.Move.performed += ctx => UpdateDirection(ctx.ReadValue<Vector2>());
        controls.PlayerControls.Move.canceled += ctx => UpdateDirection(Vector2.zero);
        controls.PlayerControls.Jump.performed += ctx => Jump();
        controls.PlayerControls.Sprint.performed += ctx => StartSprint();
        controls.PlayerControls.Sprint.canceled += ctx => StopSprint();
        controls.Enable();

        currentMaxSpeed = maxRunSpeed;

    }

    private void Update()
    {
        UpdateVelocity();
        MoveCharacter();
    }

    private void UpdateDirection(Vector2 movementDirection)
    {
        directionalInput.x = movementDirection.x * playerSpeed;
        directionalInput.y = movementDirection.y * playerSpeed;
    }

    private void CheckSlope()
    {
        float slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
         if (slopeAngle > controller.slopeLimit)
         {
             if (!IsSliding)
             {
                 slopeTimer += Time.fixedDeltaTime;
             }
             
             
             Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, groundNormal).normalized; 
             groundVelocity = slideDirection * slideSpeed;

             if (slopeTimer > slopeTimeThreshold)
             {
                 slopeTimer = 0f;
                 IsSliding = true;
             }
         }
         else
         {
             IsSliding = false;
         }
        
    }
    

    private void Jump()
    {
        if (IsGrounded && !IsSliding)
        {
            IsJumping = true;
            StartCoroutine(ResetJump());
            gravity = 0;
            gravity += Mathf.Sqrt(jumpHeight * 2 * gravityConstant);
        }
    }
    
    private void StartSprint()
    {
        if (combatInterface.CharacterAttributeData.StaminaPoints >= StaminaCostSprint && characterStateInterface.CurrentActionState == ActionStates.None)
        {
            sprintCostRoutine = StartCoroutine(ApplySprintCost());
            isSprinting = true;
            currentMaxSpeed = maxSprintSpeed;
        }
    }

    private void StopSprint()
    {
        if (sprintCostRoutine != null)
        {
            StopCoroutine(sprintCostRoutine);
        }

        isSprinting = false;
    }
    
    private void UpdateVelocity()
    {
        if (IsGrounded && gravity <0 && !IsSliding)
        {
            groundVelocity += moveOrientation.forward * directionalInput.y + moveOrientation.right * directionalInput.x;
            groundVelocity = Vector3.ClampMagnitude(groundVelocity, currentMaxSpeed);
            groundVelocity = Vector3.Lerp(groundVelocity, Vector3.zero, friction);
        }
        else if (IsSliding)
        {
            groundVelocity = Vector3.Lerp(groundVelocity, groundVelocity, slideSpeed * Time.deltaTime);
        }
        else
        {
            groundVelocity += moveOrientation.forward * (directionalInput.y * airControl) + moveOrientation.right * (directionalInput.x * airControl);
            groundVelocity = Vector3.ClampMagnitude(groundVelocity, currentMaxSpeed);
            groundVelocity = Vector3.Lerp(groundVelocity, Vector3.zero, airFriction);
        }
        
        gravity -= gravityConstant * Time.deltaTime;

        gravity = Mathf.Max(gravity, maxGravity);
        
    }

  

    private void MoveCharacter()
    {
        if (characterStateInterface.CurrentActionState == ActionStates.MoveAbilityActive)
        {
            gravity -= gravityConstant * Time.deltaTime;
            abilityVelocity.y = gravity;
            controller.Move(abilityVelocity * Time.deltaTime);
            return;
        }
        if (characterStateInterface.CurrentActionState != ActionStates.None && characterStateInterface.CurrentActionState != ActionStates.Blocked )
        {
            return;
        }
        Vector3 velocity = groundVelocity;
        velocity.y = gravity;
        controller.Move(velocity * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        GroundCheck();
        if (IsGrounded)
        {
            CheckSlope();
        }
    }

    private void GroundCheck()
    {
      
        Vector3 sphereOrigin = transform.position + Vector3.up * groundCheckOffset;

       
        IsGrounded = Physics.SphereCast(sphereOrigin, groundCheckRadius, Vector3.down, out RaycastHit hit, groundDistance, groundLayer);
        if (IsGrounded)
        {
            if (!IsJumping)
            {
                gravity = minGravity;
            }
            if (!isSprinting)
            {
                currentMaxSpeed = maxRunSpeed;
            }
            groundNormal = hit.normal;
        }
        else
        {
            groundNormal = Vector3.up;
        }
       
        //Debug.DrawRay(sphereOrigin, Vector3.down * groundDistance, Color.green);

    }

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
        
    }


    private void OnEnable()
    { 
        controls.Enable();
        if (staminaRegenRoutine == null)
        { 
            staminaRegenRoutine = StartCoroutine(AddStamina());
        }
    }
    
    private void OnDisable()
    {
        controls.Disable();
        if (staminaRegenRoutine != null)
        {
            StopCoroutine(staminaRegenRoutine);
            staminaRegenRoutine = null;
        }
        if(sprintCostRoutine != null)
        {
            StopCoroutine(sprintCostRoutine);
            sprintCostRoutine = null;
        }
    }

    private void HandleDeath()
    {
        enabled = false;
    }

    private void HandleRespawn()
    {
        Invoke("ActivateMovement", 0.5f);
    }

    IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.2f);
        IsJumping = false;
    }
    
    IEnumerator ApplySprintCost()
    {
        while (true)
        {
            if (combatInterface.CharacterAttributeData.StaminaPoints <= 0)
            {
                StopSprint();
            }
            combatInterface.LossStamina(StaminaCostSprint);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator AddStamina()
    {
        while (true)
        {
            if(combatInterface != null)
            {
                if(!MoveAbilityActive && !isSprinting)
                {
                    combatInterface.LossStamina(-5);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    public void ActivateMovement()
    {
        playableInterface.OnAnimationEvent -= ActivateMovement;
        enabled = true;
    }

    public void DisableMovement()
    {
        enabled = false;
    }
    public void HandleMoveAbility(Vector3 moveAbilityVelocity)
    {
        abilityVelocity = moveAbilityVelocity;
        abilityVelocity.y = gravity;
        MoveAbilityActive = true;
    }
    
    public void OnMoveAbilityFinished()
    {
        playableInterface.OnAnimationEvent -= OnMoveAbilityFinished;
        MoveAbilityActive = false;
        currentMaxSpeed = maxRunSpeed;
    }
}
