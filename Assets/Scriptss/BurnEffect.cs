using UnityEngine;

public class BurnEffect : MonoBehaviour
{
    [SerializeField] private float baseBurnDuration = 3f;
    [SerializeField] private float baseBurnDamagePerSecond = 2f;
    [SerializeField][Range(0f, 100f)] private float baseBurnChance = 100f;
    [SerializeField] private float burnChancePerLevel = 5f;

    public void ApplyEffect(GameObject enemy, int level)
    {
        float totalChance = baseBurnChance + burnChancePerLevel * level;
        if (Random.value > totalChance / 100f) return;

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.ApplyBurn(baseBurnDamagePerSecond, baseBurnDuration);

            EnemyAnimations anim = enemy.GetComponent<EnemyAnimations>();
            if (anim != null)
            {
                anim.PlayEffectParticles("burn");
            }
        }
    }
    private void OnEnable()
    {
        
        SoundManager.Instance.PlayBurnEffect();
    }
}
