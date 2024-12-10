
using UnityEngine;

public interface IPlayableInterface
{
    public bool IsAnimActive { get; }
    public void PlayClip(AnimationData animationData, SoundData soundData = null);

    public void Freeze(bool shouldFreeze);
    
    public delegate void AnimationEventDelegate();

    event AnimationEventDelegate OnAnimationFinished;
    event AnimationEventDelegate OnAnimationEvent;
    event AnimationEventDelegate OnAnimationCanceled;
}
