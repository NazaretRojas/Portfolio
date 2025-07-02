using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VibrationController : MonoBehaviour
{
    public Button activateButton;
    public Button deactivateButton;
    public TMP_Text messageText;

    private const string VIBRATION_PREF_KEY = "vibration_enabled";
    private bool vibrationEnabled = true;

    private void Awake()
    {
        vibrationEnabled = PlayerPrefs.GetInt(VIBRATION_PREF_KEY, 1) == 1;
    }

    private void Start()
    {
        activateButton.onClick.AddListener(ActivateVibration);
        deactivateButton.onClick.AddListener(DeactivateVibration);

        UpdateMessage(vibrationEnabled);
    }

    private void ActivateVibration()
    {
        SetVibrationEnabled(true);
        Vibrate();
    }

    private void DeactivateVibration()
    {
        SetVibrationEnabled(false);
    }

    private void SetVibrationEnabled(bool enabled)
    {
        vibrationEnabled = enabled;
        PlayerPrefs.SetInt(VIBRATION_PREF_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();
        UpdateMessage(enabled);
    }

    private void UpdateMessage(bool enabled)
    {
        messageText.text = enabled ? "VIBRACIÓN ACTIVADA" : "VIBRACIÓN DESACTIVADA";
    }

    public void Vibrate()
    {
        if (vibrationEnabled)
        {
#if UNITY_ANDROID
            Handheld.Vibrate();
#else
            Debug.Log("La vibración solo está disponible en Android.");
#endif
        }
    }
}
