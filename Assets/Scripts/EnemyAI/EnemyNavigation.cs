using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private EnemyRangeController enemyRangeController;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private float pulledSpeed = 3f;
    [SerializeField] private float patrolSpeed = 2f;

    
    private bool canPatrol;
    private int targetP;
    
    private EnemyPatrol enemyPatrol;
    private ICombatInterface combatInterface;
    private void Awake()
    {
        TryGetComponent(out combatInterface);
        canPatrol = TryGetComponent(out enemyPatrol);
    }
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (canPatrol)
        {
            targetP = enemyPatrol.targetPoint;
        }
        

        if(enemyRangeController.IsPulled) 
        {
            agent.destination = enemyRangeController.TargetCombatInterface.CharacterTransform.position;
            agent.speed = pulledSpeed;
        }
        else if(!enemyRangeController.IsPulled || !enemyRangeController.TargetCombatInterface.IsDead)
        {
            if (canPatrol)
            {
                agent.destination = enemyPatrol.patrolPoints[targetP].position;
                agent.speed = patrolSpeed;
            }
        }


        if(enemyAttack.IsAttacking || combatInterface.IsDead || combatInterface.IsHit)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;
    }
}
