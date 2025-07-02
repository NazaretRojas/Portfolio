using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
using UnityEngine.SceneManagement;

public class NotificationButtonManager : MonoBehaviour
{
    [SerializeField] private Button activateButton;
    [SerializeField] private Button deactivateButton;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private float displayDuration = 2f;

    private const string NOTIFICATION_KEY = "NotificationsEnabled";
    private Coroutine hideTextCoroutine;

    private void Start()
    {
        activateButton.onClick.AddListener(ActivateNotifications);
        deactivateButton.onClick.AddListener(DeactivateNotifications);

       
        if (statusText != null)
            statusText.gameObject.SetActive(false);

#if UNITY_ANDROID
        CreateAndroidNotificationChannel();
#endif
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    private void ActivateNotifications()
    {
        PlayerPrefs.SetInt(NOTIFICATION_KEY, 1);
        PlayerPrefs.Save();
        ShowStatus("Notifications Enabled");
        Debug.Log("Notifications enabled");

#if UNITY_ANDROID
        var notification = new AndroidNotification
        {
            Title = "Hello!",
            Text = "Notifications are now enabled.",
            SmallIcon = "icon_small", 
            FireTime = System.DateTime.Now.AddSeconds(5)
        };
        AndroidNotificationCenter.SendNotification(notification, "default");
#endif
    }

    private void DeactivateNotifications()
    {
        PlayerPrefs.SetInt(NOTIFICATION_KEY, 0);
        PlayerPrefs.Save();
        ShowStatus("Notifications Disabled");
        Debug.Log("Notifications disabled");

#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllDisplayedNotifications();
        AndroidNotificationCenter.CancelAllScheduledNotifications();
#endif
    }

    private void ShowStatus(string message)
    {
        if (statusText == null) return;

        statusText.text = message;
        statusText.gameObject.SetActive(true);

        if (hideTextCoroutine != null)
            StopCoroutine(hideTextCoroutine);

        hideTextCoroutine = StartCoroutine(HideStatusAfterDelay());
    }

    private IEnumerator HideStatusAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        statusText.gameObject.SetActive(false);
    }

#if UNITY_ANDROID
    private void CreateAndroidNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "default",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "For general notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
#endif
}