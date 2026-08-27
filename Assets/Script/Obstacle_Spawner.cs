using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Spawner : MonoBehaviour
{
    public GameObject[] obstacles;
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

        float bottomOffset = GetBottomOffset(prefab);

        Vector3 spawnPos = new Vector3(
            transform.position.x,
            transform.position.y + bottomOffset,
            transform.position.z
        );

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