using System.Collections.Generic;
using UnityEngine;

public class TurretShopManager : MonoBehaviour
{
    public static TurretShopManager Instance { get;  set; }

    [SerializeField] private GameObject turretCardPrefab;
    [SerializeField] private Transform turretPanelContainer;
    [SerializeField] private GameObject turretShopPanel;
    [SerializeField] private TurretSettings[] turretSettings;

    private Plots selectedPlot;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        PopulateTurretShop();
        CloseTurretShop();
    }

    private void PopulateTurretShop()
    {
        Debug.Log("Populando la tienda de torretas...");

        foreach (Transform child in turretPanelContainer)
        {
            Destroy(child.gameObject);
        }

        List<string> equippedIDs = Object.FindFirstObjectByType<TurretInventoryManager>().GetEquippedTurrets();
        int count = 0;

        foreach (var turret in turretSettings)
        {
            if (equippedIDs.Contains(turret.ID))


            {
                if (count >= 6) break;

                GameObject cardObj = Instantiate(turretCardPrefab, turretPanelContainer);
                TurretCard card = cardObj.GetComponent<TurretCard>();
                card.SetupTurretButton(turret);
                card.OnPlaceTurret += OnTurretSelected;

                Debug.Log($"Tarjeta creada para: {turret.name} (ID: {turret.ID})");
                count++;
            }
        }
    }

    public void OpenTurretShop(Plots plot)
    {
        selectedPlot = plot;
        turretShopPanel.SetActive(true);
        Debug.Log($"Tienda de torretas abierta para el terreno: {plot.name}");
        DisableAllPlots();
    }

    public void CloseTurretShop()
    {
        turretShopPanel.SetActive(false);
        selectedPlot = null;
        Debug.Log("Tienda de torretas cerrada");
        EnableAvailablePlots();
    }

    public Plots GetSelectedPlot()
    {
        return selectedPlot;
    }

    private void OnTurretSelected(TurretSettings turretSettings)
    {
        Debug.Log("OnTurretSelected llamado desde TurretCard");
    }

    private void DisableAllPlots()
    {
        foreach (Plots plot in FindObjectsByType<Plots>(FindObjectsSortMode.None))
        {
            plot.SetPlotActive(false);
        }
    }

    private void EnableAvailablePlots()
    {
        foreach (Plots plot in FindObjectsByType<Plots>(FindObjectsSortMode.None))
        {
            plot.SetPlotActive(true);
        }
    }
}
