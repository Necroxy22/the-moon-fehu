using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class FitBackgroundOnce : MonoBehaviour
{
    [Tooltip("Aspect ratio paling lebar yang mau lo support, misal 21/9 = 2.33")]
    public float widestSupportedAspect = 2.1f;

    void Start()
    {
        Fit();
    }

    [ContextMenu("Fit Now")]
    void Fit()
    {
        var cam = Camera.main;
        var sr = GetComponent<SpriteRenderer>();
        if (cam == null || sr == null || sr.sprite == null) return;

        float camHeight = cam.orthographicSize * 2f;
        float camWidthAtWidest = camHeight * widestSupportedAspect;

        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;

        // scale sekali aja, pas buat nutup rasio TERLEBAR yang lo support
        float scaleX = camWidthAtWidest / spriteWidth;
        float scaleY = camHeight / spriteHeight;
        float scale = Mathf.Max(scaleX, scaleY);

        transform.localScale = new Vector3(scale, scale, transform.localScale.z);
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, transform.position.z);
    }
}