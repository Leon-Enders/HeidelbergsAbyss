using UnityEngine;

[CreateAssetMenu(fileName = "Animation Data", menuName = "CastleAdventure/AnimationData")]
public class AnimationData : ScriptableObject
{
    public float PlaySpeed => playSpeed;
    public float AnimationEventTime => animationEventTime;
    public AnimationClip AnimClip => animClip;
    
    
    
    [SerializeField] private float playSpeed = 1f;
    [SerializeField] private float animationEventTime = 0f;
    [SerializeField] private AnimationClip animClip;
    
    public bool IsValid => animClip != null;
}
