using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class FixResolutionTool : EditorWindow
{
    [MenuItem("Tools/Fix Resolution and Fullscreen UI")]
    public static void FixAll()
    {
        string[] scenePaths = new string[]
        {
            "Assets/Scenes/Title_Screen.unity",
            "Assets/Scenes/Komik Panel.unity",
            "Assets/Scenes/SampleScene.unity"
        };

        string currentScenePath = SceneManager.GetActiveScene().path;

        foreach (string scenePath in scenePaths)
        {
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            if (!scene.IsValid()) continue;

            Debug.Log($"[FixTool] Memperbaiki scene: {scenePath}");

            // Standard Golden Ratio CanvasScaler: 1920x1080 with Match 0.5
            CanvasScaler[] scalers = GameObject.FindObjectsOfType<CanvasScaler>(true);
            foreach (var scaler in scalers)
            {
                Undo.RecordObject(scaler, "Fix CanvasScaler");
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scaler.matchWidthOrHeight = 0.5f;
                EditorUtility.SetDirty(scaler);
            }

            if (scenePath.Contains("Title_Screen"))
            {
                FixTitleScreenScene();
            }
            else if (scenePath.Contains("Komik Panel"))
            {
                FixKomikPanelScene();
            }

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log($"[FixTool] Berhasil menyimpan perbaikan untuk: {scenePath}");
        }

        if (!string.IsNullOrEmpty(currentScenePath))
        {
            EditorSceneManager.OpenScene(currentScenePath, OpenSceneMode.Single);
        }

        EditorUtility.DisplayDialog("Sukses!", "Semua UI telah diselaraskan ke proporsi 1920x1080 (Match 0.5) dengan rapi dan tidak terpotong!", "OK");
    }

    private static void FixTitleScreenScene()
    {
        GameObject mainMenuGO = GameObject.Find("Main menu");
        if (mainMenuGO != null)
        {
            Canvas canvas = mainMenuGO.GetComponent<Canvas>();
            if (canvas != null)
            {
                Undo.RecordObject(canvas, "Fix Main Menu Canvas RenderMode");
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                EditorUtility.SetDirty(canvas);
            }

            // 1. Title Bg (Stretch 100%)
            Transform titleBgTr = mainMenuGO.transform.Find("Title Bg");
            if (titleBgTr != null)
            {
                GameObject bgGO = titleBgTr.gameObject;
                Undo.RecordObject(bgGO, "Fix Title Bg UI");

                Sprite sprite = null;
                SpriteRenderer sr = bgGO.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sprite = sr.sprite;
                    Undo.DestroyObjectImmediate(sr);
                }

                RectTransform rt = bgGO.GetComponent<RectTransform>();
                if (rt == null) rt = Undo.AddComponent<RectTransform>(bgGO);

                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = Vector2.zero;
                rt.localScale = Vector3.one;

                Image img = bgGO.GetComponent<Image>();
                if (img == null) img = Undo.AddComponent<Image>(bgGO);
                if (sprite != null) img.sprite = sprite;
                img.raycastTarget = false;
                bgGO.layer = LayerMask.NameToLayer("UI");
                bgGO.transform.SetAsFirstSibling();

                EditorUtility.SetDirty(bgGO);
            }

            // 2. Menu Title Logo ("TO THE MOON")
            Transform logoTr = mainMenuGO.transform.Find("Menu Title Logo");
            if (logoTr != null)
            {
                GameObject logoGO = logoTr.gameObject;
                Undo.RecordObject(logoGO, "Fix Menu Title Logo UI");

                Sprite sprite = null;
                SpriteRenderer sr = logoGO.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sprite = sr.sprite;
                    Undo.DestroyObjectImmediate(sr);
                }

                RectTransform rt = logoGO.GetComponent<RectTransform>();
                if (rt == null) rt = Undo.AddComponent<RectTransform>(logoGO);

                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = new Vector2(-15f, 240f);
                rt.sizeDelta = new Vector2(750f, 170f);
                rt.localScale = Vector3.one;

                Image img = logoGO.GetComponent<Image>();
                if (img == null) img = Undo.AddComponent<Image>(logoGO);
                if (sprite != null) img.sprite = sprite;
                img.preserveAspect = true;
                img.raycastTarget = false;
                logoGO.layer = LayerMask.NameToLayer("UI");

                EditorUtility.SetDirty(logoGO);
            }

            // 3. Play ("MULAI")
            Transform playTr = mainMenuGO.transform.Find("Play");
            if (playTr != null)
            {
                RectTransform rt = playTr.GetComponent<RectTransform>();
                if (rt != null)
                {
                    Undo.RecordObject(rt, "Fix Play Button");
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.anchoredPosition = new Vector2(-15f, 50f);
                    rt.sizeDelta = new Vector2(560f, 96f);
                    EditorUtility.SetDirty(rt);
                }
            }

            // 4. Credit ("CREDIT")
            Transform creditTr = mainMenuGO.transform.Find("Credit");
            if (creditTr != null)
            {
                RectTransform rt = creditTr.GetComponent<RectTransform>();
                if (rt != null)
                {
                    Undo.RecordObject(rt, "Fix Credit Button");
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.anchoredPosition = new Vector2(-15f, -80f);
                    rt.sizeDelta = new Vector2(560f, 96f);
                    EditorUtility.SetDirty(rt);
                }
            }

            // 5. Quit ("KELUAR")
            Transform quitTr = mainMenuGO.transform.Find("Quit");
            if (quitTr != null)
            {
                RectTransform rt = quitTr.GetComponent<RectTransform>();
                if (rt != null)
                {
                    Undo.RecordObject(rt, "Fix Quit Button");
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.anchoredPosition = new Vector2(-15f, -210f);
                    rt.sizeDelta = new Vector2(560f, 96f);
                    EditorUtility.SetDirty(rt);
                }
            }

            // 6. Character Image (Astronot)
            Transform charImgTr = mainMenuGO.transform.Find("Image");
            if (charImgTr != null)
            {
                RectTransform rt = charImgTr.GetComponent<RectTransform>();
                if (rt != null)
                {
                    Undo.RecordObject(rt, "Fix Char Image");
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.anchoredPosition = new Vector2(580f, 60f);
                    rt.sizeDelta = new Vector2(650f, 650f);
                    EditorUtility.SetDirty(rt);
                }
            }

            // 7. Tutorial Button (Tanda tanya)
            Transform tutBtnTr = mainMenuGO.transform.Find("Tutorial Button");
            if (tutBtnTr != null)
            {
                RectTransform rt = tutBtnTr.GetComponent<RectTransform>();
                if (rt != null)
                {
                    Undo.RecordObject(rt, "Fix Tutorial Button");
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.anchoredPosition = new Vector2(-780f, -390f);
                    rt.sizeDelta = new Vector2(180f, 160f);
                    EditorUtility.SetDirty(rt);
                }
            }
        }

        // 8. Fix Tutorial 1 - 4
        string[] tutCanvasNames = new string[] { "Tutorial 1", "Tutorial 2", "Tutorial 3", "Tutorial 4" };
        foreach (string tName in tutCanvasNames)
        {
            GameObject tutGO = GameObject.Find(tName);
            if (tutGO != null)
            {
                Canvas c = tutGO.GetComponent<Canvas>();
                if (c != null) c.renderMode = RenderMode.ScreenSpaceOverlay;

                Image[] imgs = tutGO.GetComponentsInChildren<Image>(true);
                foreach (var img in imgs)
                {
                    if (img.gameObject.name.Contains("char") || img.gameObject.name.Contains("Pengenalan"))
                    {
                        RectTransform rt = img.GetComponent<RectTransform>();
                        if (rt != null)
                        {
                            rt.anchorMin = Vector2.zero;
                            rt.anchorMax = Vector2.one;
                            rt.offsetMin = Vector2.zero;
                            rt.offsetMax = Vector2.zero;
                            rt.anchoredPosition = Vector2.zero;
                            rt.sizeDelta = Vector2.zero;
                            rt.localScale = Vector3.one;
                            img.gameObject.transform.SetAsFirstSibling();
                        }
                    }
                }

                foreach (Transform child in tutGO.transform)
                {
                    if (child.name.ToLower().Contains("back"))
                    {
                        RectTransform rt = child.GetComponent<RectTransform>();
                        if (rt != null)
                        {
                            Undo.RecordObject(rt, "Fix Back Button Position");
                            rt.anchorMin = new Vector2(0.5f, 0.5f);
                            rt.anchorMax = new Vector2(0.5f, 0.5f);
                            rt.pivot = new Vector2(0.5f, 0.5f);
                            rt.anchoredPosition = new Vector2(-780f, -390f);
                            rt.sizeDelta = new Vector2(180f, 160f);
                            EditorUtility.SetDirty(rt);
                        }
                    }
                    else if (child.name.ToLower().Contains("next"))
                    {
                        RectTransform rt = child.GetComponent<RectTransform>();
                        if (rt != null)
                        {
                            Undo.RecordObject(rt, "Fix Next Button Position");
                            rt.anchorMin = new Vector2(0.5f, 0.5f);
                            rt.anchorMax = new Vector2(0.5f, 0.5f);
                            rt.pivot = new Vector2(0.5f, 0.5f);
                            rt.anchoredPosition = new Vector2(780f, -390f);
                            rt.sizeDelta = new Vector2(180f, 160f);
                            EditorUtility.SetDirty(rt);
                        }
                    }
                }
            }
        }
    }

    private static void FixKomikPanelScene()
    {
        GameObject panelAkhir = GameObject.Find("PanelAkhir");
        if (panelAkhir != null)
        {
            RectTransform rt = panelAkhir.GetComponent<RectTransform>();
            if (rt != null)
            {
                Undo.RecordObject(rt, "Fix PanelAkhir Stretch");
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = Vector2.zero;
                EditorUtility.SetDirty(rt);
            }
        }

        GameObject videoDisplay = GameObject.Find("VideoDisplay");
        if (videoDisplay != null)
        {
            RectTransform rt = videoDisplay.GetComponent<RectTransform>();
            if (rt != null)
            {
                Undo.RecordObject(rt, "Fix VideoDisplay Stretch");
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = Vector2.zero;
                EditorUtility.SetDirty(rt);
            }
        }

        GameObject toMainMenu = GameObject.Find("To MainMenu");
        if (toMainMenu != null)
        {
            RectTransform rt = toMainMenu.GetComponent<RectTransform>();
            if (rt != null)
            {
                Undo.RecordObject(rt, "Fix To MainMenu Anchor");
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = new Vector2(-780f, -390f);
                rt.sizeDelta = new Vector2(180f, 160f);
                EditorUtility.SetDirty(rt);
            }
        }

        GameObject toGameplay = GameObject.Find("To Gameplay");
        if (toGameplay != null)
        {
            RectTransform rt = toGameplay.GetComponent<RectTransform>();
            if (rt != null)
            {
                Undo.RecordObject(rt, "Fix To Gameplay Anchor");
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = new Vector2(780f, -390f);
                rt.sizeDelta = new Vector2(180f, 160f);
                EditorUtility.SetDirty(rt);
            }
        }
    }
}
