using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DifficultyButton : MonoBehaviour
{
    public string sceneToLoad;

    public string levelName; 
    public int difficultyIndex;
    public GameObject lockedWarningPanel;

    private bool isUnlocked;

    [SerializeField] private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        RefreshButton();
        button.onClick.AddListener(OnButtonClick);
    }

    public void RefreshButton()
    {
        if (difficultyIndex == 0)
        {
            isUnlocked = true; 
        }
        else
        {
            
            string key = $"DifficultyPassed_{levelName}_{difficultyIndex - 1}";
            int value = PlayerPrefs.GetInt(key, -1);
            isUnlocked = (value == 1);

            Debug.Log($"[REFRESH] {levelName} dificultad {difficultyIndex}: clave {key} → valor {value} → desbloqueado = {isUnlocked}");
        }

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (button == null) return;

        button.interactable = true;
        ColorBlock colors = button.colors;

        if (isUnlocked)
        {
            colors.normalColor = Color.white;
        }
        else
        {
            colors.normalColor = Color.gray;
        }

        button.colors = colors;
    }

    private void OnButtonClick()
    {
        if (isUnlocked)
        {
            PlayerPrefs.SetInt("SelectedDifficultyIndex", difficultyIndex);
            PlayerPrefs.SetString("SelectedLevelName", levelName);
            PlayerPrefs.Save();
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            if (lockedWarningPanel != null)
            {
                lockedWarningPanel.SetActive(true);
                StartCoroutine(HideWarningAfterSeconds(2f));
            }
        }
    }

    private IEnumerator HideWarningAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (lockedWarningPanel != null)
        {
            lockedWarningPanel.SetActive(false);
        }
    }
}