using UnityEngine;

[CreateAssetMenu(fileName = "PickUp Task", menuName = "CastleAdventure/Ability/PickUp Task")]
public class PickUpTask : AbilityTask
{
    protected override bool HandleAbility()
    {
        if (!interfaceContainer.ItemInterface.IsInteractionReady) return false;
        
        interfaceContainer.PlayableInterface.OnAnimationFinished +=  interfaceContainer.ItemInterface.OnInteractionFinished;
        
        base.HandleAbility();
        return true;
    }
}
