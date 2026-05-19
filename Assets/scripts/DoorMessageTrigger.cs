using UnityEngine;

public class DoorMessageTrigger : MonoBehaviour
{
    public GameObject messageUI;          // Drag the NeedKeyMessage Text GameObject here
    public CleopatraController cleopatra; // Drag the player GameObject that has this script

    private void Start()
    {
        if (messageUI != null)
            messageUI.SetActive(false); // Hide by default
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && cleopatra != null && !cleopatra.hasKey)
        {
            messageUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && cleopatra != null && !cleopatra.hasKey)
        {
            messageUI.SetActive(false);
        }
    }
}
