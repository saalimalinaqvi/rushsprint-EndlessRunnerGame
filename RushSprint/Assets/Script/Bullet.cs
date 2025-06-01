using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 20f;
    public string targetTag = ""; // e.g., "Enemy" for player bullet, "Player" for enemy bullet
    public Vector3 direction = Vector3.forward;
    public bool isEnemyBullet = false;
    public bool canHitObstacle = false; // Only true for player bullets

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Deal damage to valid target
        if (!string.IsNullOrEmpty(targetTag) && other.CompareTag(targetTag))
        {
            Debug.Log($"Bullet Fired for {targetTag}");

            HealthSystem health = other.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
            return;
        }

        //If player bullet and hits Obstacle
        if (!isEnemyBullet && canHitObstacle && other.CompareTag("Obstacle"))
        {
            Debug.Log("Player Bullet For Obstacles");
            Obstacle obstacle = other.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obstacle.TakeDamage();
            }
            Destroy(gameObject);
        }
    }
}
