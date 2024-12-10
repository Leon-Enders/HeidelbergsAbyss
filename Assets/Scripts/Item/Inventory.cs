using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IItemInterface
{
    public bool IsInteractionReady { get; private set; }
    
    public event IItemInterface.InteractionEventHandler OnInteraction;
    public event IItemInterface.GrantItemAbility OnGrantItemAbility;
    
    
    [SerializeField] private GameObject itemBar;
    [SerializeField] private Canvas infoTextCanvas;
    [SerializeField] private TextMeshProUGUI infoItemText;
    [SerializeField] private GameObject itemOverlay;
    [SerializeField] private GameObject statsBarCanvas;
    [SerializeField] private List<Image> itemSlots = new List<Image>();
    
    private TextMeshProUGUI infoItemHeader;
    
    private Dictionary<int, Image> indexToItemSlot = new Dictionary<int, Image>();
    private CastleAdventureInputActions menuControls;

    private IPlayableInterface playableInterface;
    private IMovementInterface movementInterface;
    private void Awake()
    {
        menuControls = new CastleAdventureInputActions();
        menuControls.Enable();

        TryGetComponent(out playableInterface);
        TryGetComponent(out movementInterface);
        
        infoItemHeader = infoTextCanvas.transform.Find("InfoHeader").GetComponent<TextMeshProUGUI>();
        infoTextCanvas.enabled = false;
    }

    private void Start()
    {
        var index = 1;
        
        foreach (var itemSlot in itemSlots)
        {
            indexToItemSlot.Add(index, itemSlot);
            index++;
        }
    }


    public void ReceiveInfoText(string infoHeader,string infoText)
    {
        if (infoText != null)
        {
            infoItemHeader.text = infoHeader;
            infoItemText.text = infoText;
            infoTextCanvas.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            statsBarCanvas.SetActive(false);
            itemOverlay.SetActive(false);
        }
        else
        {
            infoTextCanvas.enabled = false;
        }
    }
    public void ReceiveActionItemData(ActionItemData itemData)
    {
        OnGrantItemAbility?.Invoke(itemData.AbilitiesToUnlock);
        SetItemSlotImage(itemData);
    }

    private void SetItemSlotImage(ActionItemData itemData)
    {
        indexToItemSlot[itemData.ItemSlot].sprite = TextureToSprite(itemData.ItemIcon);
    }
    
    private Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
    }

    public void InteractionReady(bool canInteract)
    {
        IsInteractionReady = canInteract;
    }
    
    public void OnInteractionFinished()
    {
        playableInterface.OnAnimationFinished -= OnInteractionFinished;
        OnInteraction?.Invoke(true);
        
        movementInterface.DisableMovement();
    }

}
