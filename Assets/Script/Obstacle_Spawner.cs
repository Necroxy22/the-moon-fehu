using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Spawner : MonoBehaviour
{
    public GameObject[] obstacles;
    [Tooltip("Offset naik/turun obstacle (misal -0.5 untuk lebih ke bawah, 0.5 untuk lebih ke atas)")]
    public float yOffset = 0f;
    public float spawnRate = 2.5f;
    private float timer;

    private int lastSpawnedIndex = -1;
    private int consecutiveCount = 0;

    void Update()
    {
        timer += Time.deltaTime;

        float currentSpawnRate = DifficultyManager.Instance != null 
            ? DifficultyManager.Instance.CurrentObstacleSpawnRate 
            : spawnRate;

        if (timer >= currentSpawnRate)
        {
            timer = 0;
            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        if (obstacles == null || obstacles.Length == 0)
            return;

        int randomIndex = Random.Range(0, obstacles.Length);

        if (randomIndex == lastSpawnedIndex)
        {
            consecutiveCount++;
            if (consecutiveCount >= 2 && obstacles.Length > 1)
            {
                randomIndex = (randomIndex + 1) % obstacles.Length;
                consecutiveCount = 1;
            }
        }
        else
        {
            lastSpawnedIndex = randomIndex;
            consecutiveCount = 1;
        }

        GameObject prefab = obstacles[randomIndex];

        if (prefab == null)
            return;

        Vector3 spawnPos = new Vector3(
            transform.position.x,
            transform.position.y + yOffset,
            transform.position.z
        );

        // Anti-overlap check: Pastikan tidak ada obstacle atau powerup lain dalam radius 3 unit
        Collider2D[] nearby = Physics2D.OverlapCircleAll(spawnPos, 3f);
        foreach (Collider2D col in nearby)
        {
            if (col.CompareTag("Obstacle") || col.GetComponent<PowerUpPickup>() != null)
            {
                return; // Batalkan spawn jika terlalu berdekatan
            }
        }

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    private float GetBottomOffset(GameObject prefab)
    {
        SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
            return 0f;

        return -sr.bounds.min.y;
    }
}