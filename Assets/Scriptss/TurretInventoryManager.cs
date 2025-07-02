using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SerializableList
{
    public List<string> list;

    public SerializableList(List<string> list)
    {
        this.list = list;
    }
}

public class TurretInventoryManager : MonoBehaviour
{
    public static TurretInventoryManager Instance { get;  set; }
    private const int MaxEquippedTurrets = 6;

    

    [SerializeField] private List<string> startingTurrets = new List<string> { "Turret1", "Turret2", "Turret5" };
    [SerializeField] private Transform inventoryUIContainer;
    [SerializeField] private GameObject turretSlotPrefab;

    [SerializeField] private GameObject warningPanel;

    [SerializeField] private TurretSettings[] allTurretSettings;

    private List<string> unlockedTurrets = new List<string>();
    private List<string> equippedTurrets = new List<string>();

    private const string EquippedKey = "EquippedTurrets";
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        LoadInventory();

    }

    private void Start()
    {
        if (warningPanel != null)
            warningPanel.SetActive(false);

        RefreshInventoryUI();
    }

    public List<string> GetUnlockedTurrets()
    {
        return unlockedTurrets;
    }
    public void AddToInventory(string turretID)
    {
        if (!unlockedTurrets.Contains(turretID))
        {
            unlockedTurrets.Add(turretID);
            SaveInventory();
            RefreshInventoryUI();
        }
        Debug.Log($"Torreta agregada al inventario: {turretID}");
    }

    public void ToggleEquipTurret(string turretID)
    {
        if (equippedTurrets.Contains(turretID))
        {
            equippedTurrets.Remove(turretID);
        }
        else
        {
            if (equippedTurrets.Count >= MaxEquippedTurrets)
            {
                ShowWarningPanel();
                return;
            }
            equippedTurrets.Add(turretID);
        }

        SaveInventory(); 
        RefreshInventoryUI();
    }

    private void ShowWarningPanel()
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(true);
            CancelInvoke(nameof(HideWarningPanel));
            Invoke(nameof(HideWarningPanel), 2f);
        }
    }

    private void HideWarningPanel()
    {
        if (warningPanel != null)
            warningPanel.SetActive(false);
    }

    private TurretSettings GetSettingsByID(string id)
    {
        foreach (var setting in allTurretSettings)
        {
            if (setting.ID == id)
                return setting;
        }
        return null;
    }

    private void RefreshInventoryUI()
    {
        int inventorySlotLayer = LayerMask.NameToLayer("InventorySlotLayer");

        
        foreach (Transform child in inventoryUIContainer)
        {
            if (child == null) continue;
            if (child.gameObject.layer == inventorySlotLayer)
            {
                Destroy(child.gameObject);
            }
        }

        
        foreach (string turretID in unlockedTurrets)
        {
            GameObject slot = Instantiate(turretSlotPrefab, inventoryUIContainer);
            slot.layer = inventorySlotLayer;

            TurretSettings settings = GetSettingsByID(turretID);

            if (settings != null)
            {
                Transform turretImageTransform = slot.transform.Find("Image/TurretImage");
                if (turretImageTransform != null)
                {
                    Image img = turretImageTransform.GetComponent<Image>();
                    if (img != null)
                        img.sprite = settings.TurretShopSprite;
                }
                else
                {
                    Debug.LogError("TurretImage no encontrado. Revisa la jerarquía del prefab.");
                }

                Transform nameTransform = slot.transform.Find("Image/TurretName");
                if (nameTransform != null)
                {
                    var nameText = nameTransform.GetComponent<TextMeshProUGUI>();
                    if (nameText != null)
                        nameText.text = turretID;
                }

                Transform equipButtonTransform = slot.transform.Find("Image/EquipButton");
                if (equipButtonTransform != null)
                {
                    Button equipBtn = equipButtonTransform.GetComponent<Button>();
                    TextMeshProUGUI btnText = equipButtonTransform.GetComponentInChildren<TextMeshProUGUI>();
                    if (equipBtn != null && btnText != null)
                    {
                        btnText.text = equippedTurrets.Contains(turretID) ? "Unequip" : "Equip";
                        equipBtn.onClick.RemoveAllListeners();
                        equipBtn.onClick.AddListener(() => ToggleEquipTurret(turretID));
                    }
                }
            }
        }
    }

    public List<string> GetEquippedTurrets()
    {
        return equippedTurrets;
    }

    private void SaveInventory()
    {
        string unlockedJson = JsonUtility.ToJson(new SerializableList(unlockedTurrets));
        PlayerPrefs.SetString("UnlockedTurrets", unlockedJson);

        string equippedJson = JsonUtility.ToJson(new SerializableList(equippedTurrets));
        PlayerPrefs.SetString(EquippedKey, equippedJson);

        PlayerPrefs.Save();
    }

    private void LoadInventory()
    {
        if (PlayerPrefs.HasKey("UnlockedTurrets"))
        {
            string unlockedJson = PlayerPrefs.GetString("UnlockedTurrets");
            unlockedTurrets = JsonUtility.FromJson<SerializableList>(unlockedJson).list;
        }
        else
        {
            unlockedTurrets = new List<string>(startingTurrets);
        }

        if (PlayerPrefs.HasKey(EquippedKey))
        {
            string equippedJson = PlayerPrefs.GetString(EquippedKey);
            equippedTurrets = JsonUtility.FromJson<SerializableList>(equippedJson).list;
        }
        else
        {
            equippedTurrets = new List<string>(startingTurrets);
        }
    }
}