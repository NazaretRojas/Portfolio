using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get;  set; }

    [SerializeField] private int startingCurrency = 0;

    public int CurrentCurrency { get; private set; }

    public event Action<int> OnCurrencyChanged;

    private const string PlayerPrefsKey = "PlayerCurrency";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        LoadCurrency();
    }

    private void LoadCurrency()
    {
        CurrentCurrency = PlayerPrefs.GetInt(PlayerPrefsKey, startingCurrency);
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey, CurrentCurrency);
    }

    
    public void AddCurrency(int amount)
    {
        CurrentCurrency += amount;
        SaveCurrency();
        OnCurrencyChanged?.Invoke(CurrentCurrency);
    }

   
    public bool SpendCurrency(int amount)
    {
        if (CurrentCurrency >= amount)
        {
            CurrentCurrency -= amount;
            SaveCurrency();
            OnCurrencyChanged?.Invoke(CurrentCurrency);
            return true;
        }

        return false;
    }

    public void ResetCurrency()
    {
        CurrentCurrency = startingCurrency;
        SaveCurrency();
        OnCurrencyChanged?.Invoke(CurrentCurrency);
    }
}