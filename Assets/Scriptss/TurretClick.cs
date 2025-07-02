using UnityEngine;

public class TurretClick : MonoBehaviour
{
    private Turret turret;
    private TurretLevelManager levelManager;

    private void Start()
    {
        turret = GetComponent<Turret>();
        levelManager = FindFirstObjectByType<TurretLevelManager>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Click detectado en torreta.");
        Debug.Log("Turret es " + (turret != null));
        Debug.Log("LevelManager es " + (levelManager != null));

        if (levelManager != null && turret != null)
        {
            levelManager.ShowPanel(turret);
            Debug.Log("Panel de nivel abierto para la torreta.");
        }
        else
        {
            Debug.LogWarning("No se encontró el TurretLevelManager o el componente Turret.");
        }
    }
}


