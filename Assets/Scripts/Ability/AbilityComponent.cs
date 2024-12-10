using System.Collections;
using System.Collections.Generic;
using GameStates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class AbilityComponent : MonoBehaviour, IAbilityInterface
{
    public bool IsAbilityActive { get; private set; }
    public Transform SlashTransform { get; private set; }
    public Transform CrossbowSocket { get; private set;}
    public Transform AimTransform { get; private set;}
    public bool IsAiming { get; private set;}
    public GameObject Owner { get; private set; }

    [Header("Debug")] 
    [SerializeField] private bool debugModeOn = false;
    
    [Header("References")] 
    [SerializeField] private List<AbilityData> abilityDataList;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private Transform aimTransform;
    [SerializeField] private Transform slashTransform;
    [SerializeField] private Transform crossbowSocket;
    [SerializeField] private GameObject swordObject;
    [SerializeField] private GameObject crossbowObject;
    [SerializeField] private GameObject crosshair;
    


    private bool cachedIsAiming = false;
    private Dictionary<string, AbilityData> abilityDataForName = new Dictionary<string, AbilityData>();
    private Dictionary<string, bool> AbilityGrantedMap = new Dictionary<string, bool>();
    private CastleAdventureInputActions controls;

    private InterfaceContainer interfaceContainer;
    
    private void Awake()
    {
        InitializeProperties();
        InitializeAbilityInformation();
        if (debugModeOn)
        {
            swordObject.SetActive(true);
        }
    }
    
    private void ActivateAbility(InputAction.CallbackContext context)
    {
        var abilityName = context.action.name;
        
        if (!AbilityGrantedMap[abilityName]) return;
        if (!abilityDataForName.TryGetValue(abilityName, out var abilityData)) return;
        
        if (interfaceContainer.CharacterStateInterface.CurrentActionState is ActionStates.None or ActionStates.AimActive)
        {
                
            IsAbilityActive = abilityData.OAbilityTask.Activate(abilityData, interfaceContainer);
        }
    }
    
    private void ActivateCrossbowCamera(bool isActive)
    {
        if (!AbilityGrantedMap["Shoot"]) return;
        if (isActive)
        {
            ActivateAim();
        }
        else
        {
            Invoke(nameof(DeactivateAim), 0.2f);
        }
    }

    private void ActivateAim()
    {
        cachedIsAiming = true;
        if (interfaceContainer.CharacterStateInterface.CurrentActionState == ActionStates.None ||
            (interfaceContainer.CharacterStateInterface.CurrentActionState == ActionStates.AimActive 
             && !interfaceContainer.PlayableInterface.IsAnimActive))
        {
            // Handle Weapon Switch
            swordObject.SetActive(false);
            crossbowObject.SetActive(true);
            
            crosshair.SetActive(true);
            rigBuilder.enabled = true;
            IsAiming = true;
        }
    }

    private void DeactivateAim()
    {
        cachedIsAiming = false;
        if (interfaceContainer.CharacterStateInterface.CurrentActionState == ActionStates.None ||
            (interfaceContainer.CharacterStateInterface.CurrentActionState == ActionStates.AimActive 
             && !interfaceContainer.PlayableInterface.IsAnimActive))
        {
            // Handle Weapon Switch
            swordObject.SetActive(true);
            crossbowObject.SetActive(false);
            
            crosshair.SetActive(false);
            rigBuilder.enabled = false;
            IsAiming = false;
        }
    }
    
    private void InitializeProperties()
    {
        Owner = gameObject;
        SlashTransform = slashTransform;
        CrossbowSocket = crossbowSocket;
        AimTransform = aimTransform;

        if (TryGetComponent(out ICharacterStateInterface characterStateInterface) &&
            TryGetComponent(out IPlayableInterface playableInterface) &&
            TryGetComponent(out ICombatInterface combatInterface) &&
            TryGetComponent(out IMovementInterface movementInterface) &&
            TryGetComponent(out IItemInterface itemInterface) &&
            TryGetComponent(out IVFXInterface vfxInterface))
        {
            interfaceContainer = new InterfaceContainer(this, characterStateInterface, playableInterface,combatInterface,movementInterface,itemInterface,vfxInterface);
        }
        
        
        interfaceContainer.PlayableInterface.OnAnimationFinished += OnAbilityFinished;
        interfaceContainer.ItemInterface.OnGrantItemAbility += GrantAbility;
    }

    private void InitializeAbilityInformation()
    {
        controls = new CastleAdventureInputActions();

        controls.PlayerControls.Aim.performed += ctx => ActivateCrossbowCamera(true);
        controls.PlayerControls.Aim.canceled += ctx => ActivateCrossbowCamera(false);
        
        
        foreach (var inputAction in controls.Abilities.Get())
        {
            inputAction.performed += ActivateAbility;
            
            foreach (var abilityData in abilityDataList)
            {
                if (inputAction.name == abilityData.AbilityName)
                {
                    abilityDataForName.Add(inputAction.name, abilityData.CreateCopy());
                    if (debugModeOn)
                    {
                        AbilityGrantedMap.Add(inputAction.name, true);
                    }
                    else
                    {
                        AbilityGrantedMap.Add(inputAction.name, abilityData.IsGrantedOnStartUp);
                    }
                  
                }
            }
        }
        
        controls.Enable();
    }

    private void OnAbilityFinished()
    {
        IsAbilityActive = false;
        if(!AbilityGrantedMap["Shoot"])return;
        
        IsAiming = cachedIsAiming;
        rigBuilder.enabled = cachedIsAiming;
        swordObject.SetActive(!cachedIsAiming);
        crossbowObject.SetActive(cachedIsAiming);
        crosshair.SetActive(cachedIsAiming);
    }  
    
    private void GrantAbility(List<string> abilitiesToUnlock)
    {
        foreach (var abilityName in abilitiesToUnlock)
        {
            AbilityGrantedMap[abilityName] = true;
            
            if (abilityName == "Attack")
            {
                swordObject.SetActive(true);
            }
        }
    }
    
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

   

    private void OnDestroy()
    {
        interfaceContainer.PlayableInterface.OnAnimationFinished -= OnAbilityFinished;
    }
}
