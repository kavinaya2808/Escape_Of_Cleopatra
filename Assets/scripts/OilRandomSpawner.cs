using UnityEngine;
using System.Collections;

public class TorchOilSpawner : MonoBehaviour
{
    public GameObject oilPrefab;
    public GameObject cleopatra;
    public GameObject keyObject;
    public int numberOfOils = 2;
    public float keyHeightOffset = 1.5f;
    public float minDistanceFromCleopatra = 3f;
    public float exclusionRadius = 2f;
    public float overlapCheckRadius = 0.5f;

    private GameObject oilContainer;

    IEnumerator Start()
    {
        // Wait until keyObject is injected or timeout hits
        float timeout = 2f;
        while (keyObject == null && timeout > 0f)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }

        if (oilPrefab == null || cleopatra == null || keyObject == null)
        {
            Debug.LogWarning("Missing required references (oilPrefab, cleopatra, or keyObject)");
            yield break;
        }

        // === Create oil parent container
        oilContainer = new GameObject("OilContainer");

        Vector3 cleopatraSpawn = cleopatra.transform.position;
        Vector3 keyPosition = keyObject.transform.position;
        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Ground");

        int spawnedCount = 0;
        int safetyCounter = 200;

        while (spawnedCount < numberOfOils && safetyCounter > 0)
        {
            safetyCounter--;

            GameObject ground = groundObjects[Random.Range(0, groundObjects.Length)];
            float distance = Vector3.Distance(ground.transform.position, cleopatraSpawn);
            if (distance < (minDistanceFromCleopatra + exclusionRadius)) continue;

            Collider2D groundCollider = ground.GetComponent<Collider2D>();
            if (groundCollider == null) continue;

            Vector3 spawnPos = groundCollider.bounds.center;
            spawnPos.y = groundCollider.bounds.max.y + keyHeightOffset;

            if (Vector3.Distance(spawnPos, keyPosition) < 1.5f) continue;

            Collider2D[] overlaps = Physics2D.OverlapCircleAll(spawnPos, overlapCheckRadius);
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

            GameObject oil = Instantiate(oilPrefab, spawnPos, Quaternion.identity);
            oil.transform.parent = oilContainer.transform;
            Debug.Log($"🟢 Oil #{spawnedCount + 1} spawned at {spawnPos}");
            spawnedCount++;
        }

        if (spawnedCount < numberOfOils)
        {
            Debug.LogWarning($"Only spawned {spawnedCount} oils out of {numberOfOils} after safety limit.");
        }
    }

    public void DestroyAllOils()
    {
        if (oilContainer != null)
        {
            Destroy(oilContainer);
            Debug.Log("Destroyed all oil pickups.");
        }
    }
}
