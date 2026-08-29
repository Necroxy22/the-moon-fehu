using UnityEngine;

/// <summary>
/// Spawn objek dekorasi latar belakang (planet, meteor, dll.) setiap kelipatan N detik.
/// Pasang ke Empty GameObject di scene gameplay.
/// Isi decorPrefabs dengan prefab berisi SpriteRenderer, tanpa Collider2D.
/// </summary>
public class BackgroundDecorSpawner : MonoBehaviour
{
    [Header("Decor Prefabs")]
    [Tooltip("Prefab dekorasi latar (Jupiter, Bumi, Meteor, dll). Harus tanpa Collider2D.")]
    public GameObject[] decorPrefabs;

    [Header("Timing")]
    [Tooltip("Interval waktu (detik) antar kemunculan dekorasi")]
    public float intervalSeconds = 30f;

    [Header("Spawn Position")]
    [Tooltip("Posisi X spawn (kanan layar)")]
    public float spawnX = 16f;
    [Tooltip("Batas bawah posisi Y")]
    public float minY = 2f;
    [Tooltip("Batas atas posisi Y")]
    public float maxY = 5f;
    [Tooltip("Posisi Z objek dekorasi (lebih besar = lebih belakang)")]
    public float spawnZ = 1f;

    [Header("Movement")]
    [Tooltip("Kecepatan gerak dekorasi ke kiri (lebih kecil = efek parallax jauh)")]
    public float scrollSpeed = 1.2f;
    [Tooltip("Posisi X saat objek di-destroy")]
    public float despawnX = -18f;

    private int _nextTriggerIndex = 1; // mulai dari interval pertama (detik ke-30)

    void Update()
    {
        if (DifficultyManager.Instance == null) return;

        float gameTime = DifficultyManager.Instance.GameTime;

        if (gameTime >= _nextTriggerIndex * intervalSeconds)
        {
            SpawnDecor();
            _nextTriggerIndex++;
        }
    }

    private void SpawnDecor()
    {
        if (decorPrefabs == null || decorPrefabs.Length == 0) return;

        // Pilih prefab random dari array
        GameObject prefab = decorPrefabs[Random.Range(0, decorPrefabs.Length)];
        if (prefab == null) return;

        float spawnY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);

        GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Pasang mover – gunakan Init() bawaan BackgroundDecorMover
        BackgroundDecorMover mover = obj.GetComponent<BackgroundDecorMover>();
        if (mover == null) mover = obj.AddComponent<BackgroundDecorMover>();
        mover.Init(scrollSpeed, despawnX);
    }

#if UNITY_EDITOR
    // Visualisasi garis spawn & despawn di Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(spawnX, minY, 0f), new Vector3(spawnX, maxY, 0f));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(despawnX, minY, 0f), new Vector3(despawnX, maxY, 0f));
    }
#endif
}
