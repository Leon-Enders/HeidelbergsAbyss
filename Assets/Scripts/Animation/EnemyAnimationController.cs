using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private int walkSpeedHash;
    private int isMovingHash;
    
    
    private EnemyRangeController enemyRangeController;
    private NavMeshAgent navMeshAgent;
    private void Awake()
    {
        TryGetComponent(out enemyRangeController);
        TryGetComponent(out navMeshAgent);
        
        walkSpeedHash = Animator.StringToHash("WalkSpeed");
        isMovingHash = Animator.StringToHash("IsMoving");
    }

    
    void Update()
    {
        UpdateAnimationStates();
    }

    private void UpdateAnimationStates()
    {
        animator.SetFloat(walkSpeedHash, enemyRangeController.IsPulled ? 1.6f : 1.3f);
        animator.SetBool(isMovingHash,navMeshAgent.hasPath);
    }
}
