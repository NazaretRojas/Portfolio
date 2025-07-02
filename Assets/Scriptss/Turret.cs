using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private TurretData turretData;

    private float targetingRange;
    private float rotationSpeed;
    private float bps;
    private float damage;

    private Transform target;
    private float timeUntilFire;

    public int TurretLevel { get;  set; } = 0;

    public Plots parentPlot;
    private void Start()
    {
        ApplyTurretData();

    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        if (!CheckTargetIsInRange())
        {
            target = null;
            return;
        }

        RotateTowardsTarget();

        timeUntilFire += Time.deltaTime;
        if (timeUntilFire >= 1f / bps)
        {
            Shoot();
            timeUntilFire = 0f;
        }
    }

    private void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy == null || enemy.HasReachedEnd) continue;

            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = hit.transform;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return target != null && Vector2.Distance(transform.position, target.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = target.position - turretRotationPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        if (target == null) return;

        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy == null || enemy.HasReachedEnd)
        {
            target = null;
            return;
        }

        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();

        Vector2 shootDirection = (target.position - firingPoint.position).normalized;
        float extraDamage = turretData.GetDamage(TurretLevel);

        bulletScript.Initialize(shootDirection, extraDamage, TurretLevel);

        float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
        bulletObj.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        SoundManager.Instance.PlayBulletShot();
    }

    private void ApplyTurretData()
    {
        targetingRange = turretData.range;
        rotationSpeed = turretData.rotationSpeed;
        bps = turretData.GetFireRate(TurretLevel);
        damage = turretData.GetDamage(TurretLevel);
    }

    public TurretData GetData() => turretData;
    public int GetCurrentLevel() => TurretLevel;

    public void Upgrade()
    {
        if (TurretLevel + 1 >= turretData.maxLevel)
        {
            Debug.Log("Torreta ya en nivel máximo.");
            return;
        }

        int cost = turretData.GetUpgradeCost(TurretLevel + 1);
        if (CurrencySystem.Instance.RemoveCoins(cost))
        {
            TurretLevel++;
            ApplyTurretData();
        }
        else
        {
            Debug.Log("No hay monedas suficientes para mejorar.");
        }
    }

    public void Sell()
    {
        int refund = turretData.GetUpgradeCost(TurretLevel) / 2;
        CurrencySystem.Instance.AddCoins(refund);

        
        if (parentPlot != null)
        {
            parentPlot.SetPlotActive(true); 
        }

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (turretData != null)
        {
            UnityEditor.Handles.color = Color.cyan;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, turretData.range);
        }
    }
   #endif
}