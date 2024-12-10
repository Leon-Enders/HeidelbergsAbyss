using System;
using TMPro;
using UnityEngine;

public class ItemInteractionManager : MonoBehaviour
{
    public Action OnItemDestroyed;
    
    [SerializeField] private ItemData itemData;
    [SerializeField] private Canvas flyingTextCanvas;
    
    private IItemInterface itemInterface;
    private TextMeshProUGUI itemName;
    
    private void Awake()
    {
        itemName = flyingTextCanvas.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        
        if (itemName != null)
        {
            itemName.text = itemData.ItemName;
        }
        
        flyingTextCanvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        itemInterface = other.gameObject.GetComponent<IItemInterface>();

        if (itemInterface != null)
        {
            flyingTextCanvas.enabled = true;
            itemInterface.InteractionReady(true);
            itemInterface.OnInteraction += OnInteracted;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (itemInterface != null)
        {
            flyingTextCanvas.enabled = false;
            itemInterface.ReceiveInfoText(null,null);
            itemInterface.InteractionReady(false);
            itemInterface.OnInteraction -= OnInteracted;
        }
    }

    private void OnInteracted(bool isInteracting)
    {
        itemInterface.ReceiveInfoText(itemData.ItemName,itemData.ItemText);
        if(itemData as ActionItemData)
        {
            ActionItemData actionItemData = itemData as ActionItemData;
            itemInterface.ReceiveActionItemData(actionItemData);
        
            DestroyRootParent();
        }
    }

    private void DestroyRootParent()
    {
        itemInterface.OnInteraction -= OnInteracted;
        itemInterface.InteractionReady(false);
        OnItemDestroyed?.Invoke();
        
        Transform rootParent = transform.root;
        Destroy(rootParent.gameObject);
    }
}
