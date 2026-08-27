using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [Header("Speed Scaling")]
    [Tooltip("Kecepatan dasar semua obstacle dan background")]
    public float baseSpeed = 6f;
    [Tooltip("Peningkatan kecepatan per detik")]
    public float speedIncreasePerSecond = 0.05f;
    [Tooltip("Batas kecepatan maksimal")]
    public float maxSpeed = 14f;

    [Header("Spawn Rate Scaling")]
    [Tooltip("Jeda spawn obstacle awal (detik)")]
    public float baseObstacleSpawnRate = 2.5f;
    [Tooltip("Jeda spawn obstacle tercepat")]
    public float minObstacleSpawnRate = 1.1f;
    [Tooltip("Pengurangan jeda spawn per detik")]
    public float spawnRateDecreasePerSecond = 0.015f;

    public float CurrentSpeed { get; private set; }
    public float CurrentObstacleSpawnRate { get; private set; }
    public float GameTime { get; private set; }
    public int CurrentLevel { get; private set; } = 1;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        ResetDifficulty();
    }

    void Update()
    {
        GameTime += Time.deltaTime;

        CurrentSpeed = Mathf.Min(baseSpeed + (GameTime * speedIncreasePerSecond), maxSpeed);

        CurrentObstacleSpawnRate = Mathf.Max(baseObstacleSpawnRate - (GameTime * spawnRateDecreasePerSecond), minObstacleSpawnRate);

        CurrentLevel = 1 + (int)(GameTime / 30f);
    }

    public void ResetDifficulty()
    {
        GameTime = 0f;
        CurrentSpeed = baseSpeed;
        CurrentObstacleSpawnRate = baseObstacleSpawnRate;
        CurrentLevel = 1;
    }
}
