using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI percentageText;
    private HealthSystem health;

    void Start()
    {
        health = GetComponent<HealthSystem>();
        if (health != null)
        {
            health.OnHealthChanged += UpdateUI;
        }
    }

    void UpdateUI(float currentHealth, float maxHealth)
    {
        float fillAmount = currentHealth / maxHealth;
        fillImage.fillAmount = fillAmount;
        percentageText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
    }
}