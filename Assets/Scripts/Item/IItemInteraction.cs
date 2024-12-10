using System.Collections.Generic;

public interface IItemInterface
{
    public delegate void InteractionEventHandler(bool bIsActivated);

    public delegate void GrantItemAbility(List<string> abilitiesToGrant);
    
    bool IsInteractionReady { get; }
    event InteractionEventHandler OnInteraction;
    event GrantItemAbility OnGrantItemAbility;
    
    public void ReceiveActionItemData(ActionItemData actionItemData);
    public void ReceiveInfoText(string infoHeader,string infoText);
    public void InteractionReady(bool canInteract);
    public void OnInteractionFinished();
    
    
}
