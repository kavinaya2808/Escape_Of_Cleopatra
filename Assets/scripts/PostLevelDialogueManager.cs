using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PostLevelDialogueManager : MonoBehaviour
{
    public GameObject[] dialogueScreens;
    public string nextSceneName;

    [Header("Optional UI + Audio")]
    public GameObject levelCompleteCanvas;
    public GameObject gameplayRoot;
    public GameObject uiCamera;
    public GameObject mainCamera;
    public AudioSource postDialogueMusic;
    public AudioToggleButton audioToggleButton;

    [Header("UI Controls")]
    public GameObject persistentUIButtons;

    [Header("Final Scene Options")]
    public bool showTheEnd = false;
    public GameObject theEndScreen;

    private int currentIndex = 0;
    private bool dialogueStarted = false;

    void Start()
    {
        foreach (var screen in dialogueScreens)
        {
            if (screen != null)
                screen.SetActive(false);
        }

        if (theEndScreen != null)
            theEndScreen.SetActive(false);

        if (persistentUIButtons != null)
            persistentUIButtons.SetActive(true);
    }

    void Update()
    {
        if (!dialogueStarted) return;

        if (IsPointerOverUI()) return;

        if (Input.anyKeyDown && currentIndex < dialogueScreens.Length - 1)
        {
            dialogueScreens[currentIndex].SetActive(false);
            currentIndex++;
            dialogueScreens[currentIndex].SetActive(true);
        }
        else if (Input.anyKeyDown && currentIndex == dialogueScreens.Length - 1)
        {
            dialogueScreens[currentIndex].SetActive(false);
            dialogueStarted = false;
            GameState.canPause = true;

            if (showTheEnd && theEndScreen != null)
            {
                theEndScreen.SetActive(true);
                return;
            }

            if (gameplayRoot != null) gameplayRoot.SetActive(true);
            if (uiCamera != null) uiCamera.SetActive(false);
            if (mainCamera != null) mainCamera.SetActive(true);
            if (persistentUIButtons != null) persistentUIButtons.SetActive(true);

            AudioToggleButton.Instance.RefreshAudioState();

            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        GameState.isRetry = false;
        GameState.canPause = false;

        if (persistentUIButtons != null)
            persistentUIButtons.SetActive(false);

        SceneManager.LoadScene("level-1");
    }

    public void StartPostLevelDialogue()
    {
        Debug.Log("STARTING POST DIALOGUE...");

        Time.timeScale = 1f;

        currentIndex = 0;
        dialogueStarted = true;

        GameState.canPause = false;

        for (int i = 0; i < dialogueScreens.Length; i++)
            dialogueScreens[i].SetActive(i == 0);

        if (levelCompleteCanvas != null)
            levelCompleteCanvas.SetActive(false);

        if (gameplayRoot != null)
            gameplayRoot.SetActive(false);

        if (persistentUIButtons != null)
            persistentUIButtons.SetActive(false);

        if (uiCamera != null)
            uiCamera.SetActive(true);

        if (mainCamera != null)
            mainCamera.SetActive(false);

        if (postDialogueMusic != null && !postDialogueMusic.isPlaying)
            postDialogueMusic.Play();

        // 🧼 NEW: Destroy oil pickups immediately on entering dialogue
        DestroyAllOilPickups();
    }

    private void DestroyAllOilPickups()
    {
        GameObject[] oilPickups = GameObject.FindGameObjectsWithTag("Oil");
        foreach (GameObject oil in oilPickups)
        {
            Destroy(oil);
        }
        Debug.Log($"[Cleanup] Destroyed {oilPickups.Length} leftover oil pickups.");
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

        return false;
    }
}