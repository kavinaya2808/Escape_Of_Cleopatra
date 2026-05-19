using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButtonActions : MonoBehaviour
{
    public GameObject canvasMainMenu;
    public GameObject canvasContinueStory;

    // Called from the Continue button on the main menu
    public void ShowContinueScreen()
    {
        if (canvasMainMenu != null)
            canvasMainMenu.SetActive(false);

        if (canvasContinueStory != null)
            canvasContinueStory.SetActive(true);

        Debug.Log("Continue screen shown");
    }

    public void LoadLevel1()
    {
        GameState.isRetry = true; // Skips intro dialogue
        SceneManager.LoadScene("level-1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("level-2");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("level-3");
    }

    // Called from the back button on any continue screen
    public void BackToMainMenu()
    {
        if (canvasContinueStory != null)
            canvasContinueStory.SetActive(false);

        if (canvasMainMenu != null)
            canvasMainMenu.SetActive(true);

        Debug.Log("Back button clicked – returned to Main Menu");
    }
}