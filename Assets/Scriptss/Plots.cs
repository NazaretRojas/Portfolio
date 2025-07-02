using UnityEngine;

public class Plots : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        if (!HasTower())
        {
            sr.color = hoverColor;
        }
    }

    private void OnMouseExit()
    {
        if (!HasTower())
        {
            sr.color = startColor;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Upgrade panel activo: " + UIManager.Instance.IsUpgradePanelOpen);

        if (UIManager.Instance != null && UIManager.Instance.IsUpgradePanelOpen)
        {
            Debug.Log("No se puede abrir la tienda porque el panel de mejoras está activo.");
            return;
        }

        if (HasTower())
        {
            Debug.Log("Este plot ya tiene una torreta.");
            return;
        }

        TurretShopManager.Instance.OpenTurretShop(this);
    }

   
    public void BuildTower(GameObject towerPrefab)
    {
        if (towerPrefab == null)
        {
            Debug.LogError("El prefab de la torreta es nulo.");
            return;
        }

        if (HasTower())
        {
            Debug.LogWarning("Ya hay una torreta en este terreno.");
            return;
        }

        Vector3 spawnPosition = transform.position;

        GameObject turret = Instantiate(towerPrefab, spawnPosition, Quaternion.identity);
        turret.transform.SetParent(transform);

        
        Turret turretScript = turret.GetComponent<Turret>();
        if (turretScript != null)
        {
            turretScript.parentPlot = this;
        }

        Debug.Log($"Torreta colocada correctamente en el terreno: {name}");
    }
    public void RemoveTower()
    {
        if (HasTower())
        {
            
            Destroy(transform.GetChild(0).gameObject);
            Debug.Log("Torreta eliminada del plot: " + name);
        }

       
        SetPlotActive(true);
    }
    public void SetPlotActive(bool isActive)
    {
        
        GetComponent<Collider2D>().enabled = isActive && !HasTower();
    }
    public bool HasTower()
    {
        return transform.childCount > 0;
    }
}
