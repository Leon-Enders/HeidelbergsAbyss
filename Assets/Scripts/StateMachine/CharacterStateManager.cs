using GameStates;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStateManager : MonoBehaviour, ICharacterStateInterface
{
  
    // Getter
    public DirectionStates CurrentDirectionState => currentDirectionState;
    public MovementStates CurrentMovementState => currentMovementState;

    public ActionStates CurrentActionState => currentActionState;
    
    [Header("References")]
    [SerializeField] private Inventory inventory;
    
    private Vector2 lastMovementInput = Vector2.zero;
    private Vector2 characterGroundVelocity = Vector2.zero;
    
    private DirectionStates currentDirectionState = DirectionStates.None;
    private MovementStates currentMovementState = MovementStates.Idle;
    private ActionStates currentActionState = ActionStates.None;

    
    private ICombatInterface combatInterface;
    private IAbilityInterface abilityInterface;
    private IMovementInterface movementInterface;

    private void Awake()
    {
        TryGetComponent(out combatInterface);
        TryGetComponent(out abilityInterface);
        TryGetComponent(out movementInterface);
    }

    private void Update()
    {
        UpdateStates();
    }


    private void UpdateStates()
    {
        UpdateDirectionState();
        UpdateMovementState();
        UpdateActionState();
    }
    
    
    
    private void UpdateDirectionState()
    {
         if(!movementInterface.GetIsActiveAndEnabled)
        {
            currentDirectionState = DirectionStates.None;
            return;
        }

        lastMovementInput = movementInterface.DirectionalInput;
        
        if (lastMovementInput == Vector2.zero)
        {
            currentDirectionState = DirectionStates.None;
            return;
        }

        // Calculate a angle from the last Movement Input
        float angle = Mathf.Atan2(lastMovementInput.y, lastMovementInput.x) * Mathf.Rad2Deg;

        if (angle is >= -22.5f and < 22.5f)
        {
            currentDirectionState = DirectionStates.Right;
        }
        else if (angle is >= 22.5f and < 67.5f)
        {
            currentDirectionState = DirectionStates.ForwardRight;
        }
        else if (angle is >= 67.5f and < 112.5f)
        {
            currentDirectionState = DirectionStates.Forward;
        }
        else if (angle is >= 112.5f and < 157.5f)
        {
            currentDirectionState = DirectionStates.ForwardLeft;
        }
        else if (angle is >= -67.5f and < -22.5f)
        {
            currentDirectionState = DirectionStates.BackwardRight;
        }
        else if (angle is >= -112.5f and < -67.5f)
        {
            currentDirectionState = DirectionStates.Backward;
        }
        else if (angle is >= -157.5f and < -112.5f)
        {
            currentDirectionState = DirectionStates.BackwardLeft;
        }
        else
        {
            currentDirectionState = DirectionStates.Left;
        }
    }
    
    private void UpdateMovementState()
    {
        characterGroundVelocity = movementInterface.GroundVelocity;

       
       
        if (!movementInterface.IsGrounded || movementInterface.IsSliding)
        {
            currentMovementState = MovementStates.InAir;
            return;
        }
        if (characterGroundVelocity.sqrMagnitude > 50f)
        {
            currentMovementState = MovementStates.Sprinting;
            return;
        }
        if (characterGroundVelocity.sqrMagnitude > 5f)
        {
            currentMovementState = MovementStates.Running;
            return;
        }
        
        currentMovementState = MovementStates.Idle;
    }
    
    private void UpdateActionState()
    {
        if (combatInterface.IsHit)
        {
            currentActionState = ActionStates.HitActive;
            return;
        }
        
        if (abilityInterface.IsAiming)
        {
            currentActionState = ActionStates.AimActive;
            return;
        }
        
        if(movementInterface.MoveAbilityActive)
        {
            currentActionState = ActionStates.MoveAbilityActive;
            return;
        }
        
        if (abilityInterface.IsAbilityActive)
        {
            currentActionState = ActionStates.AbilityActive;
            return;
        }

        if (currentMovementState == MovementStates.InAir)
        {
            currentActionState = ActionStates.Blocked;
            return;
        }
        
        currentActionState = ActionStates.None;
    }
}
