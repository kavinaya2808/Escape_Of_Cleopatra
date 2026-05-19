using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject explanationCanvas;
    public AudioSource mainMenuMusic;

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false);
        explanationCanvas.SetActive(true);

        if (mainMenuMusic != null)
            mainMenuMusic.Stop();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
