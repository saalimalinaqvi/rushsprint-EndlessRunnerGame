using UnityEngine;

public class Gem : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime); // Rotate Gem
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            GameManager.instance.AddGems(1); // Add 1 gem
            Destroy(gameObject); // Remove Gem
        }
    }
}
