using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    private float currentHealth;

    [SerializeField] private Image healthBarFill;

    public static event Action<EnemyHealth> OnEnemyHit;
    public static event Action<EnemyHealth> OnEnemyKilled;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void DealDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        OnEnemyHit?.Invoke(this);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    public static event Action<Enemy> OnEnemyDied;

    private void Die()
    {

        Enemy enemy = GetComponent<Enemy>();
        OnEnemyDied?.Invoke(enemy);

        if (enemy != null)
        {
            enemy.RewardPlayer();
        }

        
        EnemyAnimations animations = GetComponent<EnemyAnimations>();
        if (animations != null && animations.deathParticles != null)
        {
            GameObject deathFx = Instantiate(animations.deathParticles, transform.position, Quaternion.identity);
            Destroy(deathFx, 2f); 
        }

        
        Destroy(gameObject);

        OnEnemyKilled?.Invoke(this);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
}
