using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 4f;
    public float killY = -6f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        if (transform.position.y < killY) Destroy(gameObject);
    }
}
