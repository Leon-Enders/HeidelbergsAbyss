using System;
using System.Collections;
using System.Collections.Generic;
using BossInfo;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BossInfo
{
    public class BossAction
    {
     
        public BossActionData ActionData => bossActionData;
        public bool IsActive => isActive;
        

        private BossActionData bossActionData;
        private Action actionTask;
        private bool isActive;

        public BossAction(BossActionData bossActionData, Action actionTask)
        {
            this.bossActionData = bossActionData;
            this.actionTask = actionTask;
        }
        
        public void ExecuteActionTask()
        {
            actionTask?.Invoke();
        }

        public IEnumerator ApplyCooldown()
        {
            isActive = true;
            yield return new WaitForSeconds(bossActionData.ActionCooldown);
            isActive = false;
        }

        public IEnumerator ApplyStartingCooldown(float cooldown)
        {
            isActive = true;
            yield return new WaitForSeconds(cooldown);
            isActive = false;
        }
    }
}



public class BossActionManager : MonoBehaviour
{

    public BossAction VulnerabilityAction => vulnerabilityAction;
    
    [Header("BossActionData")]
    [SerializeField] private List<BossActionData> meleeActionData;
    [SerializeField] private List<BossActionData> rangedActionData;
    [SerializeField] private BossActionData vulnerabilityActionData;
    [SerializeField] private List<int> ApplyCooldownOnStartIndices = new List<int>();

    [Header("AbilityPrefabs")]
    [SerializeField] public SpawnRandomLightnings spawnRandomLightnings;
    [SerializeField] public LightningHunt lightningHunt;
    [SerializeField] public LightningSlash lightningSlash;
    [SerializeField] private GameObject vulnerableHitbox;
    
    
    
    
    private CapsuleCollider bossCollider;
    
    private BossAction vulnerabilityAction;
    private List<BossAction> meleeActions = new List<BossAction>();
    private List<BossAction> rangedActions = new List<BossAction>();
    private Dictionary<EBossActionTask, Action> TasksForBossAction;

    private ICombatInterface combatInterface;
    private void Awake()
    {
        TryGetComponent(out bossCollider);
        TryGetComponent(out combatInterface);
        
        InitializeBossActionManager();
    }

    private void InitializeBossActionManager()
    {
        TasksForBossAction = new Dictionary<EBossActionTask, Action>();
        TasksForBossAction.Add(EBossActionTask.DealDamageInRadius, DealDamageInRadius);
        TasksForBossAction.Add(EBossActionTask.DealDamageInLine, DealDamageInLine);
        TasksForBossAction.Add(EBossActionTask.Thunderstorm, HandleThunderstorm);
        TasksForBossAction.Add(EBossActionTask.Vulnerable, HandleVulnerability);
        TasksForBossAction.Add(EBossActionTask.LightningHunt, HandleLightningHunt);
        TasksForBossAction.Add(EBossActionTask.LightningSlash, HandleLightningSlash);

        
        //Populate boss actions
        foreach (var bossActionData in meleeActionData)
        {
            meleeActions.Add(new BossAction(bossActionData, TasksForBossAction[bossActionData.EActionTask]));
        }

        foreach (var bossActionData in rangedActionData)
        {
            rangedActions.Add(new BossAction(bossActionData, TasksForBossAction[bossActionData.EActionTask]));
        }

        vulnerabilityAction = new BossAction(vulnerabilityActionData, TasksForBossAction[vulnerabilityActionData.EActionTask]);
    }
    private void DealDamageInRadius()
    {
        combatInterface.DealDamageInRadius();
    }
    
    private void DealDamageInLine()
    {
        combatInterface.DealDamageInRadius();
    }
    
    private void HandleThunderstorm()
    {
        spawnRandomLightnings.enabled = true;
    }

    private void HandleLightningSlash()
    {
        lightningSlash.enabled = true;
    }

    private void HandleLightningHunt()
    {
        lightningHunt.enabled = true;
    }

    private void HandleVulnerability()
    {
        bossCollider.enabled = false;
        vulnerableHitbox.SetActive(true);
        
        Invoke(nameof(ResetVulnerable),vulnerabilityActionData.ActionAnimData.AnimClip.length);
    }
    
    private void ResetVulnerable()
    {
        bossCollider.enabled = true;
        vulnerableHitbox.SetActive(false);
    }

    public void ApplyStartCooldowns()
    {  
        foreach (var bossActionIndex in ApplyCooldownOnStartIndices)
        {
            StartCoroutine(rangedActions[bossActionIndex].ApplyCooldown());
        }
    }

    public BossAction GetRandomMeleeAction()
    {
        return getRandomBossAction(meleeActions);
    }

    public BossAction GetRandomRangedAction()
    {
        return getRandomBossAction(rangedActions);
    }
    
    private BossAction getRandomBossAction(List<BossAction> bossActions)
    {
        // List to populate with activatable BossActions
        var validBossActionList = new List<BossAction>();
        
        bool anyReady = false;
        foreach (var bossAction in bossActions)
        {
            if (!bossAction.IsActive)
            {
                validBossActionList.Add(bossAction);
                anyReady = true;
            }
        }
        
        //break early if all actions are not ready for activation
        if (!anyReady) return null;

        int randomIndex = Random.Range(0, validBossActionList.Count);
        return validBossActionList[randomIndex];
    }
}
