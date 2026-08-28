using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Spawner : MonoBehaviour
{
    public GameObject[] powerUps;
    [Header("Rare Power-Up Settings")]
    [Tooltip("Prefab Item Langka (misal Pegasus Boots)")]
    public GameObject rarePowerUpPrefab;
    [Range(0f, 1f)]
    [Tooltip("Peluang muncul item langka (misal 0.15 = 15%)")]
    public float rareSpawnChance = 0.15f;

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
        GameObject prefabToSpawn = null;

        // Cek peluang spawn item langka
        if (rarePowerUpPrefab != null && Random.value <= rareSpawnChance)
        {
            prefabToSpawn = rarePowerUpPrefab;
        }
        else if (powerUps != null && powerUps.Length > 0)
        {
            int randomIndex = Random.Range(0, powerUps.Length);
            prefabToSpawn = powerUps[randomIndex];
        }

        if (prefabToSpawn == null)
            return;

        float spawnY = transform.position.y + Random.Range(-verticalRange * 0.5f, verticalRange * 0.5f);

        Vector3 spawnPos = new Vector3(transform.position.x, spawnY, transform.position.z);

        // Anti-overlap check: Pastikan tidak ada obstacle atau powerup lain dalam radius 3 unit
        Collider2D[] nearby = Physics2D.OverlapCircleAll(spawnPos, 3f);
        foreach (Collider2D col in nearby)
        {
            if (col.CompareTag("Obstacle") || col.GetComponent<PowerUpPickup>() != null)
            {
                return; // Batalkan spawn jika terlalu berdekatan
            }
        }

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
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
