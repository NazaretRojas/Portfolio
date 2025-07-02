using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretCard : MonoBehaviour
{
    public Action<TurretSettings> OnPlaceTurret;

    [SerializeField] private Image turretImage;
    [SerializeField] private TextMeshProUGUI turretCost;
    [SerializeField] private Button placeButton;

    public TurretSettings TurretLoaded { get; private set; }

    private void Awake()
    {
        if (placeButton != null)
        {
            placeButton.onClick.AddListener(PlaceTurret);
        }
        else
        {
            Debug.LogError("El botón no está asignado en el prefab de la tarjeta.");
        }
    }

    public void SetupTurretButton(TurretSettings turretSettings)
    {
        TurretLoaded = turretSettings;

        if (turretImage != null) turretImage.sprite = turretSettings.TurretShopSprite;
        if (turretCost != null) turretCost.text = turretSettings.TurretShopCost.ToString();
    }

    public void PlaceTurret()
    {
        if (TurretLoaded == null)
        {
            Debug.LogError("TurretLoaded es null. ¿Se llamó SetupTurretButton?");
            return;
        }

        var selectedPlot = TurretShopManager.Instance.GetSelectedPlot();

        if (selectedPlot == null)
        {
            return; 
        }

        if (selectedPlot.HasTower())
        {
            Debug.LogWarning($"El plot {selectedPlot.name} ya tiene una torreta.");
            return;
        }

        if (CurrencySystem.Instance.TotalCoins >= TurretLoaded.TurretShopCost)
        {
            CurrencySystem.Instance.RemoveCoins(TurretLoaded.TurretShopCost);
            selectedPlot.BuildTower(TurretLoaded.TurretPrefab);
            TurretShopManager.Instance.CloseTurretShop();
            Debug.Log($"Torreta colocada en {selectedPlot.name}");
        }
        else
        {
            Debug.Log("No tienes suficiente dinero para esta torreta.");
        }
    }
}
