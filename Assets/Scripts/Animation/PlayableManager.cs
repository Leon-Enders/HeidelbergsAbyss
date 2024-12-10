using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayableManager : MonoBehaviour, IPlayableInterface
{

    public event IPlayableInterface.AnimationEventDelegate OnAnimationFinished;
    public event IPlayableInterface.AnimationEventDelegate OnAnimationEvent;
    public event IPlayableInterface.AnimationEventDelegate OnAnimationCanceled;
    
    public bool IsAnimActive { get; private set; } 
    

    [Header("Playable Properties")]
    [SerializeField] private float fadeInTime = 0.1f;
    [SerializeField] private float fadeOutTime = 0.1f;
    
    
    [Header("References")]
    [SerializeField] private Animator animator;

    private bool isFrozen = false;
    private PlayableGraph playableGraph;
    private AnimationClipPlayable clipPlayable;
    private AnimationMixerPlayable animationMixer;
    private SoundManager soundManager;


    private void Awake()
    {
        TryGetComponent(out soundManager);
    }


    private void Start()
    {
        playableGraph = PlayableGraph.Create();
        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "AnimationOutput", animator);
        animationMixer = AnimationMixerPlayable.Create(playableGraph, 1);
        playableOutput.SetSourcePlayable(animationMixer);
        
        var animatorControllerPlayable = AnimatorControllerPlayable.Create(playableGraph, animator.runtimeAnimatorController);
        animationMixer.ConnectInput(0,animatorControllerPlayable,0);
        
       
        
        playableGraph.Play();
    }

    private void Update()
    {
        if (isFrozen) return;
        if (animationMixer.GetInputCount() == 2)
        {
            var time = clipPlayable.GetTime();
            var normalizedTime = time / clipPlayable.GetDuration();
            var weight = 1d;

            if (normalizedTime < fadeInTime)
            {
                weight = normalizedTime / fadeInTime;
            }
            else if (normalizedTime > (1 - fadeOutTime))
            {
                weight = (1 - normalizedTime) / fadeOutTime;
            }

            animationMixer.SetInputWeight(0, 1 - (float)weight);
            animationMixer.SetInputWeight(1, (float)weight);

            if (clipPlayable.IsDone())
            {
                animationMixer.DisconnectInput(1);
                animationMixer.SetInputCount(1);
                clipPlayable.Destroy();
                IsAnimActive = false;
                OnAnimationFinished?.Invoke();
            }
        }
        else
        {
            animationMixer.SetInputWeight(0, 1);
        }
    }
    
    public void PlayClip(AnimationData animationData, SoundData soundData = null)
    {
        if (!animationData.IsValid)
        {
            print("animationData not valid");
            return;
        }
        
        if (clipPlayable.IsValid())
        {
            clipPlayable.Destroy();
        }

        if (soundData != null)
        {
            soundManager.PlaySound(soundData);
        }

        if (IsAnimActive)
        {
            OnAnimationCanceled?.Invoke();
        }
        
        IsAnimActive = true;
        clipPlayable = AnimationClipPlayable.Create(playableGraph, animationData.AnimClip);
        clipPlayable.SetDuration(animationData.AnimClip.length);
        clipPlayable.SetSpeed(animationData.PlaySpeed);
        animationMixer.SetInputCount(2);
        animationMixer.DisconnectInput(1);
        animationMixer.ConnectInput(1, clipPlayable, 0);

        if (animationData.AnimationEventTime > 0f)
        {
            //Start Timer for animation event
            StartCoroutine(BroadCastAnimationEvent(animationData.AnimationEventTime));
        }
    }
    
    private void OnDestroy()
    {
        if (playableGraph.IsValid())
        {
            playableGraph.Destroy();
        }
    }

    private IEnumerator BroadCastAnimationEvent(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        OnAnimationEvent?.Invoke();
    }

    public void Freeze(bool shouldFreeze)
    {
        isFrozen = shouldFreeze;
    }
}
