using UnityEngine;

public class DeathTriggerVolume : MonoBehaviour
{
    private CombatComponent combatComponent;    
    [SerializeField] private Color gizmoColor = Color.green;

    void OnTriggerEnter(Collider other)
    {
        ICombatInterface combatInterface = other.gameObject.GetComponent<ICombatInterface>();
        if(combatInterface != null && combatInterface.GetCharacterTag() == "Player" && !combatInterface.IsDead)
        {
            combatComponent = other.gameObject.GetComponent<CombatComponent>();
            combatComponent.TakeDamage(10000);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix; 
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size); 
        }
    }
}
