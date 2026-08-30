using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1000)]
public class UIResolutionAutoFixer : MonoBehaviour
{
    void Awake()
    {
        CanvasScaler[] scalers = FindObjectsOfType<CanvasScaler>(true);
        foreach (var scaler in scalers)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
        }

        Canvas[] canvases = FindObjectsOfType<Canvas>(true);
        foreach (var c in canvases)
        {
            if (c.renderMode == RenderMode.ScreenSpaceCamera && c.worldCamera == null)
            {
                c.worldCamera = Camera.main;
            }
        }
    }
}
