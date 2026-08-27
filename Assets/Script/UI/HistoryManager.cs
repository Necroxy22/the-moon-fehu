using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class HistoryEntry
{
    public string date;
    public string duration;
}

[System.Serializable]
public class HistoryWrapper
{
    public List<HistoryEntry> list = new List<HistoryEntry>();
}

public class HistoryManager : MonoBehaviour
{
    public static HistoryManager Instance;

    [Header("UI Display (Optional)")]
    public TextMeshProUGUI historyDisplayText;
    public int maxEntries = 10;

    private const string HistoryKey = "GamePlayHistory";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RefreshUI();
    }

    public static void SaveHistory(float timeSurvived)
    {
        HistoryWrapper wrapper = LoadHistory();

        int minutes = (int)(timeSurvived / 60);
        int seconds = (int)(timeSurvived % 60);

        HistoryEntry newEntry = new HistoryEntry
        {
            date = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
            duration = string.Format("{0:00}:{1:00}", minutes, seconds)
        };

        wrapper.list.Insert(0, newEntry);

        if (wrapper.list.Count > 10)
        {
            wrapper.list.RemoveAt(wrapper.list.Count - 1);
        }

        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(HistoryKey, json);
        PlayerPrefs.Save();
    }

    public static HistoryWrapper LoadHistory()
    {
        if (PlayerPrefs.HasKey(HistoryKey))
        {
            string json = PlayerPrefs.GetString(HistoryKey);
            return JsonUtility.FromJson<HistoryWrapper>(json);
        }
        return new HistoryWrapper();
    }

    public void RefreshUI()
    {
        if (historyDisplayText == null) return;

        HistoryWrapper wrapper = LoadHistory();
        if (wrapper.list == null || wrapper.list.Count == 0)
        {
            historyDisplayText.text = "Belum ada riwayat bermain.";
            return;
        }

        string displayText = "";
        foreach (var entry in wrapper.list)
        {
            displayText += $"• {entry.date} - Duration: {entry.duration}\n";
        }

        historyDisplayText.text = displayText;
    }

    public void ClearHistory()
    {
        PlayerPrefs.DeleteKey(HistoryKey);
        RefreshUI();
    }
}
