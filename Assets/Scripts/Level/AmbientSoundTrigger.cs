using UnityEngine;

public class AmbientSoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource ambientSound;
    
    private AmbientSoundManager ambientSoundManager;

    private void Awake()
    {
        ambientSoundManager = GetComponentInParent<AmbientSoundManager>();
        //To load sound in memory
        ambientSound.Play();
        ambientSound.Stop();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (ambientSound.isPlaying) return;
        
        ambientSoundManager.ActivateAmbientSound(ambientSound);
    }
}
