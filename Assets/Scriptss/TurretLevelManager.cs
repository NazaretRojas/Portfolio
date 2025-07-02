using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretLevelManager : MonoBehaviour
{
    
    [SerializeField] private GameObject panel;
    [SerializeField] private Image turretIcon;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private Image coinIcon;
    [SerializeField] private Button closeButton;

    
    [SerializeField] private TurretData[] turretDataArray;

    private Turret selectedTurret;

    private void Start()
    {
        panel.SetActive(false);
        closeButton.onClick.AddListener(HidePanel);
    }

    public void ShowPanel(Turret turret)
    {
        Debug.Log($"Intentando mostrar panel para: {turret?.name}");
        Debug.Log("Panel activo: " + panel.activeSelf);

        if (panel.activeSelf)
            Debug.Log("El panel ya estaba activo, refrescando.");

        UIManager.Instance.CloseAllPanels(); 
        panel.SetActive(false);              
        panel.SetActive(true);               

        selectedTurret = turret;

        TurretData data = turret.GetData();
        int level = turret.GetCurrentLevel();

        if (data != null)
        {
            turretIcon.sprite = data.turretIcon;
            levelText.text = $" {level + 1}";

            if (level >= data.maxLevel - 1)
            {
                upgradeCostText.text = "Máx.";
                coinIcon.enabled = false;
            }
            else
            {
                int cost = data.GetUpgradeCost(level + 1);
                upgradeCostText.text = cost.ToString();
                coinIcon.enabled = true;
            }
        }
    }

    public void OnUpgradeButton()
    {
        if (selectedTurret == null) return;

        selectedTurret.Upgrade();
        ShowPanel(selectedTurret);
    }

    public void OnSellButton()
    {
        if (selectedTurret == null) return;

        selectedTurret.Sell();
        HidePanel();
    }

    public void HidePanel()
    {
        panel.SetActive(false);
        selectedTurret = null;
    }
}
