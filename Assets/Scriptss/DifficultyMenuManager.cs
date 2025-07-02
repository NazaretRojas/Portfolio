using UnityEngine;

public class DifficultyMenuManager : MonoBehaviour
{
    public DifficultyButton[] difficultyButtons;

    private void Start()
    {
        foreach (DifficultyButton button in difficultyButtons)
        {
            button.RefreshButton();
        }
    }
}