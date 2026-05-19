using System.Collections;
using UnityEngine;

public class KeyCollection : MonoBehaviour
{
    public GameObject EnemyObject;         // Mummy to activate
    public PlayerSFX playerSFX;
    public GameObject mummyChaseMessage;   // Assign the popup message in Inspector

    void Start()
    {
        // Ensure the mummy message is hidden at the beginning
        if (mummyChaseMessage != null)
            mummyChaseMessage.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(HandlePickupSequence(other));
        }
    }

    IEnumerator HandlePickupSequence(Collider2D other)
    {
        // 1. Set hasKey on CleopatraController
        CleopatraController controller = other.GetComponent<CleopatraController>();
        if (controller != null)
        {
            controller.hasKey = true;
            Debug.Log("Cleopatra picked up the key!");
        }

        // 1b. Also call PickupKey() to update sprites
        PlayerMovement movement = other.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.PickupKey(); // 🔑 This updates the sprite set!
        }

        // 2. Play pickup sound
        if (playerSFX != null)
            playerSFX.PlayKeyPickup();

        // 3. Enable mummy
        if (EnemyObject != null)
        {
            EnemyObject.SetActive(true);
        }

        // 4. Hide the key's sprite and collider
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // 5. Show mummy message
        if (mummyChaseMessage != null)
        {
            mummyChaseMessage.SetActive(true);
            yield return new WaitForSeconds(3f);
            mummyChaseMessage.SetActive(false);
        }

        // 6. Destroy the key object
        Destroy(gameObject);
    }



}
