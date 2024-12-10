using System.Collections;
using UnityEngine;


public class CombatComponent : MonoBehaviour, ICombatInterface
{
    public event ICombatInterface.OnCombatDelegate OnDeath;
    public event ICombatInterface.OnCombatDelegate OnRespawn;

    public bool IsDead { get; private set; }
    public bool IsHit { get; private set; }
    public bool IsImmune { get; private set; }
    public int MaxHealth { get; private set;}
    public Transform CharacterTransform { get; private set; }
    public AttributeData CharacterAttributeData { get; private set; }

    public int StaggerCount
    {
        get => staggerCount;
        set => staggerCount = value;
    }
    
    
    //[SerializeField] private GameObject CharacterMesh;
    [SerializeField] private AttributeData defaultAttributeData;
    [SerializeField] private float hitboxDistance = 1.5f;
    [SerializeField] private float hitboxRadius = 1f;
    [SerializeField] private AnimationData hit;
    [SerializeField] private AnimationData death;
    [SerializeField] private SoundData hitSound;
    [SerializeField] private Transform aimTransform;
    [SerializeField] private int staggerCount = 1;
    
    

    
    private int maxStamina;
    private int currentStaggerCount;

    private bool cancelNextAttack;
    
    private IPlayableInterface playableInterface;
    private SoundManager soundManager;
    private Vector3 hitboxLocation = Vector3.zero;
    
    private bool overlapDrawn = false;
    
    private void Awake()
    {
        CharacterAttributeData = defaultAttributeData.CreateCopy();
        MaxHealth = defaultAttributeData.HealthPoints;
        maxStamina = defaultAttributeData.StaminaPoints;
        
        TryGetComponent(out playableInterface);
        TryGetComponent(out soundManager);
        
        CharacterTransform = transform;
    }

    public void TriggerRespawn()
    {
        CharacterAttributeData.HealthPoints = MaxHealth;
        playableInterface.Freeze(false);
        IsDead = false;
        OnRespawn?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        if (IsImmune) return;
        
        CharacterAttributeData.HealthPoints -= damage;
        if (CharacterAttributeData.HealthPoints > 0 && !IsDead)
        {
            ActivateHitAnimation();
        }
        else if(!IsDead)
        {
            IsDead = true;
            OnDeath?.Invoke();
            DeathAnimation();
        }
    }
    
    public string GetCharacterTag()
    {
        return CharacterAttributeData.CharacterTag;
    }

     public void DealDamageInRadius()
     { 
         playableInterface.OnAnimationEvent -= DealDamageInRadius;
         if (cancelNextAttack)
         {
             cancelNextAttack = false;
             return;
         }

        overlapDrawn = true;
        Invoke("ResetOverlapDrawn",1f);
        hitboxLocation = aimTransform.position + aimTransform.forward * hitboxDistance;
        Collider[] hitColliders = Physics.OverlapSphere(hitboxLocation, hitboxRadius,Physics.AllLayers, QueryTriggerInteraction.Ignore);
        foreach (var hitCollider in hitColliders) 
        {
            ICombatInterface combatInterface = hitCollider.gameObject.GetComponent<ICombatInterface>();
            if(combatInterface != null && !combatInterface.IsDead && GetCharacterTag() != combatInterface.GetCharacterTag())
            {
                combatInterface.TakeDamage(CharacterAttributeData.AttackPower);
            }
        }
    }

     public void DealDamageAroundCaster(int damage,int damageInterval, float duration, float radius)
     {
         StartCoroutine(DamageAroundCasterInterval(damage,damageInterval, duration, radius));
     }

     private IEnumerator DamageAroundCasterInterval(int damage, int damageInterval, float duration, float radius)
     {
         int currentIteration = 0;
         float damageFrequency = duration / damageInterval;

         while (currentIteration < damageInterval)
         {
             currentIteration++;
             
             Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius,Physics.AllLayers, QueryTriggerInteraction.Ignore);
             foreach (var hitCollider in hitColliders) 
             {
                 ICombatInterface combatInterface = hitCollider.gameObject.GetComponent<ICombatInterface>();
                 if(combatInterface != null && !combatInterface.IsDead && GetCharacterTag() != combatInterface.GetCharacterTag())
                 {
                     combatInterface.TakeDamage(damage);
                 }
             }
             
             yield return new WaitForSeconds(damageFrequency);
         }
     }
    public void ActivateImmunity(float duration)
    {
        IsImmune = true;
        Invoke(nameof(ClearImmunity), duration);
    }

    private void ClearImmunity()
    {
        IsImmune = false;
    }

    private void ActivateHitAnimation()
    {
       
        currentStaggerCount += 1;
        if (currentStaggerCount >= staggerCount)
        {
            cancelNextAttack = true;
            currentStaggerCount = 0;
            IsHit = true;
            playableInterface.OnAnimationFinished += OnHitFinished;
            playableInterface.PlayClip(hit ,hitSound);
        }
        else
        {
           soundManager.PlaySound(hitSound);
        }
    }

    private void DeathAnimation()
    {
        if (GetCharacterTag() == "Enemy")
        {
            playableInterface.OnAnimationEvent += DestroyEnemy;
        }
        else
        {
            playableInterface.OnAnimationEvent += FreezeOnDeath;
        }
        playableInterface.PlayClip(death);
    }

    public void ReceiveHeal(int heal)
    {
        CharacterAttributeData.HealthPoints += heal;
        CharacterAttributeData.HealthPoints = Mathf.Clamp(CharacterAttributeData.HealthPoints, CharacterAttributeData.HealthPoints, MaxHealth);
    }

    public void LossStamina(int stamina)
    {
        if(CharacterAttributeData != null)
        {
            CharacterAttributeData.StaminaPoints -= stamina;
            CharacterAttributeData.StaminaPoints = Mathf.Clamp(CharacterAttributeData.StaminaPoints, 0, maxStamina);
        }
    }

    private void OnHitFinished()
    {
        cancelNextAttack = false;
        IsHit = false;
        playableInterface.OnAnimationFinished -= OnHitFinished;
    }
    
    private void DestroyEnemy()
    {
        playableInterface.OnAnimationEvent -= DestroyEnemy;
        Destroy(gameObject);
    }

    private void ResetOverlapDrawn()
    {
        overlapDrawn = false;
    }

    private void OnDrawGizmos()
    {
        if(overlapDrawn)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitboxLocation, hitboxRadius);
        } 
    }

    private void FreezeOnDeath()
    {
        playableInterface.OnAnimationEvent -= FreezeOnDeath;
        playableInterface.Freeze(true);
    }
}
