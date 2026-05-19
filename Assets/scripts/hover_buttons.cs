using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject visualScaler;

    [Header("Scale Settings")]
    public float hoverScale = 1.05f;
    public float clickScale = 0.95f;

    [Header("Sound Settings")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private Vector3 originalScale;
    private AudioSource audioSource;
    private bool isPointerOver = false;

    private void Start()
    {
        if (visualScaler == null) visualScaler = this.gameObject;
        originalScale = visualScaler.transform.localScale;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        visualScaler.transform.localScale = originalScale * hoverScale;

        if (hoverSound != null && !AudioToggleButton.Instance.IsMuted())
            audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        visualScaler.transform.localScale = originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        visualScaler.transform.localScale = originalScale * clickScale;

        if (clickSound != null && !AudioToggleButton.Instance.IsMuted())
            audioSource.PlayOneShot(clickSound);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        visualScaler.transform.localScale = isPointerOver
            ? originalScale * hoverScale
            : originalScale;
    }

    public void ForceResetVisual()
    {
        isPointerOver = false;
        visualScaler.transform.localScale = originalScale;
    }

    private void OnDisable()
    {
        if (visualScaler != null)
            visualScaler.transform.localScale = originalScale;

        isPointerOver = false;
    }
}
