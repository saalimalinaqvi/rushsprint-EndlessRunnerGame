using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float healAmount = 5f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); // Rotate Coin
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            if (health != null)
            {
               // health.Heal(healAmount);
            }

            GameManager.instance.AddCoins(1); // Add 1 coin
            Destroy(gameObject); // Remove Coin
        }
    }
}
