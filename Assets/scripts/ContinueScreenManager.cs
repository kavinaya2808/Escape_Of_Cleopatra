using UnityEngine;

public class ContinueScreenManager : MonoBehaviour
{
    public GameObject continueScreen1; // Only level 1 unlocked
    public GameObject continueScreen2; // Levels 1 & 2 unlocked
    public GameObject continueScreen3; // All 3 levels unlocked

    void OnEnable()
    {
        // Hide all screens first
        continueScreen1.SetActive(false);
        continueScreen2.SetActive(false);
        continueScreen3.SetActive(false);

        // Show the correct screen based on progress
        if (GameState.level3Unlocked)
        {
            continueScreen3.SetActive(true);
        }
        else if (GameState.level2Unlocked)
        {
            continueScreen2.SetActive(true);
        }
        else
        {
            continueScreen1.SetActive(true);
        }
    }
}
