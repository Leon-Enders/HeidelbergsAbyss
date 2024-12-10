using UnityEngine;

[CreateAssetMenu(fileName = "Attack Task", menuName = "CastleAdventure/Ability/AttackTask")]
public class AttackTask : AbilityTask
{

    [SerializeField] private int StaminaCostAttack = 15;

    protected override bool HandleAbility()
    {
        if (interfaceContainer.CombatInterface.CharacterAttributeData.StaminaPoints <= StaminaCostAttack) return false;
        interfaceContainer.CombatInterface.LossStamina(StaminaCostAttack);
        abilityData.AbilityTransform = interfaceContainer.AbilityInterface.SlashTransform;
        interfaceContainer.PlayableInterface.OnAnimationEvent += interfaceContainer.CombatInterface.DealDamageInRadius;
        
        base.HandleAbility();
        return true;
    }
}
