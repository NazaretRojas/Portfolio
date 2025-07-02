using UnityEngine;

public class TestPanel : MonoBehaviour
{
    public GameObject testPanel;

    public void ShowPanel()
    {
        if (testPanel != null)
        {
            Debug.Log("[TEST] Activando panel manualmente");
            testPanel.SetActive(true);
        }
    }
}
