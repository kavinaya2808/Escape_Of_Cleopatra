using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject instructionsCanvas;

    public void ShowInstructions()
    {
        mainMenuCanvas.SetActive(false);
        instructionsCanvas.SetActive(true);
    }

    public void BackToMenu()
    {
        instructionsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
}
