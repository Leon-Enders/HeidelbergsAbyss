using UnityEngine;

[CreateAssetMenu(fileName = "MagicShield Task", menuName = "CastleAdventure/Ability/MagicShield Task")]
public class MagicShieldTask : AbilityTask
{
    protected override bool HandleAbility()
    {
        //TODO: other transform generation
        abilityData.AbilityTransform = interfaceContainer.AbilityInterface.Owner.transform;
        interfaceContainer.CombatInterface.ActivateImmunity(abilityData.AbilityVFXData.LifeTime);
        
        base.HandleAbility();
        return true;
    }
}
