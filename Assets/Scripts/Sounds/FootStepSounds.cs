using UnityEngine;

public class FootStepSounds : MonoBehaviour
{
    
    [SerializeField] private SoundData footStepSound;
    [SerializeField] private AudioSource footStepSource;
    
    private IAbilityInterface abilityInterface;
    private void Awake()
    {
        TryGetComponent(out abilityInterface);
            
        footStepSource.volume = footStepSound.Volume;
        footStepSource.pitch = footStepSound.Pitch;
        footStepSource.spatialBlend = 1;

    }
    
    public void PlayFootstep()
    {
        if (abilityInterface.IsAbilityActive) return;
        
        footStepSource.PlayOneShot(footStepSound.SoundClip);
    }
    
    
}
