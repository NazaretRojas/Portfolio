using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text currencyText;

    private void Start()
    {
        if (CurrencyManager.Instance != null)
        {
            UpdateUI(CurrencyManager.Instance.CurrentCurrency);
            CurrencyManager.Instance.OnCurrencyChanged += UpdateUI;
        }
        else
        {
            Debug.LogWarning("CurrencyManager.Instance es null al iniciar CurrencyDisplay.");
        }
    }

    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateUI;
        }
    }


    private void UpdateUI(int newAmount)
    {
        currencyText.text = newAmount.ToString();
    }
}