using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Spawner : MonoBehaviour
{
    public GameObject[] powerUps;
    public float spawnRate = 6f;
    private float timer;

    public GameObject[] obstacleReference;
    public float groundY = 0f;
    public float clearance = 1f;
    public float verticalRange = 1f;

    private float tallestObstacleTop = 0f;

    void Start()
    {
        tallestObstacleTop = CalculateTallestObstacleTop();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            timer = 0;
            TrySpawnPowerUp();
        }
    }

    private void TrySpawnPowerUp()
    {
        if (powerUps == null || powerUps.Length == 0)
            return;

        int randomIndex = Random.Range(0, powerUps.Length);

        float baseHeight = groundY + tallestObstacleTop + clearance;
        float extraHeight = Random.Range(0f, verticalRange);
        float spawnY = baseHeight + extraHeight;

        Vector3 spawnPos = new Vector3(transform.position.x, spawnY, transform.position.z);

        Instantiate(powerUps[randomIndex], spawnPos, Quaternion.identity);
    }

    private float CalculateTallestObstacleTop()
    {
        float tallestTop = 0f;

        if (obstacleReference == null)
            return tallestTop;

        foreach (GameObject prefab in obstacleReference)
        {
            if (prefab == null)
                continue;

            SpriteRenderer sr = prefab.GetComponentInChildren<SpriteRenderer>();
            if (sr == null)
                continue;
                
            float spriteHeight = sr.bounds.max.y - sr.bounds.min.y;

            if (spriteHeight > tallestTop)
                tallestTop = spriteHeight;
        }

        return tallestTop;
    }
}
