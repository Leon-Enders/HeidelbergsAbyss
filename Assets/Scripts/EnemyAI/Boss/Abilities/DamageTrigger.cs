using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] private int damage;

    private bool damageIsDealt = false;

    
    private void OnTriggerEnter(Collider other)
    {
        if (!damageIsDealt)
        {
            ICombatInterface combatInterface = other.gameObject.GetComponent<ICombatInterface>();
            if(combatInterface != null && !combatInterface.IsDead && other.gameObject != gameObject)
            {
                damageIsDealt = true;
                combatInterface.TakeDamage(damage);
            }
        }
    }
}
