using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> ambientSounds;

    public void ActivateAmbientSound(AudioSource inAmbientSound)
    {
        foreach (var ambientSound in ambientSounds)
        {
            ambientSound.Stop();
        }
        inAmbientSound.Play();
    }
}
