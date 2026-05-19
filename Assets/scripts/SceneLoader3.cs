using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader3 : MonoBehaviour
{
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("level-3");
    }
}
