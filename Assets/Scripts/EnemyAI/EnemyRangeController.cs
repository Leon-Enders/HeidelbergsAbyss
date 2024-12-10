using System;
using TMPro;
using UnityEngine;

public class EnemyRangeController : MonoBehaviour
{
    public delegate void PullDelegate(ICombatInterface target);
    public event PullDelegate OnPullChanged;
    
    
   
    
    public bool IsPulled => isPulled;
    public ICombatInterface TargetCombatInterface => targetCombatInterface;

    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private SphereCollider pullRange;

    private ICombatInterface targetCombatInterface;
    private bool isPulled;
    void OnTriggerEnter(Collider other)
    {
        ICombatInterface combatInterface = other.gameObject.GetComponent<ICombatInterface>();
        if(combatInterface != null && combatInterface.GetCharacterTag() == "Player" && !combatInterface.IsDead)
        {
            targetCombatInterface = combatInterface;
            targetCombatInterface.OnDeath += OnTargetDeath;
            enemyNameText.enabled = true;
            OnPullChanged?.Invoke(targetCombatInterface);
            isPulled = true;
            pullRange.radius = 20;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        ICombatInterface combatInterface = other.gameObject.GetComponent<ICombatInterface>();
        if(combatInterface != null && combatInterface.GetCharacterTag() == "Player")
        {
            isPulled = false;
            pullRange.radius = 10;

            if (targetCombatInterface != null)
            {
                targetCombatInterface.OnDeath -= OnTargetDeath;
            }
        }
    }

    private void OnDestroy()
    {
        if (targetCombatInterface != null)
        {
            targetCombatInterface.OnDeath -= OnTargetDeath;
        }
    }

    private void OnTargetDeath()
    {
        isPulled = false;
        pullRange.radius = 10;
        OnPullChanged?.Invoke(null);
    }

}

