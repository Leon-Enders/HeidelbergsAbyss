using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject itemOverlay;
    [SerializeField] private GameObject statsBar;
    [SerializeField] private GameObject infoItemUI;
    [SerializeField] private GameObject player;
    
    [Header("Menu")]
    [SerializeField] private GameObject controlMenu;
    [SerializeField] private GameObject soundSettings;
    [SerializeField] private GameObject menuButtons;
    

    private SoundSettings soundSettingsScript;
    private CastleAdventureInputActions controls;
    private IMovementInterface movementInterface;
    private void Awake()
    {
        controls = new CastleAdventureInputActions();
        controls.PlayerControls.Menu.performed += ctx => OpenAndCloseOptions();
        controls.Enable();

        if (soundSettings.TryGetComponent(out soundSettingsScript))
        {
            soundSettingsScript.InitializeSoundSettings();
        }

        player.TryGetComponent(out movementInterface);
    }

    private void OnDisable() 
    {
        controls.Disable();
    }

    private void OnEnable() 
    {
        controls.Enable();
    }

    public void OpenAndCloseOptions()
    {
        if (infoItemUI.GetComponent<Canvas>().enabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            infoItemUI.GetComponent<Canvas>().enabled = false;
            statsBar.SetActive(true);
            itemOverlay.SetActive(true);
            movementInterface.ActivateMovement();
            return;
        }
        
        
        if (optionsMenu.activeInHierarchy)
        {
            if (statsBar != null && itemOverlay != null)
            {
                QuitSoundSettings();
                optionsMenu.SetActive(false);
                statsBar.SetActive(true);
                itemOverlay.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                movementInterface.ActivateMovement();
            }
        }
        else
        {
            if (statsBar != null && itemOverlay != null)
            {
                optionsMenu.SetActive(true);
                statsBar.SetActive(false);
                itemOverlay.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                movementInterface.DisableMovement();
            }
        }
    }

    public void EnableControlMenu()
    {
        menuButtons.SetActive(false);
        soundSettings.SetActive(false);
        controlMenu.SetActive(true);
    }

    public void EnableSoundSettings()
    {
        menuButtons.SetActive(false);
        soundSettings.SetActive(true);
        controlMenu.SetActive(false);
    }

    public void QuitSoundSettings()
    {
        menuButtons.SetActive(true);
        soundSettings.SetActive(false);
        controlMenu.SetActive(false);
    }
}
