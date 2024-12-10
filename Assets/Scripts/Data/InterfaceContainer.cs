
public class InterfaceContainer
{
    public IAbilityInterface AbilityInterface => abilityInterface;
    public ICharacterStateInterface CharacterStateInterface => characterStateInterface;
    public IPlayableInterface PlayableInterface => playableInterface;
    public ICombatInterface CombatInterface => combatInterface;
    public IMovementInterface MovementInterface => movementInterface;
    public IItemInterface ItemInterface => itemInterface;
    public IVFXInterface VfxInterface => vfxInterface;
    
    
    public InterfaceContainer(IAbilityInterface abilityInterface,ICharacterStateInterface characterStateInterface, IPlayableInterface playableInterface,
        ICombatInterface combatInterface, IMovementInterface movementInterface,IItemInterface itemInterface, IVFXInterface vfxInterface)
    {
        this.abilityInterface = abilityInterface;
        this.characterStateInterface = characterStateInterface;
        this.playableInterface = playableInterface;
        this.combatInterface = combatInterface;
        this.movementInterface = movementInterface;
        this.itemInterface = itemInterface;
        this.vfxInterface = vfxInterface;
    }

    public bool IsValid =>
        abilityInterface != null &&
        characterStateInterface != null &&
        playableInterface != null &&
        combatInterface != null &&
        movementInterface != null &&
        itemInterface != null &&
        vfxInterface != null;
    
    
    private readonly IAbilityInterface abilityInterface;
    private readonly ICharacterStateInterface characterStateInterface;
    private readonly IPlayableInterface playableInterface;
    private readonly ICombatInterface combatInterface;
    private readonly IMovementInterface movementInterface;
    private readonly IItemInterface itemInterface;
    private readonly IVFXInterface vfxInterface;
}
