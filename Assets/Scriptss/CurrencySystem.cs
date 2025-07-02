using UnityEngine;
using System;
using TMPro;

public class CurrencySystem : MonoBehaviour
{
    public static CurrencySystem Instance { get;  set; }

    [SerializeField] private int startingCoins = 100;
    [SerializeField] private TextMeshProUGUI coinsText;

    private int totalCoins;

    public int TotalCoins => totalCoins;
    public event Action<int> OnCoinsChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[CurrencySystem] Instancia duplicada encontrada. Destruyendo este objeto.");
            Destroy(gameObject); 
            return;
        }

        Instance = this;

        

        totalCoins = startingCoins;
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        Debug.Log($"[CurrencySystem] Añadidas {amount} monedas. Total ahora: {totalCoins}");
        OnCoinsChanged?.Invoke(totalCoins);
        UpdateUI();
    }

    public bool RemoveCoins(int amount)
    {
        Debug.Log($"[CurrencySystem] Intentando remover {amount} monedas. Total actual: {totalCoins}");

        if (amount > totalCoins)
        {
            Debug.LogWarning("[CurrencySystem] No hay suficientes monedas.");
            return false;
        }

        totalCoins -= amount;
        Debug.Log($"[CurrencySystem] Compra exitosa. Monedas restantes: {totalCoins}");
        OnCoinsChanged?.Invoke(totalCoins);
        UpdateUI();
        return true;
    }

    private void UpdateUI()
    {
        if (coinsText != null)
        {
            coinsText.text = totalCoins.ToString();
        }
        else
        {
            Debug.LogWarning("No se ha asignado el TextMeshProUGUI para mostrar las monedas.");
        }
    }

    public void ResetCurrency()
    {
        totalCoins = startingCoins;
        UpdateUI();
        OnCoinsChanged?.Invoke(totalCoins);
    }
}
