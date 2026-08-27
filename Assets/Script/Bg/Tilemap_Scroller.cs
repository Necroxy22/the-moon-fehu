using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap_Scroller : MonoBehaviour
{
    public float speed = 6f;
    public float resetPositionX = -20f;
    public float loopWidth = 20f;

    void Update()
    {
        float currentSpeed = DifficultyManager.Instance != null ? DifficultyManager.Instance.CurrentSpeed : speed;
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);

        if (transform.position.x <= resetPositionX)
        {
            transform.position = new Vector3(
                transform.position.x + loopWidth,
                transform.position.y,
                transform.position.z
            );
        }
    }
}
