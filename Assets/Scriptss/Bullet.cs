using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float range = 3f;
    [SerializeField] private float maxLifeTime = 2f;
    [SerializeField] private float baseDamage = 5f;

    private float totalDamage;
    private int turretLevel;
    private Vector2 startPosition;
    private Vector2 direction;

    private void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, maxLifeTime);
    }

    public void Initialize(Vector2 shootDirection, float extraDamage, int level)
    {
        direction = shootDirection.normalized;
        totalDamage = baseDamage + extraDamage;
        turretLevel = level;
        rb.linearVelocity = direction * speed;
    }

    private void Update()
    {
        if (Vector2.Distance(startPosition, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && enemy.HasReachedEnd)
        {
            return; 
        }

        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.DealDamage(totalDamage);
        }

        GetComponent<BurnEffect>()?.ApplyEffect(other.gameObject, turretLevel);
        GetComponent<PoisonEffect>()?.ApplyEffect(other.gameObject, turretLevel);
        GetComponent<SlowEffect>()?.ApplyEffect(other.gameObject, turretLevel);

        Destroy(gameObject);
    }
}
