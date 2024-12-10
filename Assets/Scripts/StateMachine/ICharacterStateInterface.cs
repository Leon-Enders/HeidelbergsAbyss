using GameStates;
using UnityEngine;

public interface ICharacterStateInterface
{
    public DirectionStates CurrentDirectionState { get; }
    public MovementStates CurrentMovementState { get; }
    public ActionStates CurrentActionState { get; }
}
