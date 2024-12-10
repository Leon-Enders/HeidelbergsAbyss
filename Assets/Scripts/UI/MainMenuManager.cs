using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject blendInPanel;
    
    
    [Header("Menus")]
    [SerializeField] private GameObject titelScreen;
    [SerializeField] private GameObject optionsMenu;
    
    [Header("OptionsMenu")]
    [SerializeField] private GameObject menuButtons;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject soundMenu;
    
    
    public void ExitGame() 
    { 
        print("Exit Game");
        Application.Quit();
    }

    public void PlayGame() 
    {
        blendInPanel.SetActive(true);
        Invoke("InvokedStartGame", 1f);
    }

    public void OpenOptionsMenu()
    {
        titelScreen.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void BackOptionsMenu()
    {
        titelScreen.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void BackOptionsSetting()
    {
        menuButtons.SetActive(true);
        controlsMenu.SetActive(false);
        soundMenu.SetActive(false);
    }

    public void OpenSoundSettings()
    {
        menuButtons.SetActive(false);
        controlsMenu.SetActive(false);
        soundMenu.SetActive(true);
    }

    public void OpenControls()
    {
        menuButtons.SetActive(false);
        controlsMenu.SetActive(true);
        soundMenu.SetActive(false);
    }
    
    private void InvokedStartGame()
    {
        SceneManager.LoadScene(1);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
        SceneManager.LoadScene(4, LoadSceneMode.Additive);
    }
    
}
