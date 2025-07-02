using UnityEngine;
using System;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Waypoint waypoint;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private int rewardAmount = 10;
    [SerializeField] private int damageToPlayer = 1;

    private int _currentWaypointIndex = 0;
    private Vector3 _lastPointPosition;
    private SpriteRenderer _spriteRenderer;
    private EnemyHealth _enemyHealth;

    private bool _isMoving = true;
    private float originalSpeed;
    private bool _hasReachedEnd = false;

    public static Action<Enemy> OnEndReached;

    public int DamageToPlayer => damageToPlayer;
    public bool HasReachedEnd => _hasReachedEnd; 

    private void Start()
    {
        StopAllCoroutines();
        originalSpeed = moveSpeed;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyHealth = GetComponent<EnemyHealth>();

        if (waypoint != null)
        {
            _lastPointPosition = waypoint.GetWaypointPosition(0);
            transform.position = _lastPointPosition;
        }

        _hasReachedEnd = false;
    }

    private void OnEnable()
    {
        _hasReachedEnd = false;
    }

    private void Update()
    {
        if (waypoint == null || !_isMoving || _hasReachedEnd) return;

        Move();

        if (CurrentPointPositionReached())
            UpdateCurrentPointIndex();
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, CurrentPointPosition, moveSpeed * Time.deltaTime);
    }

    private bool CurrentPointPositionReached()
    {
        float distance = (transform.position - CurrentPointPosition).magnitude;
        if (distance < 0.1f)
        {
            _lastPointPosition = transform.position;
            return true;
        }
        return false;
    }

    private void UpdateCurrentPointIndex()
    {
        int lastIndex = waypoint.Points.Length - 1;
        if (_currentWaypointIndex < lastIndex)
        {
            _currentWaypointIndex++;
            UpdateRotation();
        }
        else
        {
            EndPointReached();
        }
    }

    private void UpdateRotation()
    {
        Vector3 dir = CurrentPointPosition - transform.position;
        if (dir != Vector3.zero)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void EndPointReached()
    {
        _hasReachedEnd = true;
        StopAllCoroutines();

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.TakeDamage(damageToPlayer);
        }
        else
        {
            Debug.LogWarning("[Enemy] No se encontró LevelManager.Instance para aplicar daño.");
        }

        OnEndReached?.Invoke(this);

        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(gameObject);
    }

    private Vector3 CurrentPointPosition => waypoint.GetWaypointPosition(_currentWaypointIndex);

    public void StopMovement() => _isMoving = false;
    public void ResumeMovement() => _isMoving = true;

    public void SetWaypoint(Waypoint newWaypoint)
    {
        waypoint = newWaypoint;
        _currentWaypointIndex = 0;
        _lastPointPosition = waypoint.GetWaypointPosition(0);
        transform.position = _lastPointPosition;
        _hasReachedEnd = false;
    }

    public void SetMoveSpeed(float newSpeed) => moveSpeed = newSpeed;

    public void SetReward(int reward) => rewardAmount = reward;

    public void RewardPlayer()
    {
        CurrencySystem.Instance.AddCoins(rewardAmount);
        Debug.Log($"Recompensa: {rewardAmount} monedas otorgadas");
    }

    public void ApplyBurn(float dps, float duration)
    {
        if (isActiveAndEnabled)
            StartCoroutine(BurnCoroutine(dps, duration));
    }

    private IEnumerator BurnCoroutine(float dps, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (!_enemyHealth) yield break; 
            _enemyHealth.DealDamage(dps * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void ApplyPoison(float dps, float duration)
    {
        if (isActiveAndEnabled)
            StartCoroutine(PoisonCoroutine(dps, duration));
    }

    private IEnumerator PoisonCoroutine(float dps, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (!_enemyHealth) yield break; 
            _enemyHealth.DealDamage(dps * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void ApplySlow(float slowAmount, float duration)
    {
        if (isActiveAndEnabled)
        {
            StopCoroutine("SlowCoroutine");
            StartCoroutine(SlowCoroutine(slowAmount, duration));
        }
    }

    private IEnumerator SlowCoroutine(float slowAmount, float duration)
    {
        float original = moveSpeed;
        moveSpeed *= (1f - slowAmount);
        yield return new WaitForSeconds(duration);
        moveSpeed = original;
    }
}