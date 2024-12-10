using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action Item Data", menuName = "CastleAdventure/Item/ActionItemData")]
public class ActionItemData : ItemData
{
    public int ItemSlot => itemSlot;
    public Texture2D ItemIcon => itemIcon;
    public List<string> AbilitiesToUnlock => abilitiesToUnlock;
  
    [SerializeField] private int itemSlot;
    [SerializeField] private Texture2D itemIcon;
    [SerializeField] private List<string> abilitiesToUnlock;
}
