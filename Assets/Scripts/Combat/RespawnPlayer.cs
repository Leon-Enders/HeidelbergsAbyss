using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject playerCharacter;
    [SerializeField] private CheckPointManager checkPointManager;
    
    private bool isDead = false;
    
    private IPlayableInterface playableInterface;
    private ICombatInterface combatInterface;
    
    private void Awake()
    {
        playerCharacter.TryGetComponent(out playableInterface);
        playerCharacter.TryGetComponent(out combatInterface);

        combatInterface.OnRespawn += Respawn;
        combatInterface.OnDeath += HandleDeath;
    }

    private void HandleDeath()
    {
        isDead = true;
        playableInterface.OnAnimationEvent += DeathFinished;   
    }

    private void DeathFinished()
    {
        Invoke(nameof(DeathFinishedInvoked), 1f);
    }

    private void DeathFinishedInvoked()
    {   
        // Refresh scenes for checkpoint
        if (checkPointManager.CurrentCheckPoint.SceneIndexToReload != 0)
        {
            SceneManager.UnloadSceneAsync(checkPointManager.CurrentCheckPoint.SceneIndexToReload);
            SceneManager.LoadSceneAsync(checkPointManager.CurrentCheckPoint.SceneIndexToReload, LoadSceneMode.Additive);
        }
       
        playableInterface.OnAnimationEvent -= DeathFinished;
        deathScreen.SetActive(true);
        Invoke(nameof(InvokedMouseUnlock), 1f);
    }

    private void InvokedMouseUnlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RespawnButton()
    {
        if(isDead)
        {
            combatInterface.TriggerRespawn();
        }
    }

    private void Respawn()
    {
        playerCharacter.transform.position = checkPointManager.CurrentCheckPoint.transform.position;
       
        isDead = false;
        deathScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
