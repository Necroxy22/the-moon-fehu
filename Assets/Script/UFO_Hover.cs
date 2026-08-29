using UnityEngine;

/// <summary>
/// Bikin UFO naik-turun (hover) sambil tetap bergerak ke kiri via Obstacle.cs.
/// Pasang script ini ke UFO.prefab.
/// </summary>
public class UFO_Hover : MonoBehaviour
{
    [Tooltip("Seberapa tinggi naik-turunnya (0.3 = cukup keliatan)")]
    public float hoverAmplitude = 0.3f;

    [Tooltip("Seberapa cepat naik-turunnya")]
    public float hoverSpeed = 2f;

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
