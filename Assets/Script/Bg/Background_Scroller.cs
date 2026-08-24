using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Scroller : MonoBehaviour
{
    public float speed = 6f;
    public float loopWidth = 20f;

    private float startX;

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x <= startX - loopWidth)
        {
            transform.position = new Vector3(
                startX,
                transform.position.y,
                transform.position.z
            );
        }
    }
}