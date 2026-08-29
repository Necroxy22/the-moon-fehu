using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 6f;
    [Tooltip("Offset ketinggian khusus untuk prefab ini jika perlu disesuaikan (misal -0.5 untuk menurunkan batu)")]
    public float customYOffset = 0f;

    void Update()
    {
        float currentSpeed = DifficultyManager.Instance != null ? DifficultyManager.Instance.CurrentSpeed : speed;
        transform.position += Vector3.left * (currentSpeed * Time.deltaTime);

        if (transform.position.x < -35f)
        {
            Destroy(gameObject);
        }
    }
}