using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    public float speed = 0.5f;
    public float resetY = -10f;
    public float startY = 10f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        if (transform.position.y <= resetY)
        {
            var pos = transform.position;
            pos.y = startY;
            transform.position = pos;
        }
    }
}
