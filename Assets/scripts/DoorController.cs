using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public Sprite doorClosedSprite;
    public Sprite doorOpenedSprite;
    public GameObject levelCompleteCanvas;
    public PlayerSFX playerSFX;

    private SpriteRenderer spriteRenderer;
    public EnemyChase enemy;
    private bool isUnlocked = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = doorClosedSprite;

        if (levelCompleteCanvas != null)
            levelCompleteCanvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CleopatraController cleopatra = other.GetComponent<CleopatraController>();

            if (cleopatra != null && cleopatra.hasKey)
            {
                UnlockDoor();

                if (playerSFX != null)
                {
                    playerSFX.PlayDoorUnlock();
                }

                if (enemy != null)
                {
                    enemy.StopChaseAudio();
                }

                StartCoroutine(ShowLevelCompleteCanvasAfterDelay());
            }
        }
    }

    void UnlockDoor()
    {
        isUnlocked = true;
        spriteRenderer.sprite = doorOpenedSprite;
        Debug.Log("Door Unlocked!");

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "level-1")
        {
            GameState.level2Unlocked = true;
            Debug.Log("Level 2 Unlocked!");
        }
        else if (currentScene == "level-2")
        {
            GameState.level3Unlocked = true;
            Debug.Log("Level 3 Unlocked!");
        }
    }


    private IEnumerator ShowLevelCompleteCanvasAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (levelCompleteCanvas != null)
        {
            levelCompleteCanvas.SetActive(true);
            Debug.Log("Level Complete Canvas Shown");
        }

        // Hide lives panel on level complete
        PlayerLifeManager lifeManager = Object.FindFirstObjectByType<PlayerLifeManager>();
        if (lifeManager != null)
        {
            lifeManager.OnLevelCompleted();
        }

        // Disable pausing during level complete
        GameState.canPause = false;

        // Pause gameplay
        Time.timeScale = 0f;
    }
}
