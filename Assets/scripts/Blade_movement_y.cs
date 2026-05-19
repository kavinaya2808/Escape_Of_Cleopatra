using UnityEngine;

public class BladeMovementy : MonoBehaviour
{
    public float speed = 2f;             // Speed of movement
    public float distance = 5f;          // Distance to move from the start position

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float move = Mathf.PingPong(Time.time * speed, distance) - (distance / 2f);
        transform.position = startPos + new Vector3(0, move, 0); // Move on X axis
    }
}
