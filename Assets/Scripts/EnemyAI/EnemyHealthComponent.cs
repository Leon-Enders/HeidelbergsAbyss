using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthComponent : MonoBehaviour
{
    private bool enemyAlive = true;
    [SerializeField] private Image healthBar;

    private float maxHealth;
    private ICombatInterface combatInterface;

    private void Start()
    {
        TryGetComponent(out combatInterface);
        maxHealth = combatInterface.MaxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = combatInterface.CharacterAttributeData.HealthPoints / maxHealth;

        if(combatInterface.CharacterAttributeData.HealthPoints <= 0 && enemyAlive)
        {
            enemyAlive = false;
        }
    }
}
