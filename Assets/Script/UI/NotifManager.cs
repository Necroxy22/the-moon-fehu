using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    public TextMeshProUGUI notifText;
    public CanvasGroup notifCanvasGroup;

    public AudioClip notifSound;
    private AudioSource audioSource;

    public float showDuration = 1.5f;
    public float fadeDuration = 0.3f;

    private Coroutine currentRoutine;

    void Awake()
    {
        Instance = this;
        notifCanvasGroup.alpha = 0f;
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public static void Notif(string message)
    {
        if (Instance == null) return;
        Instance.ShowNotif(message);
    }

    private void ShowNotif(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        if (notifSound != null)
            audioSource.PlayOneShot(notifSound);

        currentRoutine = StartCoroutine(NotifRoutine(message));
    }

    private IEnumerator NotifRoutine(string message)
    {
        notifText.text = message;

        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        yield return new WaitForSeconds(showDuration);

        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            notifCanvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        notifCanvasGroup.alpha = to;
    }
}