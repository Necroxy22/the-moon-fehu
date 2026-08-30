using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class FitBackgroundToCamera : MonoBehaviour
{
    private Camera cam;
    private SpriteRenderer sr;
    private int lastScreenW, lastScreenH;

    void Awake()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        Fit();
    }

    void Update()
    {
        // deteksi resize (misal browser WebGL di-resize)
        if (Screen.width != lastScreenW || Screen.height != lastScreenH)
        {
            Fit();
        }
    }

    void Fit()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null || sr == null || sr.sprite == null) return;

        lastScreenW = Screen.width;
        lastScreenH = Screen.height;

        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;

        // "cover" mode: scale biar nutup seluruh layar, bisa crop dikit di salah satu sisi
        float scaleX = camWidth / spriteWidth;
        float scaleY = camHeight / spriteHeight;
        float scale = Mathf.Max(scaleX, scaleY);

        transform.localScale = new Vector3(scale, scale, transform.localScale.z);

        // posisiin center pas di tengah kamera
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
    }
}