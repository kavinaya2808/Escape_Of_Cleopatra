using UnityEngine;
using UnityEngine.EventSystems;

public class IntroDialogueManager : MonoBehaviour
{
    public GameObject[] dialogues;
    private int currentIndex = 0;
    private bool dialogueStarted = false;

    public GameObject mainMenuCanvas;
    public GameObject gameplayRoot;

    public GameObject uiCamera;
    public GameObject mainCamera;

    public GameObject pauseButtonUI;
    public AudioSource introMusic;
    public AudioSource mainMusic;
    public AudioToggleButton audioToggleButton;

    void Start()
    {
        GameState.canPause = false;
        GameState.gameplayStarted = false;

        if (pauseButtonUI != null)
            pauseButtonUI.SetActive(false);

        if (GameState.isRetry)
        {
            GameState.isRetry = false;
            if (mainMenuCanvas != null) mainMenuCanvas.SetActive(false);
            if (uiCamera != null) uiCamera.SetActive(false);
            if (mainCamera != null) mainCamera.SetActive(true);
            if (gameplayRoot != null) gameplayRoot.SetActive(true);

            GameState.canPause = true;
            GameState.gameplayStarted = true;

            if (pauseButtonUI != null) pauseButtonUI.SetActive(true);
        }
        else
        {
            gameplayRoot.SetActive(false);
        }

        //  Ensure audio state is applied even if it's a retry or fresh start
        if (AudioToggleButton.Instance != null)
        {
            AudioToggleButton.Instance.RefreshAudioState();
        }
    }


    void Update()
    {
        if (!dialogueStarted) return;
        if (IsPointerOverUI()) return;

        if (Input.anyKeyDown && currentIndex < dialogues.Length - 1)
        {
            dialogues[currentIndex].SetActive(false);
            currentIndex++;
            dialogues[currentIndex].SetActive(true);
        }
        else if (Input.anyKeyDown && currentIndex == dialogues.Length - 1)
        {
            dialogues[currentIndex].SetActive(false);
            if (mainMenuCanvas != null) mainMenuCanvas.SetActive(false);
            if (gameplayRoot != null) gameplayRoot.SetActive(true);
            if (uiCamera != null) uiCamera.SetActive(false);
            if (mainCamera != null) mainCamera.SetActive(true);
            if (introMusic != null) introMusic.Stop();

            dialogueStarted = false;
            GameState.canPause = true;
            GameState.gameplayStarted = true;

            if (pauseButtonUI != null) pauseButtonUI.SetActive(true);

            AudioToggleButton.Instance.RefreshAudioState(); // Apply correct mute state to gameplay music
        }
    }

    public void StartDialogue()
    {
        if (mainMusic != null && mainMusic.isPlaying)
            mainMusic.Stop();

        dialogueStarted = true;
        currentIndex = 0;

        for (int i = 0; i < dialogues.Length; i++)
            dialogues[i].SetActive(i == 0);

        gameplayRoot.SetActive(false);
        if (uiCamera != null) uiCamera.SetActive(true);
        if (mainCamera != null) mainCamera.SetActive(false);

        GameState.canPause = false;
        GameState.gameplayStarted = false;

        if (pauseButtonUI != null) pauseButtonUI.SetActive(false);
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject.CompareTag("AudioToggle"))
                return true;
        }

        return false;
    }
}
