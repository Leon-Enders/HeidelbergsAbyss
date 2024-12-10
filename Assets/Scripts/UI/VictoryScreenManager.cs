using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private float timeToShowVictoryScreen = 4f;
    [SerializeField] private GameObject boss;
    private ICombatInterface combatInterface;

    private void Start()
    {
        if(boss.TryGetComponent(out combatInterface))
        {
            combatInterface.OnDeath += HandleDeath;
        }
    }
    private void HandleDeath()
    {
        combatInterface.OnDeath -= HandleDeath;
        StartCoroutine(showVictoryScreen());
    }
    	
    public void TransitionToTitlescreen()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator showVictoryScreen()
    {
        yield return new WaitForSeconds(timeToShowVictoryScreen);
        print("activated");
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
