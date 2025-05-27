using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public float destroyDistanceBehindPlayer = 5f;

    private float nextFireTime;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Rotate once towards the player's position at spawn
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0; // ignore vertical difference
        if (lookDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDir);

        // Destroy on death
        GetComponent<HealthSystem>().OnDeath += Die;
    }

    private void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + 1f / fireRate;
        }

        // Auto destroy if player passes
        if (player.position.z - transform.position.z > destroyDistanceBehindPlayer)
        {
            Destroy(gameObject);
        }
    }

    void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        bulletScript.damage = 20f;
        bulletScript.targetTag = "Player";
        bulletScript.canHitObstacle = false;
        bulletScript.isEnemyBullet = true;
        bulletScript.direction = (player.position - firePoint.position).normalized;

        SoundManager.Instance.PlayAudio(SoundType.SHOOT);

        Destroy(bullet, 3f); // Auto destroy after 3 seconds
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
