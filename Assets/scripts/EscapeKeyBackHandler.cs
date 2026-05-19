using UnityEngine;

public class EscapeKeyBackHandler : MonoBehaviour
{
    public GameObject canvasInstructions;
    public GameObject canvasContinue;
    public GameObject canvasExplanation;

    public GameObject canvasMainMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvasInstructions != null && canvasInstructions.activeInHierarchy)
            {
                BackToMainMenu(canvasInstructions);
            }
            else if (canvasContinue != null && canvasContinue.activeInHierarchy)
            {
                BackToMainMenu(canvasContinue);
            }
            else if (canvasExplanation != null && canvasExplanation.activeInHierarchy)
            {
                BackToMainMenu(canvasExplanation);
            }
        }
    }

    void BackToMainMenu(GameObject activeScreen)
    {
        if (activeScreen != null)
            activeScreen.SetActive(false);

        if (canvasMainMenu != null)
            canvasMainMenu.SetActive(true);
    }
}
