using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightRadiusReducer : MonoBehaviour
{
    public Light2D spotlight;
    public float maxRadius = 5f;
    public float minRadius = 1f;
    public float duration = 5f; // Time in seconds to go from max to min
    public GameObject gameOverUI; // Assign your GameOver UI GameObject here

    private float elapsedTime = 0f;
    private bool isGameOver = false;

    void Start()
    {
        if (spotlight == null)
        {
            spotlight = GetComponent<Light2D>();
        }

        if (spotlight == null)
        {
            Debug.LogError("Light2D component not found!");
        }
        else
        {
            spotlight.pointLightOuterRadius = maxRadius;
        }

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // Make sure it's hidden at the start
        }
    }

    void Update()
    {
        if (spotlight == null || isGameOver) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        spotlight.pointLightOuterRadius = Mathf.Lerp(maxRadius, minRadius, t);

        if (t >= 1f && !isGameOver)
        {
            isGameOver = true;
            if (gameOverUI != null)
            {
                gameOverUI.SetActive(true);
            }
            Debug.Log("Game Over!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Oil"))
        {
            ResetLightRadius();
            Destroy(other.gameObject); // This removes the oil collectible
        }
    }


    public void ResetLightRadius()
    {
        elapsedTime = 0f;
        spotlight.pointLightOuterRadius = maxRadius;
        Debug.Log("Oil collected — radius reset!");
    }
}
