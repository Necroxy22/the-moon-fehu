using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 6f;

    void Update()
    {
        float currentSpeed = DifficultyManager.Instance != null ? DifficultyManager.Instance.CurrentSpeed : speed;
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);

        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }
}