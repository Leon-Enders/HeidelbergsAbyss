using UnityEngine;

public interface IMovementInterface
{
    public Transform CharacterOrientation { get; }
    public Vector2 DirectionalInput { get; }
    public Vector2 GroundVelocity { get; } 
    public bool GetIsActiveAndEnabled { get; }
    public bool MoveAbilityActive { get; }
    public bool IsGrounded { get; }
    public bool IsSliding { get; }
    public bool IsJumping { get; }
    public void ActivateMovement();
    public void DisableMovement();
    public void HandleMoveAbility(Vector3 moveAbilityVelocity);
    public void OnMoveAbilityFinished();
}
