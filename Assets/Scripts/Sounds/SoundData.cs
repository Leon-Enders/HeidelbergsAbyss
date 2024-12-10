using UnityEngine;


[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "CastleAdventure/Sounds/SoundEffect")]
public class SoundData : ScriptableObject
{

    public AudioClip SoundClip => soundClip;
    public float Volume => volume;
    public float Pitch => pitch;
    public float SoundDelay => soundDelay;
    
    [SerializeField] private AudioClip soundClip;         
    [SerializeField][Range(0f, 1f)] private float volume = 1f;   
    [SerializeField][Range(0.5f, 2f)] private float pitch = 1f;
    [SerializeField][Range(0f, 3f)] private float soundDelay = 0f;
}
