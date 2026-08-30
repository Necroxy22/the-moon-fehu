using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// Editor tool to fix all Canvas Scalers in all scenes to "Scale With Screen Size"
/// so the UI scales correctly in WebGL builds and Fullscreen mode.
/// </summary>
public class FixCanvasScaler : EditorWindow
{
    private static readonly Vector2 referenceResolution = new Vector2(1920, 1080);
    private static readonly CanvasScaler.ScreenMatchMode matchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
    private static readonly float matchWidthOrHeight = 0.5f;

    [MenuItem("Tools/Fix Canvas Scalers (All Open Scenes)")]
    public static void FixAllOpenScenes()
    {
        int totalFixed = 0;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded) continue;

            int fixedInScene = FixSceneCanvasScalers(scene);
            totalFixed += fixedInScene;
            Debug.Log($"[FixCanvasScaler] Scene '{scene.name}': fixed {fixedInScene} CanvasScaler(s).");

            if (fixedInScene > 0)
                EditorSceneManager.MarkSceneDirty(scene);
        }

        Debug.Log($"[FixCanvasScaler] Done. Total fixed: {totalFixed} CanvasScaler(s). Saving...");

        if (totalFixed > 0)
        {
            EditorSceneManager.SaveOpenScenes();
            Debug.Log("[FixCanvasScaler] Scenes saved.");
        }
    }

    [MenuItem("Tools/Fix Canvas Scalers (Build Settings Scenes)")]
    public static void FixBuildSettingsScenes()
    {
        EditorSceneManager.SaveOpenScenes();

        int totalFixed = 0;

        foreach (EditorBuildSettingsScene buildScene in EditorBuildSettings.scenes)
        {
            if (string.IsNullOrEmpty(buildScene.path)) continue;

            Scene scene = EditorSceneManager.OpenScene(buildScene.path, OpenSceneMode.Additive);
            int fixedInScene = FixSceneCanvasScalers(scene);
            totalFixed += fixedInScene;
            Debug.Log($"[FixCanvasScaler] Scene '{scene.name}': fixed {fixedInScene} CanvasScaler(s).");

            if (fixedInScene > 0)
            {
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }

            EditorSceneManager.CloseScene(scene, true);
        }

        Debug.Log($"[FixCanvasScaler] Done. Total fixed: {totalFixed} across all build scenes.");
    }

    private static int FixSceneCanvasScalers(Scene scene)
    {
        int count = 0;
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            CanvasScaler[] scalers = root.GetComponentsInChildren<CanvasScaler>(true);
            foreach (CanvasScaler scaler in scalers)
            {
                bool dirty = false;

                if (scaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
                {
                    scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    dirty = true;
                }

                if (scaler.referenceResolution != referenceResolution)
                {
                    scaler.referenceResolution = referenceResolution;
                    dirty = true;
                }

                if (scaler.screenMatchMode != matchMode)
                {
                    scaler.screenMatchMode = matchMode;
                    dirty = true;
                }

                if (!Mathf.Approximately(scaler.matchWidthOrHeight, matchWidthOrHeight))
                {
                    scaler.matchWidthOrHeight = matchWidthOrHeight;
                    dirty = true;
                }

                if (dirty)
                {
                    EditorUtility.SetDirty(scaler);
                    count++;
                }
            }
        }
        return count;
    }
}
