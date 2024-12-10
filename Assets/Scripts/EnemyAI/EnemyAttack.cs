using UnityEngine;

public class EnemyAttack : MonoBehaviour 
{
    public bool IsAttacking => isAttacking;
    
    
    [Header("Attack Properties")]
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private AnimationData attack;
    [SerializeField] private float attackRotationSpeed = 1f;
    
     private EnemyRangeController enemyRangeController;

    private ICombatInterface combatInterface;
    private IPlayableInterface playableInterface;
    private bool isAttacking = false;

    private bool hasEnemyRangeController;
    private void Awake()
    {
        TryGetComponent(out combatInterface);
        TryGetComponent(out playableInterface);
        hasEnemyRangeController = TryGetComponent(out enemyRangeController);
    }
    private void Update()
    {
        TryToAttack();
        if (isAttacking)
        {
            UpdateRotation();
        }
    }
    private void TryToAttack()
    {
        if(hasEnemyRangeController) 
        { 
            if(!isAttacking && enemyRangeController.IsPulled && !combatInterface.IsDead && !combatInterface.IsHit)
            {
                if (enemyRangeController.TargetCombatInterface.IsDead)
                {
                    return;
                }
                Vector3 posEnemy = transform.position;
                Vector3 posTarget = enemyRangeController.TargetCombatInterface.CharacterTransform.position;
                float distance = Vector3.Distance(posEnemy, posTarget);

                if(distance <= attackRange) 
                {
                    isAttacking = true;
                    if(combatInterface != null)
                    {
                        playableInterface.OnAnimationEvent += combatInterface.DealDamageInRadius;
                        playableInterface.OnAnimationFinished += ResetAttack;
                        playableInterface.PlayClip(attack);
                    }
                }
            }
        }
    }

    private void UpdateRotation()
    {
        Vector3 direction = (enemyRangeController.TargetCombatInterface.CharacterTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * attackRotationSpeed);
    }
    
    private void ResetAttack()
    {
        isAttacking = false;
    }
    
    
}
