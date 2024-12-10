using UnityEngine;

public interface ICombatInterface
{
    public delegate void OnCombatDelegate();

    public event OnCombatDelegate OnDeath;
    public event OnCombatDelegate OnRespawn;
    
    public bool IsDead { get; }
    public bool IsHit { get; }
    public bool IsImmune { get;}
    public int MaxHealth { get; }
    public int StaggerCount { get;  set; }
    public Transform CharacterTransform { get; }
    public AttributeData CharacterAttributeData { get; }

    
    public void TakeDamage(int damage);
    public void DealDamageInRadius();
    public void DealDamageAroundCaster(int damage,int damageInterval, float duration, float radius);
    public string GetCharacterTag();
    public void ReceiveHeal(int heal);
    public void LossStamina(int stamina);
    public void TriggerRespawn();

    public void ActivateImmunity(float Duration);

}
