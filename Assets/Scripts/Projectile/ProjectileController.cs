using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float speed;
    [SerializeField] private int projectileDamage = 40;
    [SerializeField] private GameObject hitEffect;

    private void Start()
    {
        rigidBody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }
    
    void OnCollisionEnter(Collision other)
    {
        ICombatInterface combatInterface = other.gameObject.GetComponent<ICombatInterface>();
        if (combatInterface != null && !combatInterface.IsDead && other.gameObject != gameObject)
        {
            combatInterface.TakeDamage(projectileDamage);
        }
        if (other.gameObject.CompareTag("Destroyable"))
        {
            Destroy(other.gameObject);
        }
        Destroy(Instantiate(hitEffect, transform.position, transform.rotation), 1f);
        Destroy(gameObject);
    }


}
