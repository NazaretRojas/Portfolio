using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthContainer : MonoBehaviour
{
    [SerializeField] private Image fillAmountImage;
    public Image FillAmountImage => fillAmountImage;

    private void Awake()
    {
        if (fillAmountImage == null)
            Debug.LogWarning($"[EnemyHealthContainer] No hay Image asignada en {gameObject.name}");
    }
}
