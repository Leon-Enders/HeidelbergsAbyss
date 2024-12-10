using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "CastleAdventure/Item/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemName => itemName;
    public string ItemText => itemText;
    
    [SerializeField] private string itemName;
    [SerializeField] private string itemText;
}
