
using UnityEngine;

public class VulnerableHitbox : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject bossReference;


    
    private void OnTriggerEnter(Collider other)
    {
            print("OnTrigger!");
            ICombatInterface combatInterface = bossReference.GetComponent<ICombatInterface>();
            if(combatInterface != null && !combatInterface.IsDead && other.gameObject != gameObject)
            {
                combatInterface.TakeDamage(damage);
            }
        
    }
}
