using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Spawner : MonoBehaviour
{
    public GameObject[] obstacles;
    public float spawnRate = 2f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            timer = 0;
            SpawnObstacle();
        }
    }

    private void SpawnObstacle()
    {
        int randomIndex = Random.Range(0, obstacles.Length);
        GameObject prefab = obstacles[randomIndex];

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