using UnityEngine;

public class ResetProgress : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Todo el progreso ha sido reiniciado.");
    }
}
