using UnityEngine;
using UnityEngine.UI;

public class AudioToggleButton : MonoBehaviour
{
    public Sprite audioOnSprite;
    public Sprite audioOffSprite;
    public Image iconImage;

    public static AudioToggleButton Instance;

    private bool isMuted;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Load mute preference once when the button is created
            isMuted = PlayerPrefs.GetInt("GameMuted", 0) == 1;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        ApplyMuteState(); // Apply mute state to all current AudioSources
    }

    void OnEnable()
    {
        ApplyMuteState(); // Reapply mute when object is reactivated
    }

    public void ToggleAudio()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("GameMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        ApplyMuteState();
    }

    private void ApplyMuteState()
    {
        AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (var src in sources)
        {
            if (src != null)
                src.mute = isMuted;
        }

        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (iconImage != null)
        {
            iconImage.sprite = isMuted ? audioOffSprite : audioOnSprite;
        }
    }

    /// <summary>
    /// Call this manually when new AudioSources are loaded (e.g. after retry or dialogue end)
    /// </summary>
    public void RefreshAudioState()
    {
        ApplyMuteState();
    }

    public bool IsMuted()
    {
        return isMuted;
    }
}
