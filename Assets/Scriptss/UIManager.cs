using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get;  set; }

    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject upgradePanel;

    public bool IsUpgradePanelOpen => upgradePanel != null && upgradePanel.activeSelf;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleShopPanel()
    {
        if (IsUpgradePanelOpen)
        {
            Debug.Log("No se puede abrir la tienda mientras está abierto el panel de mejora.");
            return;
        }

        if (shopPanel != null)
        {
            bool isActive = shopPanel.activeSelf;
            shopPanel.SetActive(!isActive);
            Debug.Log($"ShopPanel ahora {(shopPanel.activeSelf ? "abierto" : "cerrado")}");
        }
    }

    public void ToggleUpgradePanel()
    {
        if (shopPanel != null && shopPanel.activeSelf)
        {
            shopPanel.SetActive(false);
            Debug.Log("Tienda cerrada porque se va a abrir el panel de mejora.");
        }

        if (upgradePanel != null)
        {
            bool isActive = upgradePanel.activeSelf;
            upgradePanel.SetActive(!isActive);
            Debug.Log($"UpgradePanel ahora {(upgradePanel.activeSelf ? "abierto" : "cerrado")}");
        }
    }
    public void OpenUpgradePanel()
    {
        CloseAllPanels();
        upgradePanel.SetActive(true);
    }
    public void CloseAllPanels()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);

        if (upgradePanel != null)
            upgradePanel.SetActive(false);

        Debug.Log("Todos los paneles han sido cerrados.");
    }

    public void CloseTurretShopPanel()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
            Debug.Log("Cerrar panel de tienda de torretas");
        }
    }

    public void CloseUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
            Debug.Log("Cerrar panel de mejoras");
        }
    }
    private void Start()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);

        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }
}
