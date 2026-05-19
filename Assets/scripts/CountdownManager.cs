using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    public GameObject countdownCanvas;
    public TextMeshProUGUI countdownText;

    public float freezeTimeScale = 0f;
    public float countdownDuration = 1f;  // time between each number

    private void Start()
    {
        if (countdownCanvas != null)
            countdownCanvas.SetActive(false);

        StartCoroutine(BeginCountdown());
    }

    IEnumerator BeginCountdown()
    {
        if (countdownCanvas != null)
            countdownCanvas.SetActive(true);

        Time.timeScale = freezeTimeScale; // freeze gameplay
        float realTimeDelay = countdownDuration;

        string[] countdownValues = { "3", "2", "1", "GO!" };

        foreach (string val in countdownValues)
        {
            countdownText.text = val;
            yield return new WaitForSecondsRealtime(realTimeDelay);
        }

        if (countdownCanvas != null)
            countdownCanvas.SetActive(false);

        Time.timeScale = 1f; // resume gameplay
    }
}
