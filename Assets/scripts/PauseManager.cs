using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseCanvas;       // assign Canvas_game_paused
    public GameObject pauseButtonUI;     // assign the top-right UI pause button
    public GameObject mainMenuCanvas;    // assign Main Menu Canvas here!

    private bool isPaused = false;
    private float pauseCooldownTimer = 0f;
    private const float pauseCooldownDuration = 0.25f;

    private bool buttonActivated = false;

    void Update()
    {
        if (pauseCooldownTimer > 0f)
        {
            pauseCooldownTimer -= Time.unscaledDeltaTime;
            return;
        }

        // If the menu is active, make sure the button stays hidden
        if (mainMenuCanvas != null && mainMenuCanvas.activeInHierarchy)
        {
            if (pauseButtonUI != null)
                pauseButtonUI.SetActive(false);
            return;
        }

        // One-time activation when gameplay has started
        if (!buttonActivated && GameState.gameplayStarted)
        {
            if (pauseButtonUI != null)
            {
                pauseButtonUI.SetActive(true);
                Debug.Log("Pause button activated during gameplay start.");
            }

            buttonActivated = true;
        }

        if (!GameState.canPause) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }


    public void TogglePause()
    {
        if (pauseCooldownTimer > 0f) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pauseCanvas != null) pauseCanvas.SetActive(true);
            if (pauseButtonUI != null) pauseButtonUI.SetActive(false);
        }
        else
        {
            StartCoroutine(ResumeAfterFrame());
        }

        EventSystem.current.SetSelectedGameObject(null);
        pauseCooldownTimer = pauseCooldownDuration;
    }

    private IEnumerator ResumeAfterFrame()
    {
        yield return new WaitForEndOfFrame();

        Time.timeScale = 1f;
        if (pauseCanvas != null) pauseCanvas.SetActive(false);

        // Only enable pause button if main menu is not visible
        if (pauseButtonUI != null && (mainMenuCanvas == null || !mainMenuCanvas.activeInHierarchy))
            pauseButtonUI.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        // Reset hover visuals
        var pointer = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);
        foreach (var result in raycastResults)
        {
            var hover = result.gameObject.GetComponent<ButtonHoverEffect>();
            if (hover != null)
                hover.ForceResetVisual();
        }

        isPaused = false;
        pauseCooldownTimer = pauseCooldownDuration;
    }

    public void QuitToMainMenu()
    {
        GameState.isRetry = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("level-1");
    }
}
