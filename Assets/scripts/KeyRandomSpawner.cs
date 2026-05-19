using UnityEngine;

public class KeyRandomSpawner : MonoBehaviour
{
    public GameObject keyPrefab;                 // Prefab to spawn
    public GameObject cleopatra;                 // Player reference
    public float keyHeightOffset = 1.5f;
    public float minDistanceFromCleopatra = 4f;
    public float exclusionRadius = 2f;
    public float trapAvoidanceRadius = 1f;

    private Collider2D keyCollider;
    private GameObject spawnedKey;

    private KeyCollection keyReference;

    void Start()
    {
        if (keyPrefab == null || cleopatra == null)
        {
            Debug.LogWarning("Missing keyPrefab or cleopatra reference!");
            return;
        }

        keyReference = GetComponent<KeyCollection>();
        if (keyReference == null)
        {
            Debug.LogError("KeyCollection script missing on KeySpawner!");
            return;
        }

        Vector3 cleopatraSpawnPoint = cleopatra.transform.position;
        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Ground");

        int safetyCounter = 100;

        while (safetyCounter-- > 0)
        {
            GameObject ground = groundObjects[Random.Range(0, groundObjects.Length)];
            float distance = Vector3.Distance(ground.transform.position, cleopatraSpawnPoint);
            if (distance < (minDistanceFromCleopatra + exclusionRadius)) continue;

            Collider2D groundCollider = ground.GetComponent<Collider2D>();
            if (groundCollider == null)
            {
                Debug.Log($"Skipping ground {ground.name} — no collider.");
                continue;
            }

            Vector3 spawnPos = groundCollider.bounds.center;
            spawnPos.y = groundCollider.bounds.max.y + keyHeightOffset;

            Collider2D[] overlaps = Physics2D.OverlapCircleAll(spawnPos, trapAvoidanceRadius);
            bool invalid = false;
            foreach (var col in overlaps)
            {
                if (col.CompareTag("Obstacle") || col.CompareTag("Wall") || col.CompareTag("Untagged"))
                {
                    invalid = true;
                    break;
                }
            }

            if (invalid) continue;

            // Valid position found — spawn
            spawnedKey = Instantiate(keyPrefab, spawnPos, Quaternion.identity);
            SetupSpawnedKey();
            Debug.Log($"Key spawned at {spawnPos}");
            return;
        }

        //  Fallback if no valid ground was found
        Debug.LogWarning(" Could not find a valid key spawn location after 100 tries. Using fallback above Cleopatra.");
        Vector3 fallback = cleopatra.transform.position + new Vector3(2f, 1.5f, 0f);
        spawnedKey = Instantiate(keyPrefab, fallback, Quaternion.identity);
        SetupSpawnedKey();
    }

    void SetupSpawnedKey()
    {
        // Set key collider with slight delay
        keyCollider = spawnedKey.GetComponent<Collider2D>();
        if (keyCollider != null) keyCollider.enabled = false;
        Invoke(nameof(EnableColliderSafely), 0.2f);

        // Inject references into KeyCollection
        KeyCollection kc = spawnedKey.GetComponent<KeyCollection>();
        if (kc != null)
        {
            kc.EnemyObject = keyReference.EnemyObject;
            kc.playerSFX = keyReference.playerSFX;
            kc.mummyChaseMessage = keyReference.mummyChaseMessage;
            Debug.Log("Scene references assigned to spawned KeyCollection.");
        }

        // Inject key into oil spawner
        TorchOilSpawner oilSpawner = Object.FindFirstObjectByType<TorchOilSpawner>();
        if (oilSpawner != null)
        {
            oilSpawner.keyObject = spawnedKey;
            Debug.Log("Injected spawned key into TorchOilSpawner.");
        }
    }

    void EnableColliderSafely()
    {
        if (keyCollider != null)
        {
            keyCollider.enabled = true;
            Debug.Log($"Key collider enabled at {spawnedKey.transform.position}");
        }
    }
}
