using UnityEngine;

public class ExplanationScreenManager : MonoBehaviour
{
    public GameObject explanationCanvas;
    public GameObject mainMenuCanvas;
    public AudioSource introMusic;
    public IntroDialogueManager introDialogueManager;

    public void OnContinueClicked()
    {
        explanationCanvas.SetActive(false);
        introDialogueManager.StartDialogue();
    }

    public void OnBackToMainMenuClicked()
    {
        explanationCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);

        if (introMusic != null && !introMusic.isPlaying)
            introMusic.Play();

        // Make sure audio mute state is respected when returning
        if (AudioToggleButton.Instance != null)
            AudioToggleButton.Instance.RefreshAudioState();
    }
}
