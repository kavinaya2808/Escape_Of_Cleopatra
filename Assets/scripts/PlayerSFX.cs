using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip keyPickupSFX;
    public AudioClip hurtSFX;
    public AudioClip doorUnlockSFX;
    public AudioClip jumpSFX;

    public void PlayKeyPickup()
    {
        audioSource.PlayOneShot(keyPickupSFX);
    }

    public void PlayHurt()
    {
        audioSource.PlayOneShot(hurtSFX);
    }

    public void PlayDoorUnlock()
    {
        audioSource.PlayOneShot(doorUnlockSFX);
    }

    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpSFX);
    }
}
