using UnityEngine;

public class GameplayInitializer : MonoBehaviour
{
    void Start()
    {
        GameState.canPause = true;
        GameState.gameplayStarted = true;
        Debug.Log("GameplayInitializer: GameState set for " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
