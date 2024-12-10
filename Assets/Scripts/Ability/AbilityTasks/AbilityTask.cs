using GameStates;
using UnityEngine;

public class AbilityTask : ScriptableObject
{
    protected InterfaceContainer interfaceContainer;
    protected AbilityData abilityData;
    
   public bool Activate(AbilityData inAbilityData, InterfaceContainer inInterfaceContainer)
   {
       interfaceContainer = inInterfaceContainer;
       abilityData = inAbilityData;
       
       if (!interfaceContainer.IsValid || interfaceContainer.CombatInterface.IsDead) return false;
       if (interfaceContainer.CharacterStateInterface.CurrentActionState == ActionStates.AimActive &&
           inAbilityData.AbilityName != "Shoot") return false;
     
       
       return HandleAbility();
   }

   
   protected virtual bool HandleAbility()
   {
       //TODO: spawn Transform for vfx should be set different
       interfaceContainer.VfxInterface.SpawnVFX(abilityData.AbilityVFXData, abilityData.AbilityTransform);
       interfaceContainer.PlayableInterface.PlayClip(abilityData.AbilityAnimationData, abilityData.AbilitySoundData);
       return true;
   }
}
