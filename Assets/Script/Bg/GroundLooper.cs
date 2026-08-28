using UnityEngine;

public class GroundLooper : MonoBehaviour
{
    public Transform[] segments;
    public float defaultSpeed = 6f;
    [Tooltip("Overlap sedikit (misal 0.1 sampai 0.3) agar tidak ada celah garis putih")]
    public float overlapOffset = 0.15f;
    public float despawnBuffer = 10f;

    [Header("Seam Obstacle Cover")]
    [Tooltip("Centang untuk menaruh obstacle penutup di setiap garis sambungan awan")]
    public bool spawnObstacleOnSeam = true;
    [Tooltip("Daftar prefab obstacle penutup sambungan")]
    public GameObject[] seamObstaclePrefabs;
    [Tooltip("Ketinggian Y obstacle penutup sambungan (sesuaikan dengan tanah)")]
    public float seamObstacleY = -3.4f;

    private float autoSegmentWidth;
    private float despawnThresholdX;

    void Start()
    {
        if (segments == null || segments.Length == 0)
            return;

        SpriteRenderer sr = segments[0].GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            autoSegmentWidth = sr.sprite.rect.width / sr.sprite.pixelsPerUnit * segments[0].localScale.x;
        }
        else
        {
            autoSegmentWidth = 25.6f * segments[0].localScale.x;
        }

        Camera cam = Camera.main;
        if (cam != null)
        {
            float camLeftX = cam.transform.position.x - (cam.orthographicSize * cam.aspect);
            despawnThresholdX = camLeftX - despawnBuffer;
        }
        else
        {
            despawnThresholdX = -40f;
        }
    }

    void Update()
    {
        if (segments == null || segments.Length < 2) 
            return;

        float speed = DifficultyManager.Instance != null
            ? DifficultyManager.Instance.CurrentSpeed
            : defaultSpeed;

        Vector3 move = Vector2.left * speed * Time.deltaTime;

        Transform rightmost = segments[0];
        for (int i = 1; i < segments.Length; i++)
        {
            if (segments[i] != null && segments[i].position.x > rightmost.position.x)
            {
                rightmost = segments[i];
            }
        }

        float effectiveWidth = Mathf.Max(0.1f, autoSegmentWidth - overlapOffset);

        for (int i = 0; i < segments.Length; i++)
        {
            Transform seg = segments[i];
            if (seg == null) continue;

            seg.Translate(move);

            if (seg.position.x + (autoSegmentWidth * 0.5f) < despawnThresholdX)
            {
                // Titik sambungan tepat berada di ujung kanan sebelum segmen baru dipasang
                float seamX = rightmost.position.x + (autoSegmentWidth * 0.5f);

                float newX = rightmost.position.x + effectiveWidth;
                seg.position = new Vector3(
                    newX,
                    seg.position.y,
                    seg.position.z
                );

                if (spawnObstacleOnSeam)
                {
                    SpawnSeamObstacle(seamX);
                }

                rightmost = seg;
            }
        }
    }

    private void SpawnSeamObstacle(float spawnX)
    {
        if (seamObstaclePrefabs == null || seamObstaclePrefabs.Length == 0)
            return;

        // Anti-Double Check: Cek apakah di sekitar posisi X tersebut sudah ada obstacle/item terdekat (radius 3.5 unit)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(spawnX, seamObstacleY), 3.5f);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Obstacle") || col.GetComponent<PowerUpPickup>() != null)
            {
                return; // Jangan spawn jika sudah ada obstacle/item lain di dekat situ!
            }
        }

        int rand = Random.Range(0, seamObstaclePrefabs.Length);
        GameObject prefab = seamObstaclePrefabs[rand];
        if (prefab == null) return;

        Vector3 pos = new Vector3(spawnX, seamObstacleY, 0f);
        Instantiate(prefab, pos, Quaternion.identity);
    }
}
