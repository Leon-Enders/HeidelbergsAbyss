using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource soundEffectSource;
    
    
    private void Awake()
    {
       
        soundEffectSource.spatialBlend = 0.7f;
    }

    public void PlaySound(SoundData soundData)
    {
        soundEffectSource.volume = soundData.Volume;
        soundEffectSource.pitch = soundData.Pitch;

        if (soundData.SoundDelay == 0)
        {
            soundEffectSource.PlayOneShot(soundData.SoundClip);
            return;
        }
        
        StartCoroutine( PlaySoundWithDelay(soundData.SoundClip, soundData.SoundDelay));
    }

    IEnumerator PlaySoundWithDelay(AudioClip audioClip, float delay)
    {
        yield return new WaitForSeconds(delay);
        soundEffectSource.PlayOneShot(audioClip);
    }
}
