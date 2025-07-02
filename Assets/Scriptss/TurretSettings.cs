using UnityEngine;

[CreateAssetMenu]
public class TurretSettings : ScriptableObject
{
    public string ID;
    public Sprite TurretShopSprite;
    public int TurretShopCost;
    public GameObject TurretPrefab;
}
