using System.Collections;
using UnityEngine;

public class PowerUp_Manager : MonoBehaviour
{
    [Header("Inventory Slots (Manual Items: J & K)")]
    [SerializeField] private PowerUpType? slot0 = null;
    [SerializeField] private PowerUpType? slot1 = null;

    [Header("Slot Item Expiry Timers")]
    public float pegasusShelfLife = 5f;
    public float sepatuShelfLife = 10f;
    public float bintangShelfLife = 15f;

    private float slot0Timer = 0f;
    private float slot0MaxDuration = 1f;

    private float slot1Timer = 0f;
    private float slot1MaxDuration = 1f;

    [Header("Bubble Shield (Instant World Passive)")]
    public float athenaDuration = 10f;
    public float athenaBlinkThreshold = 5f;
    public float athenaBlinkInterval = 0.15f;
    private bool athenaShieldActive = false;
    private Coroutine athenaTimerRoutine;
    public GameObject bubbleShieldOverlay;
    public AudioClip BubbleEffect;

    [Header("Buff Durations Once Used")]
    public float hermesBuffDuration = 5f;
    private bool hermesActive = false;
    private Coroutine hermesTimerRoutine;

    public float pegasusBuffDuration = 10f;
    private bool pegasusActive = false;
    public GameObject pegasusShieldOverlay;
    private Coroutine pegasusTimerRoutine;

    [Header("Zeus / Bintang Settings")]
    public float zeusDestroyDistance = 10f;
    public float zeusDestroyWidth = 2.5f;
    public string obstacleTag = "Obstacle";

    [Header("Audio & References")]
    public PowerUp_Holder holderUI;
    public AudioClip StarEffect;
    public AudioClip PegasusEffect;

    public bool HasShield => athenaShieldActive;
    public bool HasHermesEffect => hermesActive;
    public bool HasPegasusEffect => pegasusActive;

    // Properties for PowerUp_Holder UI to read
    public bool IsSlot0Occupied => slot0 != null;
    public bool IsSlot1Occupied => slot1 != null;

    public float Slot0RemainingRatio => slot0MaxDuration > 0f ? Mathf.Clamp01(slot0Timer / slot0MaxDuration) : 0f;
    public float Slot1RemainingRatio => slot1MaxDuration > 0f ? Mathf.Clamp01(slot1Timer / slot1MaxDuration) : 0f;

    // Animasi memegang item (Sepatu / Bintang / Pegasus)
    public PowerUpType? ActiveHeldPowerUp
    {
        get
        {
            if (pegasusActive) return PowerUpType.Pegasus;
            if (hermesActive) return PowerUpType.Hermes;
            if (slot0 == PowerUpType.Zeus || slot1 == PowerUpType.Zeus) return PowerUpType.Zeus;
            if (slot0 != null) return slot0;
            if (slot1 != null) return slot1;
            return null;
        }
    }

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (bubbleShieldOverlay != null)
            bubbleShieldOverlay.SetActive(false);

        if (pegasusShieldOverlay != null)
            pegasusShieldOverlay.SetActive(false);

