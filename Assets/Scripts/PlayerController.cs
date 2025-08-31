using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 9f;
    public float xClamp = 3.2f;        // will be recalculated in Start()
    public bool followFinger = false;  // if true, move toward finger x; else use drag delta

    Camera cam;
    Vector2 lastTouchWorld;
    bool dragging;
    float targetX;                     // smoothed target for keyboard/touch

    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        // compute clamps based on camera size & player width
        float halfWidth = cam.orthographicSize * cam.aspect;
        float halfPlayer = GetComponent<SpriteRenderer>() ? GetComponent<SpriteRenderer>().bounds.extents.x : 0.6f;
        xClamp = Mathf.Max(0.5f, halfWidth - halfPlayer);
        targetX = transform.position.x;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        // works if Active Input Handling = Both or Old; set in Project Settings → Player
        float axis = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(axis) > 0.001f)
            targetX = Mathf.Clamp(transform.position.x + axis * speed * Time.deltaTime, -xClamp, xClamp);
#else
        HandleTouch(); // sets targetX
#endif
        // smooth toward target
        var pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, targetX, 0.25f); // 0.15–0.35 feels good
        transform.position = pos;
    }

    void HandleTouch()
    {
        if (Input.touchCount == 0) { dragging = false; return; }
        var t = Input.GetTouch(0);
        var world = cam.ScreenToWorldPoint(t.position);

        if (t.phase == TouchPhase.Began)
        {
            lastTouchWorld = world;
            dragging = true;
        }
        else if (t.phase == TouchPhase.Moved && dragging)
        {
            if (followFinger)
                targetX = Mathf.Clamp(world.x, -xClamp, xClamp);
            else
            {
                float dx = world.x - lastTouchWorld.x;
                targetX = Mathf.Clamp(transform.position.x + dx, -xClamp, xClamp);
                lastTouchWorld = world;
            }
        }
        else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
        {
            dragging = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
            FindObjectOfType<GameManager>()?.GameOver();
    }
}
