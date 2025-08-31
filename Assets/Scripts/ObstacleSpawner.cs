using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject obstaclePrefab;
    public float xRange = 3.2f;
    public float startY = 6f;

    [Header("Pacing")]
    public float startInterval = 0.9f;
    public float minInterval = 0.28f;
    public float intervalRampPerSecond = 0.02f; // how fast we shorten interval

    [Header("Difficulty")]
    public float startSpeed = 4f;
    public float speedRampPerSecond = 0.35f;

    float nextSpawnTime;
    float currentInterval;
    float elapsed;

    void OnEnable()
    {
        elapsed = 0f;
        currentInterval = startInterval;
        nextSpawnTime = Time.time + currentInterval;
    }

    void Update()
    {
        elapsed += Time.deltaTime;

        // ramp difficulty
        currentInterval = Mathf.Max(minInterval, startInterval - intervalRampPerSecond * elapsed);

        if (Time.time >= nextSpawnTime)
        {
            SpawnOne();
            nextSpawnTime = Time.time + currentInterval;
        }
    }

    void SpawnOne()
    {
        float x = Random.Range(-xRange, xRange);
        Vector3 pos = new Vector3(x, startY, 0f);
        var go = Instantiate(obstaclePrefab, pos, Quaternion.identity);
        var o = go.GetComponent<Obstacle>();
        if (o == null) o = go.AddComponent<Obstacle>();

        o.speed = startSpeed + speedRampPerSecond * elapsed;

        // ensure collider + trigger
        var col = go.GetComponent<Collider2D>();
        if (col == null) col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

        go.tag = "Obstacle";
    }
}
