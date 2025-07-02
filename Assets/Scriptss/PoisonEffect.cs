using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    [SerializeField] private float basePoisonDuration = 4f;
    [SerializeField] private float basePoisonDamagePerSecond = 1f;
    [SerializeField][Range(0f, 100f)] private float basePoisonChance = 100f;
    [SerializeField] private float poisonChancePerLevel = 5f;

    public void ApplyEffect(GameObject enemy, int level)
    {
        float totalChance = basePoisonChance + poisonChancePerLevel * level;
        if (Random.value > totalChance / 100f) return;

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.ApplyPoison(basePoisonDamagePerSecond, basePoisonDuration);

            EnemyAnimations anim = enemy.GetComponent<EnemyAnimations>();
            if (anim != null)
            {
                anim.PlayEffectParticles("poison");
            }
        }
    }
    private void OnEnable()
    {

        SoundManager.Instance.PlayPoisonEffect();
    }
}