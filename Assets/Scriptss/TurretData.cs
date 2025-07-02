using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretData", menuName = "TurretData", order = 1)]
public class TurretData : ScriptableObject
{
   
    public string turretName;
    public Sprite turretIcon;

    
    public float range = 3f;
    public float rotationSpeed = 180f;

  
    public int maxLevel = 3;
    public float[] damagePerLevel;
    public float[] fireRatePerLevel;
    public int[] upgradeCosts;

    public float GetDamage(int level)
    {
        if (level >= 0 && level < damagePerLevel.Length)
            return damagePerLevel[level];
        return damagePerLevel[damagePerLevel.Length - 1];
    }

    public float GetFireRate(int level)
    {
        if (level >= 0 && level < fireRatePerLevel.Length)
            return fireRatePerLevel[level];
        return fireRatePerLevel[fireRatePerLevel.Length - 1];
    }

    public int GetUpgradeCost(int level)
    {
        if (level >= 0 && level < upgradeCosts.Length)
            return upgradeCosts[level];
        return upgradeCosts[upgradeCosts.Length - 1];
    }
}