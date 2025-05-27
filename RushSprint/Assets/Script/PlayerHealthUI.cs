using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI percentageText;
    private HealthSystem playerHealth;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            playerHealth = player.GetComponent<HealthSystem>();
            playerHealth.OnHealthChanged += UpdateUI;
        }
    }

    void UpdateUI(float currentHealth, float maxHealth)
    {
        float fillAmount = currentHealth / maxHealth;
        fillImage.fillAmount = fillAmount;
        percentageText.text = Mathf.RoundToInt(fillAmount * 100f) + "%";
    }
}