using UnityEngine;

[CreateAssetMenu(fileName = "Roll Task", menuName = "CastleAdventure/Ability/RollTask")]
public class RollTask : AbilityTask
{
    [SerializeField] private int StaminaCostRoll = 15;
    [SerializeField] private float rollImmunityDuration = 0.5f;
    [SerializeField] private float rollDistance = 3f;
    
    
    protected override bool HandleAbility()
    {
        if (interfaceContainer.CombatInterface.CharacterAttributeData.StaminaPoints <= StaminaCostRoll) return false;
        interfaceContainer.CombatInterface.LossStamina(StaminaCostRoll);
        interfaceContainer.CombatInterface.ActivateImmunity(rollImmunityDuration);
        
        interfaceContainer.PlayableInterface.OnAnimationFinished += interfaceContainer.MovementInterface.OnMoveAbilityFinished;

        Vector3 rollVelocity = interfaceContainer.MovementInterface.CharacterOrientation.forward * rollDistance;
                
        interfaceContainer.MovementInterface.HandleMoveAbility(rollVelocity);
        
        base.HandleAbility();
        return true;
    }
}
