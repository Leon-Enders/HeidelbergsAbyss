
using UnityEngine;

[CreateAssetMenu(fileName = "Ability Data", menuName = "CastleAdventure/Ability/AbilityData")]
public class AbilityData : ScriptableObject
{
    public string AbilityName => abilityName;
    public bool IsGrantedOnStartUp => isGrantedOnStartUp;
    public AbilityTask OAbilityTask => oAbilityTask;
    public AnimationData AbilityAnimationData => abilityAnimationData;
    public VFXData AbilityVFXData => abilityVfxData;
    public SoundData AbilitySoundData => abilitySoundData;
    
    public Transform AbilityTransform
    {
        get => abilityTransform;
        set => abilityTransform = value;
    }
    
    
    [Header("Ability Properties")]
    [SerializeField] private string abilityName;
    [SerializeField] private bool isGrantedOnStartUp;
    [SerializeField] private AbilityTask oAbilityTask;
    [SerializeField] private AnimationData abilityAnimationData;
    [SerializeField] private VFXData abilityVfxData;
    [SerializeField] private SoundData abilitySoundData;
    
    
    private Transform abilityTransform;
    
    public AbilityData CreateCopy()
    {
        AbilityData copy = CreateInstance<AbilityData>();
        
        copy.abilityName = this.abilityName;
        copy.oAbilityTask = this.oAbilityTask;
        copy.abilityAnimationData = this.abilityAnimationData;
        copy.abilityVfxData = this.abilityVfxData;
        copy.abilitySoundData = this.abilitySoundData;
       

        return copy;
    }
}
