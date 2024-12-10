using UnityEngine;

public interface IAbilityInterface
{
    
    public bool IsAbilityActive { get; }
    public Transform SlashTransform { get; }
    public Transform CrossbowSocket { get; }
    public Transform AimTransform { get; }
    public bool IsAiming { get; }
    public GameObject Owner { get; }
    
    
    
}
