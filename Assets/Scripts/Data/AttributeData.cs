using UnityEngine;

[CreateAssetMenu(fileName = "Attribute Data", menuName = "CastleAdventure/AttributeData")]

public class AttributeData : ScriptableObject
{
    public int AttackPower => attackPower;
    public string CharacterTag => characterTag;



     public int HealthPoints
    {
        get => healthPoints;
        set => healthPoints = value;
    }
    
    public int StaminaPoints
    {
        get => staminaPoints;
        set => staminaPoints = value;
    }


    [SerializeField] private int healthPoints;
    [SerializeField] private int staminaPoints;
    [SerializeField] private int attackPower;
    [SerializeField] private string characterTag;


    public AttributeData CreateCopy()
    {
        
        AttributeData copy = ScriptableObject.CreateInstance<AttributeData>();

        copy.healthPoints = this.healthPoints;
        copy.staminaPoints = this.staminaPoints;
        copy.attackPower = this.attackPower;
        copy.characterTag = this.characterTag;

        return copy;
    }

}
