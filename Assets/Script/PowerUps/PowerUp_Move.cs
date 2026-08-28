using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Move : MonoBehaviour
{
    public float speed = 6f;

    [Header("Floating Effect")]
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 3f;

    private float startY;
    private float floatTimer;

    void Start()
    {
        startY = transform.position.y;
        floatTimer = Random.Range(0f, 6.28f);
    }

    void Update()
    {
        float currentSpeed = DifficultyManager.Instance != null ? DifficultyManager.Instance.CurrentSpeed : speed;
        floatTimer += Time.deltaTime * floatFrequency;

        float newX = transform.position.x - (currentSpeed * Time.deltaTime);
        float newY = startY + Mathf.Sin(floatTimer) * floatAmplitude;

        transform.position = new Vector3(newX, newY, transform.position.z);

        if (transform.position.x < -35f)
        {
            Destroy(gameObject);
        }
    }
}
