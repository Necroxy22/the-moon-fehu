using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Scroller : MonoBehaviour
{
    public float speed = 6f;
    public float loopWidth = 20f;

    private float startX;
    private float defaultSpeed;

    void Start()
    {
        startX = transform.position.x;
        defaultSpeed = speed;
    }

    void Update()
    {
        float speedRatio = DifficultyManager.Instance != null ? (DifficultyManager.Instance.CurrentSpeed / DifficultyManager.Instance.baseSpeed) : 1f;
        float currentSpeed = defaultSpeed * speedRatio;

        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);

        if (transform.position.x <= startX - loopWidth)
        {
            transform.position = new Vector3(
                transform.position.x + loopWidth,
                transform.position.y,
                transform.position.z
            );
        }
    }
}