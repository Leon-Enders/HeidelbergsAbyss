using UnityEngine;
using UnityEngine.UI;

public class StatsBarManager : MonoBehaviour
{

    [SerializeField] private CombatComponent combatComponent;

    public Image healthBar;

    public Image staminaBar;

    
    void Update()
    {
        UpdateHealthBar();
        UpdateStaminaBar();
    }

    private void UpdateHealthBar() {
        healthBar.fillAmount = combatComponent.CharacterAttributeData.HealthPoints / 1000f;
    }

    private void UpdateStaminaBar() {
        staminaBar.fillAmount = combatComponent.CharacterAttributeData.StaminaPoints / 100f;
    }
}
