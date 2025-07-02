using UnityEngine;

public class SlowEffect : MonoBehaviour
{
    [SerializeField] private float baseSlowAmount = 0.5f;
    [SerializeField] private float baseSlowDuration = 2f;
    [SerializeField][Range(0f, 100f)] private float baseSlowChance = 100f;
    [SerializeField] private float slowChancePerLevel = 5f;

    public void ApplyEffect(GameObject enemy, int level)
    {
        float totalChance = baseSlowChance + slowChancePerLevel * level;
        if (Random.value > totalChance / 100f) return;

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.ApplySlow(baseSlowAmount, baseSlowDuration);

            EnemyAnimations anim = enemy.GetComponent<EnemyAnimations>();
            if (anim != null)
            {
                anim.PlayEffectParticles("freeze");
            }
        }
    }
    private void OnEnable()
    {
        
        SoundManager.Instance.PlayFreezeEffect();
    }
}