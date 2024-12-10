using System.Collections;
using BossInfo;
using UnityEngine;
using UnityEngine.AI;


public class BossController : MonoBehaviour
{
    public bool isWalking => !navMeshAgent.isStopped;
    
    
    [Header("Boss Properties")]
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private float castRange = 8f;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float bossActionFrequency = 3f;
    [SerializeField] private float bossRangedAbilityGlobalCooldown = 3000f;
    [SerializeField] private float vulnerabilityThreshold = 500f;
    [SerializeField] private float attackRotationSpeed = 2f;
    
    
    [Header("References")]
    [SerializeField] private EnemyRangeController enemyRangeController;
    
    private bool isEncounterActive;
    private bool isMeleeRange;
    private bool hasAction = false;
    private bool canTriggerVulnerability = true;
    public Vector3 playerPos;

    private BossActionManager bossActionManager;
    private NavMeshAgent navMeshAgent;
    private IPlayableInterface playableInterface;
    private ICombatInterface combatInterface;
    private ICombatInterface targetCombatInterface;
    
    
    private Coroutine bossActionHandler;
    private void Awake()
    {
        TryGetComponent(out navMeshAgent);
        TryGetComponent(out playableInterface);
        TryGetComponent(out combatInterface);
        TryGetComponent(out bossActionManager);
        
        navMeshAgent.speed = walkSpeed;
        navMeshAgent.isStopped = true;
        
        enemyRangeController.OnPullChanged += UpdateEncounterStatus;
        playableInterface.OnAnimationFinished += OnAnimationFinished;
    }

    private void Update()
    {
        if (hasAction)
        {
            UpdateRotation();
        }
    }

    private void FixedUpdate()
    {

        if(!isEncounterActive) return;
        UpdateNavMeshAgent();
        playerPos = targetCombatInterface.CharacterTransform.position;
    }

    

    private void UpdateEncounterStatus(ICombatInterface target)
    {
        if (target == null)
        {
            print("Encounter ended!");
            targetCombatInterface = null;
            isEncounterActive = false;
            navMeshAgent.isStopped = true;
            
            if (bossActionHandler != null)
            {
                StopCoroutine(bossActionHandler);
            }
        }
        else
        {
            print("Encounter started!");
            targetCombatInterface = target;
            isEncounterActive = true;
            bossActionHandler = StartCoroutine(HandleBossAction());
            navMeshAgent.isStopped = false;
        }
       
    }
    
    private bool IsInMeleeRange()
    {
        return Vector3.Distance(transform.position, targetCombatInterface.CharacterTransform.position) <= meleeRange;
    }

    private bool IsInCastRange()
    {
        return Vector3.Distance(transform.position, targetCombatInterface.CharacterTransform.position) <= castRange;
    }

    private void UpdateNavMeshAgent()
    {
        navMeshAgent.destination = targetCombatInterface.CharacterTransform.position;
        if(hasAction || combatInterface.IsDead || combatInterface.IsHit)
        {
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = false;
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
    }
    
    
    private BossAction DetermineAction()
    {
        if (combatInterface.IsHit || combatInterface.IsDead) return null;
        
        if (canTriggerVulnerability && combatInterface.CharacterAttributeData.HealthPoints <= vulnerabilityThreshold)
        {
            canTriggerVulnerability = false;
            return bossActionManager.VulnerabilityAction;
        }
        
        if (IsInMeleeRange())
        {
            BossAction randomMeleeAction = bossActionManager.GetRandomMeleeAction();
            if (randomMeleeAction != null) return randomMeleeAction;
        }
        if(IsInCastRange())
        {
            BossAction randomRangedAction = bossActionManager.GetRandomRangedAction();
            if (randomRangedAction != null) return randomRangedAction;
        }
        
        return null;
    }
  
    // This is the main Boss Action loop
    IEnumerator HandleBossAction()
    {
        bossActionManager.ApplyStartCooldowns();
        while (true)
        {
            // Determine next BossAction
            BossAction nextAction = DetermineAction();

            // Wait until the action + cooldown is finished
            yield return StartCoroutine(ExecuteBossAction(nextAction));
        }
    }
    
    IEnumerator ExecuteBossAction(BossAction currentAction)
    {
        
        if (currentAction != null && playableInterface != null )
        {
            // we are in an action, apply cooldown, play animation and execute action task
            hasAction = true;
            StartCoroutine(currentAction.ApplyCooldown());
          
            
            playableInterface.PlayClip(currentAction.ActionData.ActionAnimData,currentAction.ActionData.ActionSoundData);
            playableInterface.OnAnimationEvent += currentAction.ExecuteActionTask;
            
        
            //Wait until the action is finished, it is finished when the animation is done
            yield return new WaitWhile(() => hasAction);
        
            //Unsubscribe the current actionTask
            playableInterface.OnAnimationEvent -= currentAction.ExecuteActionTask;
            
        }
        
        // if playableInterface or the currentAction is null we return with the base action frequency
        yield return new WaitForSeconds(bossActionFrequency);
    }
    
    
    // This resets the action and triggers the waitUntil task
    private void OnAnimationFinished()
    {
        hasAction = false;
    }
    
    private void UpdateRotation()
    {
        Vector3 direction = (enemyRangeController.TargetCombatInterface.CharacterTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * attackRotationSpeed);
    }
    
}
