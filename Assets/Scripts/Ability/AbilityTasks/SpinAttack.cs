
using UnityEngine;

[CreateAssetMenu(fileName = "SpinAttack Task", menuName = "CastleAdventure/Ability/SpinAttackTask")]
public class SpinAttack : AbilityTask
{
    [SerializeField] private int StaminaCostAttack = 40;
    [SerializeField] private int damage = 40;
    [SerializeField] private int intervalNumber = 4;
    [SerializeField] private float damageRadius = 2f;
    [SerializeField] private float attackDistance = 2f;
    
    protected override bool HandleAbility()
    {
        if (interfaceContainer.CombatInterface.CharacterAttributeData.StaminaPoints <= StaminaCostAttack) return false;
        interfaceContainer.CombatInterface.LossStamina(StaminaCostAttack);
        abilityData.AbilityTransform = interfaceContainer.AbilityInterface.SlashTransform;
        
        interfaceContainer.CombatInterface.DealDamageAroundCaster(damage, intervalNumber, abilityData.AbilityAnimationData.AnimClip.length, damageRadius);
        
        interfaceContainer.CombatInterface.StaggerCount = 3;
        interfaceContainer.PlayableInterface.OnAnimationFinished += ResetStaggerCount;
        interfaceContainer.PlayableInterface.OnAnimationFinished += interfaceContainer.MovementInterface.OnMoveAbilityFinished;
        
        Vector3 abilityVelocity = interfaceContainer.MovementInterface.CharacterOrientation.forward * attackDistance;
        interfaceContainer.MovementInterface.HandleMoveAbility(abilityVelocity);
        
        base.HandleAbility();
        return true;
    }

    private void ResetStaggerCount()
    {
        interfaceContainer.PlayableInterface.OnAnimationFinished -= ResetStaggerCount;
        interfaceContainer.CombatInterface.StaggerCount = 1;
    }
}
