using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get;  set; }
    
    [System.Serializable]
    public class DifficultyProgression
    {
        public string levelName;
        public int difficultyIndexToUnlock;
    }

    [SerializeField] private DifficultyProgression progression;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject levelCompletedPanel;

    [SerializeField] private string menuSceneName = "Nextmenu";

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private Image healthBar;

  
    [SerializeField] private int levelReward = 100;
    [SerializeField] private int maxHealth = 10; 

    private int currentHealth;
    private int currentWave = 0;
    private bool levelEnded = false;


    private CurrencyManager currencyManager;
    public int currentDifficultyIndex;

    private void Start()
    {
        
        currencyManager = CurrencyManager.Instance;

        currentHealth = maxHealth;
        levelEnded = false;
        currentWave = 0;

        gameOverPanel.SetActive(false);
        levelCompletedPanel.SetActive(false);

        UpdateHealthBar();
        UpdateWaveText();
       
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Ya existe un LevelManager, destruyendo duplicado");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gameOverPanel?.SetActive(false);
        levelCompletedPanel?.SetActive(false);

        currentDifficultyIndex = PlayerPrefs.GetInt("SelectedDifficultyIndex", 0); 

        currentHealth = maxHealth;
        UpdateHealthBar();
        UpdateWaveText();
    }

    public void TakeDamage(int amount)
    {
        if (levelEnded) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            healthBar.fillAmount = fillAmount;
        }
    }

    private void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave: {currentWave}";
        }
    }

    public void GameOver()
    {
        if (levelEnded) return;

        gameOverPanel.SetActive(true);
        levelEnded = true;
        Time.timeScale = 0f;
    }

    public void LevelCompleted()
    {
        if (levelEnded)
        {
            Debug.LogWarning("Level already ended.");
            return;
        }

        Debug.Log("Activando panel de nivel completado");

        if (levelCompletedPanel != null)
        {
            levelCompletedPanel.SetActive(true);
            Debug.Log("Panel activado");
        }
        else
        {
            Debug.LogError("El panel de nivel completado no está asignado.");
        }

        levelEnded = true;
        Time.timeScale = 0f;

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddCurrency(levelReward);
        }
        else
        {
            Debug.LogError("CurrencyManager.Instance es null al completar el nivel.");
        }

        if (rewardText != null)
        {
            rewardText.text = $"+{levelReward}";
        }

        
        int currentDifficulty = PlayerPrefs.GetInt("SelectedDifficultyIndex", 0);
        string levelName = PlayerPrefs.GetString("SelectedLevelName", "Unknown");

        string key = $"DifficultyPassed_{levelName}_{currentDifficulty}";
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();

        Debug.Log($"[SAVE] Progreso guardado: {key} = {PlayerPrefs.GetInt(key)}");


        
    }


    
    public void ReplayLevel()
    {
        Time.timeScale = 1f;
        currentHealth = maxHealth;
        levelEnded = false;
        currentWave = 0;

        UpdateHealthBar();
        UpdateWaveText();

        gameOverPanel.SetActive(false);
        levelCompletedPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        CleanupCurrencySystem();

        if (!string.IsNullOrEmpty(menuSceneName))
        {
            SceneManager.LoadScene(menuSceneName);
        }
        else
        {
            Debug.LogError("El nombre de la escena del menú no está asignado en el Inspector.");
        }
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        CleanupCurrencySystem();

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("No hay más niveles disponibles.");
            GoToMenu();
        }
    }

    public void IncreaseWave()
    {
        currentWave++;
        if (waveText != null)
        {
            waveText.text = $"Wave: {currentWave}";
        }
    }

    private void CleanupCurrencySystem()
    {
        if (CurrencySystem.Instance != null)
        {
            Destroy(CurrencySystem.Instance.gameObject);
        }
    }

    private void ResetCurrencySystem()
    {
        if (CurrencySystem.Instance != null)
        {
            CurrencySystem.Instance.ResetCurrency();
        }
    }

   
}
