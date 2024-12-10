using UnityEngine;

[CreateAssetMenu(fileName = "Heal Potion Task", menuName = "CastleAdventure/Ability/Heal Potion Task")]
public class HealPotionTask : AbilityTask
{
    [SerializeField] private int healValue;
    
    protected override bool HandleAbility()
    {
        //TODO: other transform generation
        abilityData.AbilityTransform = interfaceContainer.AbilityInterface.Owner.transform;
        interfaceContainer.CombatInterface.ReceiveHeal(healValue);
        
        base.HandleAbility();
        return true;
    }
}
