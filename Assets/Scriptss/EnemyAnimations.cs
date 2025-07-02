using System.Collections;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    
    public GameObject deathParticles;
    public GameObject burnParticles;
    public GameObject poisonParticles;
    public GameObject freezeParticles;

    private Animator _animator;
    private Enemy _enemy;
    private EnemyHealth _enemyHealth;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _enemyHealth = GetComponent<EnemyHealth>();

        if (deathParticles == null)
        {
            Debug.LogWarning($"[EnemyAnimations] No se asignaron partículas de muerte en {gameObject.name}");
        }
    }

    private void PlayDeathAnimation()
    {
        if (_animator != null)
            _animator.SetTrigger("Die");
    }

    private IEnumerator PlayDead()
    {
        _enemy.StopMovement();

        PlayDeathAnimation();

        if (deathParticles != null)
        {
            
            GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);

            
            Destroy(particles, 2f);
        }

        yield return new WaitForSeconds(0.5f);

        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(_enemy.gameObject);
    }

    private void EnemyDead(EnemyHealth enemyHealth)
    {
        if (_enemyHealth == enemyHealth)
        {
            StartCoroutine(PlayDead());
        }
    }

    public void PlayEffectParticles(string effectType)
    {
        GameObject particlesPrefab = null;

        switch (effectType.ToLower())
        {
            case "burn":
                particlesPrefab = burnParticles;
                break;
            case "poison":
                particlesPrefab = poisonParticles;
                break;
            case "freeze":
                particlesPrefab = freezeParticles;
                break;
        }

        if (particlesPrefab != null)
        {
            
            GameObject instance = Instantiate(particlesPrefab, transform.position, Quaternion.identity, transform);

           
            Destroy(instance, 3f); 
        }
    }

    private void OnEnable()
    {
        EnemyHealth.OnEnemyKilled += EnemyDead;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyKilled -= EnemyDead;
    }
}