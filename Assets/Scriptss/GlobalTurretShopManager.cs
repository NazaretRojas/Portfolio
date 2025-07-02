using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalTurretShopManager : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string turretID;
        public int price;
        public Sprite image;
        public GameObject itemUI;
    }

    [SerializeField] private List<ShopItem> shopItems;  
    private List<ShopItem> runtimeShopItems;  

    [SerializeField] private Transform shopContainer;
    [SerializeField] private GameObject noMoreItemsPanel;
    [SerializeField] private GameObject notEnoughCurrencyPanel;
    [SerializeField] private CurrencyManager currencyManager;
    [SerializeField] private TextMeshProUGUI warningText;

    private TurretInventoryManager inventoryManagerInstance;
    

    private void Start()
    {
        
        runtimeShopItems = new List<ShopItem>(shopItems);

        inventoryManagerInstance = FindFirstObjectByType<TurretInventoryManager>();

        if (inventoryManagerInstance == null)
        {
            Debug.LogWarning("TurretInventoryManager no encontrado en la escena.");
        }

        FilterShopItems(); 

        RefreshShop();

        if (noMoreItemsPanel != null)
            noMoreItemsPanel.SetActive(false);
    }

    
    private void FilterShopItems()
    {
        if (inventoryManagerInstance == null) return;

        List<string> unlocked = inventoryManagerInstance.GetUnlockedTurrets();

        runtimeShopItems.RemoveAll(item => unlocked.Contains(item.turretID));
    }

    public void BuyTurret(string turretID)
    {
        int index = runtimeShopItems.FindIndex(i => i.turretID == turretID);
        if (index < 0) return;

        ShopItem item = runtimeShopItems[index];

        if (currencyManager.CurrentCurrency >= item.price)
        {
            currencyManager.SpendCurrency(item.price);

            if (inventoryManagerInstance != null)
            {
                inventoryManagerInstance.AddToInventory(item.turretID);
            }
            else
            {
                Debug.LogWarning("No se pudo agregar la torreta al inventario: inventoryManagerInstance es null.");
            }

            
            runtimeShopItems.RemoveAt(index);

            RefreshShop();

            if (runtimeShopItems.Count == 0 && noMoreItemsPanel != null)
            {
                warningText.text = "No more turrets available for purchase.";
                noMoreItemsPanel.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Not enough currency");

            if (notEnoughCurrencyPanel != null)
                notEnoughCurrencyPanel.SetActive(true);

            if (warningText != null)
            {
                warningText.text = "Not enough currency.";
                warningText.gameObject.SetActive(true);
            }

            Invoke(nameof(HideCurrencyWarning), 2f);
        }
    }

    private void HideCurrencyWarning()
    {
        if (notEnoughCurrencyPanel != null)
            notEnoughCurrencyPanel.SetActive(false);

        if (warningText != null)
            warningText.gameObject.SetActive(false);
    }

    private void RefreshShop()
    {
        foreach (Transform child in shopContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (ShopItem item in runtimeShopItems)
        {
            if (item.itemUI == null)
            {
                Debug.LogWarning($"ItemUI no asignado para {item.turretID}, se omitirá.");
                continue;
            }

            GameObject instance = Instantiate(item.itemUI, shopContainer); 

            Transform buyButton = instance.transform.Find("BuyButton");
            Transform priceText = instance.transform.Find("BuyButton/PriceText");
            Transform turretImage = instance.transform.Find("TurretImage");

            if (priceText != null)
                priceText.GetComponent<TextMeshProUGUI>().text = item.price.ToString();

            if (turretImage != null && item.image != null)
                turretImage.GetComponent<Image>().sprite = item.image;

            if (buyButton != null)
            {
                Button btn = buyButton.GetComponent<Button>();
                btn.onClick.RemoveAllListeners();
                string turretID = item.turretID;
                btn.onClick.AddListener(() => BuyTurret(turretID));
            }
        }
    }
}
