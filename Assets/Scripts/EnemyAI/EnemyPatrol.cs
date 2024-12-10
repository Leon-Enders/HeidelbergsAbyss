using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] public Transform[] patrolPoints;
    [HideInInspector] public int targetPoint;
    [SerializeField] private float patrolPointRange = 3f;
    [SerializeField] private EnemyRangeController ERC;
   
    private ICombatInterface combatInterface;
    private void Awake()
    {
        gameObject.TryGetComponent(out combatInterface);
    }
    void Start()
    {
        targetPoint = 0;
    }

    void Update()
    {
        if(!ERC.IsPulled && !combatInterface.IsHit) {
            Vector3 posEnemy = transform.position;
            Vector3 posPatrolPoint = patrolPoints[targetPoint].transform.position;
            float patrolPointDistance = Vector3.Distance(posEnemy, posPatrolPoint);

            if(patrolPointDistance <= patrolPointRange)
            {
                IncreaseTargetInt();
            }
        }
    }

    void IncreaseTargetInt()
    {
        targetPoint++;
        if(targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0;
        }
    }
}
