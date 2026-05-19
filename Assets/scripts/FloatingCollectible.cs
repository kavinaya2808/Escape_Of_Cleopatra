using UnityEngine;

public class FloatingCollectible : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceAmplitude = 0.15f;
    public float bounceFrequency = 2f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 30f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        AnimateBounce();
        AnimateRotation();
    }

    private void AnimateBounce()
    {
        float newY = Mathf.Sin(Time.time * bounceFrequency) * bounceAmplitude;
        transform.position = new Vector3(startPos.x, startPos.y + newY, startPos.z);
    }

    private void AnimateRotation()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