        RefreshUI();
    }

    void Update()
    {
        if (playerController != null && playerController.IsDead)
            return;

        // Slot 0 Countdown
        if (slot0 != null)
        {
            slot0Timer -= Time.deltaTime;
            if (slot0Timer <= 0f)
            {
                slot0 = null; // Waktu habis, item hilang otomatis
                RefreshUI();
            }
        }

        // Slot 1 Countdown
        if (slot1 != null)
        {
            slot1Timer -= Time.deltaTime;
            if (slot1Timer <= 0f)
            {
                slot1 = null; // Waktu habis, item hilang otomatis
                RefreshUI();
            }
        }
        if(!playerController.IsInvulnerable)
        {
            // Input J (Slot 0) & K (Slot 1)
            if (Input.GetKeyDown(KeyCode.J))
            {
                UseSlot(0);
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                UseSlot(1);
            }   
        }
    }

    private float GetShelfLife(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.Pegasus: return pegasusShelfLife; // 5 detik
            case PowerUpType.Hermes: return sepatuShelfLife;   // 10 detik
            case PowerUpType.Zeus: return bintangShelfLife;    // 15 detik
            default: return 10f;
        }
    }

    // Hanya menerima item manual (Hermes, Zeus, Pegasus). Athena tidak masuk sini!
    public bool TryStore(PowerUpType type)
    {
        if (type == PowerUpType.Athena)
            return false;

        float duration = GetShelfLife(type);

        if (slot0 == null)
        {
            slot0 = type;
            slot0MaxDuration = duration;
            slot0Timer = duration;
            RefreshUI();
            return true;
        }

        if (slot1 == null)
        {
            slot1 = type;
            slot1MaxDuration = duration;
            slot1Timer = duration;
            RefreshUI();
            return true;
        }

        return false; // Kedua slot penuh
    }

    public void ActivateBubbleShield()
    {
        if (athenaTimerRoutine != null)
            StopCoroutine(athenaTimerRoutine);

        athenaShieldActive = true;

        if (bubbleShieldOverlay != null)
            bubbleShieldOverlay.SetActive(true);

        if (BubbleEffect != null && playerController != null)
            playerController.PlaySound(BubbleEffect, 1f);

        athenaTimerRoutine = StartCoroutine(BubbleShieldTimer());
    }
    private IEnumerator BubbleShieldTimer()
    {
        float elapsed = 0f;

        float normalPhase = athenaDuration - athenaBlinkThreshold;
        if (normalPhase > 0f)
            yield return new WaitForSeconds(normalPhase);

        float blinkElapsed = 0f;
        bool visible = true;
        while (blinkElapsed < athenaBlinkThreshold)
        {
            visible = !visible;
            if (bubbleShieldOverlay != null)
                bubbleShieldOverlay.SetActive(visible);

            yield return new WaitForSeconds(athenaBlinkInterval);
            blinkElapsed += athenaBlinkInterval;
        }

        athenaShieldActive = false;

        if (bubbleShieldOverlay != null)
            bubbleShieldOverlay.SetActive(false);

        athenaTimerRoutine = null;
    }

    private void UseSlot(int slotIndex)
    {
        PowerUpType? type = slotIndex == 0 ? slot0 : slot1;
        if (type == null) return;

        // Kosongkan slot seketika
        if (slotIndex == 0) slot0 = null;
        else slot1 = null;

        RefreshUI();

        // Jalankan efek item
        switch (type.Value)
        {
            case PowerUpType.Hermes:
                ActivateHermes();
                break;

            case PowerUpType.Pegasus:
                ActivatePegasus();
                break;

            case PowerUpType.Zeus:
                ActivateZeus();
                break;
        }
    }

    private void ActivateHermes()
    {
        if (hermesTimerRoutine != null)
            StopCoroutine(hermesTimerRoutine);

        hermesActive = true;
        hermesTimerRoutine = StartCoroutine(HermesTimer());
    }

    private IEnumerator HermesTimer()
    {
        yield return new WaitForSeconds(hermesBuffDuration);
        hermesActive = false;
        hermesTimerRoutine = null;
    }

    private void ActivatePegasus()
    {
        if (pegasusTimerRoutine != null)
            StopCoroutine(pegasusTimerRoutine);

        pegasusActive = true;
        if (PegasusEffect != null && playerController != null)
            playerController.PlaySound(PegasusEffect, 1f);
        
        
        if (pegasusShieldOverlay != null)
            pegasusShieldOverlay.SetActive(true);

        pegasusTimerRoutine = StartCoroutine(PegasusTimer());
    }

    private IEnumerator PegasusTimer()
    {
        yield return new WaitForSeconds(pegasusBuffDuration);
        pegasusActive = false;
        
        if (pegasusShieldOverlay != null)
            pegasusShieldOverlay.SetActive(false);
        pegasusTimerRoutine = null;

    }
 
    private void ActivateZeus()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag(obstacleTag);
        foreach (GameObject obstacle in obstacles)
        {
            Vector2 toObstacle = obstacle.transform.position - transform.position;
            bool isAhead = toObstacle.x > 0f;
            bool isWithinDistance = toObstacle.x <= zeusDestroyDistance;
            bool isWithinWidth = Mathf.Abs(toObstacle.y) <= zeusDestroyWidth;

            if (isAhead && isWithinDistance && isWithinWidth)
            {
                Destroy(obstacle);
            }
        }

        if (StarEffect != null && playerController != null)
            playerController.PlaySound(StarEffect, 1f);
    }

    public void ConsumeShield()
    {
    }

    private void RefreshUI()
    {
        if (holderUI != null)
            holderUI.SetSlots(slot0, slot1);
    }
}