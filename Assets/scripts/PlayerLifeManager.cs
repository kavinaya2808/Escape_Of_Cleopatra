using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerLifeManager : MonoBehaviour
{
    public int lives = 3;

    public Image screenFlash;
    public Image[] heartImages;

    public Sprite redHeart;
    public Sprite blackHeart;
    public PlayerSFX playerSFX;

    public GameObject gameOverImageObject;
    public GameObject canvasLives; 

    private Vector2 lastSafePosition;
    private Rigidbody2D rb;
    private bool isInvincible = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateLivesDisplay();

        if (screenFlash != null)
            screenFlash.color = new Color(0, 0, 0, 0);

        if (gameOverImageObject != null)
            gameOverImageObject.SetActive(false);

        if (canvasLives != null)
            canvasLives.SetActive(true); 

        lastSafePosition = transform.position;
    }

    void Update()
    {
        if (!IsTouchingTrap())
        {
            lastSafePosition = transform.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isInvincible)
        {
            LoseLifeAndReposition(collision);
        }
    }

    void LoseLifeAndReposition(Collision2D collision)
    {
        lives--;
        Debug.Log("Life lost! Lives left: " + lives);
        UpdateLivesDisplay();

        StartCoroutine(FlashScreen());

        if (playerSFX != null)
        {
            playerSFX.PlayHurt();
        }

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            RepositionAwayFromTrap(collision);
            StartCoroutine(TempInvincibility());
        }
    }

    void RepositionAwayFromTrap(Collision2D collision)
    {
        Vector2 trapPos = collision.transform.position;
        Vector2 directionAway = ((Vector2)transform.position - trapPos).normalized;

        if (trapPos.y > transform.position.y)
        {
            directionAway += Vector2.down;
        }
        else if (trapPos.y < transform.position.y)
        {
            directionAway += (transform.position.x - trapPos.x >= 0) ? Vector2.right : Vector2.left;
        }

        directionAway = directionAway.normalized;

        Vector2 newPosition = (Vector2)transform.position + directionAway * 1.5f;

        Vector2 checkPosition = new Vector2(newPosition.x, transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(checkPosition, Vector2.down, 5f, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            newPosition.y = hit.point.y + 0.5f;
        }
        else
        {
            Debug.LogWarning("Ground not found when repositioning! Using fallback Y.");
        }

        transform.position = newPosition;
        rb.linearVelocity = Vector2.zero;

        Debug.Log("Repositioned Cleopatra away from trap.");
    }

    IEnumerator TempInvincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(1f);
        isInvincible = false;
    }

    IEnumerator FlashScreen()
    {
        if (screenFlash != null)
        {
            screenFlash.color = new Color(0, 0, 0, 0.8f);
            yield return new WaitForSeconds(0.2f);
            screenFlash.color = new Color(0, 0, 0, 0);
        }
    }

    void UpdateLivesDisplay()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < lives ? redHeart : blackHeart;
        }
    }

    public void RetryGame()
    {
        GameState.isRetry = true;
        Time.timeScale = 1f;
        GameState.canPause = true;

        if (canvasLives != null)
            canvasLives.SetActive(true); 

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StartCoroutine(DelayedAudioRefresh());
    }

    private IEnumerator DelayedAudioRefresh()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (AudioToggleButton.Instance != null)
            AudioToggleButton.Instance.RefreshAudioState();
    }

    public void KillInstantly()
    {
        lives = 0;
        UpdateLivesDisplay();
        GameOver();
    }

    public void QuitToMainMenu()
    {
        GameState.isRetry = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("level-1");
    }

    void GameOver()
    {
        Debug.Log("Game Over!");

        if (canvasLives != null)
            canvasLives.SetActive(false); 

        if (gameOverImageObject != null)
            gameOverImageObject.SetActive(true);

        Time.timeScale = 0f;
        GameState.canPause = false;
    }

    public void OnLevelCompleted()
    {
        Debug.Log("Level Completed!");

        if (canvasLives != null)
            canvasLives.SetActive(false);
    }

    bool IsTouchingTrap()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        foreach (Collider2D col in hits)
        {
            if (col.CompareTag("Obstacle"))
                return true;
        }
        return false;
    }
}
