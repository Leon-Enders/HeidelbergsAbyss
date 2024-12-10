using UnityEngine;
public class FadeOutOnActivate : MonoBehaviour
{

    [SerializeField] private AnimationData wakeUp;
    [SerializeField] private GameObject playerCharacter;
    
    
    private IMovementInterface movementInterface;
    private IPlayableInterface playableInterface;
    private void Awake()
    {
        playerCharacter.TryGetComponent(out movementInterface);
        playerCharacter.TryGetComponent(out playableInterface);
       
        Invoke("InvokeWakeUp", 2.5f);
        Invoke("InvokedSetInactive", 1.5f);
    }

    void InvokedSetInactive()
    {
        gameObject.SetActive(true);
    }

    private void InvokeWakeUp()
    {
        if (movementInterface == null || playableInterface == null) return;
       
        playableInterface.OnAnimationEvent += movementInterface.ActivateMovement;
        playableInterface.PlayClip(wakeUp);
    }
}
