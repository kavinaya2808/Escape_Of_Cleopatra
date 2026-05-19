using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float speed = 2f;
    public GameObject gameOverImageObject;
    public GameObject mummyPopupText;

    [Header("Chase Audio")]
    public AudioSource chaseAudio;
    public float minDistance = 1f;
    public float maxDistance = 10f;
    public float maxVolume = 1f;

    private Transform player;
    private Rigidbody2D rb;
    private CleopatraController cleopatra;
    private Vector2 initialPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();

        if (player != null)
        {
            cleopatra = player.GetComponent<CleopatraController>();
            transform.position = GetRandomSpawnPositionNearPlayer();
        }

        // Save initial position after randomized spawn (for legacy reference if needed)
        initialPosition = transform.position;

        // Disable collisions with other objects
        Collider2D[] all = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
        foreach (var col in all)
        {
            if (col.gameObject != gameObject && !col.CompareTag("Player"))
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col);
        }

        rb.gravityScale = 0;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Setup chase audio
        if (chaseAudio != null)
        {
            chaseAudio.loop = true;
            chaseAudio.volume = 0f;
            chaseAudio.Play();
        }
    }

    void Update()
    {
        // Freeze movement and mute audio when paused
        if (Time.timeScale == 0f)
        {
            if (chaseAudio != null) chaseAudio.volume = 0f;
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        // Resume audio if unpaused and not playing
        if (chaseAudio != null && !chaseAudio.isPlaying)
        {
            chaseAudio.Play();
        }

        if (player == null || cleopatra == null || rb == null || !cleopatra.hasKey)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Stop if very close
        if (Vector2.Distance(transform.position, player.position) < 0.5f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Move toward player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
        FlipSprite(direction.x);

        // Adjust volume based on proximity and mute state
        if (chaseAudio != null)
        {
            if (AudioToggleButton.Instance != null && AudioToggleButton.Instance.IsMuted())
            {
                chaseAudio.volume = 0f;
            }
            else
            {
                float distance = Vector2.Distance(transform.position, player.position);
                float t = Mathf.InverseLerp(maxDistance, minDistance, distance);
                chaseAudio.volume = Mathf.Lerp(0f, maxVolume, t);
            }
        }
    }


    public void Respawn()
    {
        // Only reposition if mummy is active (i.e., chasing)
        if (player != null && cleopatra != null && cleopatra.hasKey)
        {
            transform.position = GetRandomSpawnPositionNearPlayer();
        }
    }

    private Vector2 GetRandomSpawnPositionNearPlayer()
    {
        Vector2 cleoPos = player.position;
        float angle = Random.Range(0f, Mathf.PI); // 0 to 180 degrees
        float radius = Random.Range(4.5f, 6.5f);  // min/max distance
        float offsetX = Mathf.Cos(angle) * radius;
        float offsetY = Mathf.Sin(angle) * radius;
        return cleoPos + new Vector2(offsetX, offsetY);
    }

    private void FlipSprite(float directionX)
    {
        if (directionX != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(directionX) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    public void StopChaseAudio()
    {
        if (chaseAudio != null && chaseAudio.isPlaying)
        {
            chaseAudio.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLifeManager lifeManager = other.GetComponent<PlayerLifeManager>();
            if (lifeManager != null) lifeManager.KillInstantly();

            if (mummyPopupText != null)
                mummyPopupText.SetActive(false);

            if (gameOverImageObject != null)
                gameOverImageObject.SetActive(true);

            GameState.canPause = false;
            Time.timeScale = 0f;
        }
    }
}
