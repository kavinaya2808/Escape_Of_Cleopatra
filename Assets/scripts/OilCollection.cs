using UnityEngine;

public class OilCollection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Reset torch light radius
            LightRadiusReducer torch = other.GetComponentInChildren<LightRadiusReducer>();
            if (torch != null)
            {
                torch.ResetLightRadius();
                Debug.Log("Oil collected by Cleopatra!");
            }

            // 2. Play sound if available
            PlayerSFX sfx = other.GetComponent<PlayerSFX>();
            if (sfx != null)
            {
                sfx.PlayKeyPickup(); // Replace with oil-specific sound if needed
            }

            // 3. Check if Cleopatra has the key, then reset mummy
            CleopatraController cleopatra = other.GetComponent<CleopatraController>();
            if (cleopatra != null && cleopatra.hasKey)
            {
                EnemyChase mummy = FindAnyObjectByType<EnemyChase>();
                if (mummy != null)
                {
                    mummy.Respawn();
                    Debug.Log("[Oil Pickup] Mummy has been teleported back to start!");
                }
            }

            // 4. Destroy oil pickup
            Destroy(gameObject);
        }
    }
}
