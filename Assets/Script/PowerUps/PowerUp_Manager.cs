using System.Collections;
using UnityEngine;

public class PowerUp_Manager : MonoBehaviour
{
    [SerializeField] private PowerUpType? slot0 = null;
    [SerializeField] private PowerUpType? slot1 = null;

    public float zeusDestroyDistance = 4f;
    public float zeusDestroyWidth = 1.5f;
    public string obstacleTag = "Obstacle";

    public float athenaDuration = 20f;
    private bool athenaShieldActive = false;
    private Coroutine athenaTimerRoutine;

    public GameObject bubbleShieldOverlay;

    public float hermesDuration = 10f;
    private bool hermesActive = false;
    private Coroutine hermesTimerRoutine;

    public float pegasusDuration = 15f;
    private bool pegasusActive = false;
    private Coroutine pegasusTimerRoutine;

    public PowerUp_Holder holderUI;
    public bool HasShield => athenaShieldActive;
    public bool HasHermesEffect => hermesActive;
    public bool HasPegasusEffect => pegasusActive;
    public float useAnimationDuration = 0.5f;
    private PowerUpType? currentlyUsing = null;
    private Coroutine useAnimationRoutine;
    public AudioClip BubbleEffect;
    public AudioClip StarEffect;
    public AudioClip PegasusEffect;
    public PowerUpType? CurrentlyUsing => currentlyUsing;
    public bool HasZeusInInventory => slot0 == PowerUpType.Zeus || slot1 == PowerUpType.Zeus;
    public bool HasAthenaInInventory => slot0 == PowerUpType.Athena || slot1 == PowerUpType.Athena || athenaShieldActive;
    public bool HasHermesInInventory => slot0 == PowerUpType.Hermes || slot1 == PowerUpType.Hermes || hermesActive;
    public bool HasPegasusInInventory => slot0 == PowerUpType.Pegasus || slot1 == PowerUpType.Pegasus || pegasusActive;
    public PowerUpType? ActiveHeldPowerUp
    {
        get
        {
            if (slot0 == PowerUpType.Zeus || slot1 == PowerUpType.Zeus) 
                return PowerUpType.Zeus;

            if (pegasusActive)
                return PowerUpType.Pegasus;

            if (hermesActive) 
                return PowerUpType.Hermes;

            return null;
        }
    }
        
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (bubbleShieldOverlay != null)
            bubbleShieldOverlay.SetActive(false);

        RefreshUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            UseSlot(0);

        if (Input.GetKeyDown(KeyCode.K))
            UseSlot(1);
    }

    public bool TryStore(PowerUpType type)
    {
        if (playerController != null && (playerController.IsInvulnerable || playerController.IsDead))
            return false;

        if (athenaShieldActive || hermesActive || pegasusActive)
            return false;

        if (slot0 == type || slot1 == type)
            return false;

        if (slot0 == null)
        {
            slot0 = type;
            RefreshUI();
            return true;
        }

        if (slot1 == null)
        {
            slot1 = type;
            RefreshUI();
            return true;
        }

        return false;
    }

    private void UseSlot(int slotIndex)
    {
        if (playerController != null && (playerController.IsInvulnerable || playerController.IsDead))
            return;

        if (athenaShieldActive || hermesActive || pegasusActive)
            return;

        PowerUpType? type = slotIndex == 0 ? slot0 : slot1;

        if (type == null)
            return;

        if (slotIndex == 0)
            slot0 = null;
        else
            slot1 = null;

        TriggerUseAnimation(type.Value);
        Activate(type.Value);

        RefreshUI();
    }

    private void TriggerUseAnimation(PowerUpType type)
    {
        if (useAnimationRoutine != null)
            StopCoroutine(useAnimationRoutine);

        currentlyUsing = type;
        useAnimationRoutine = StartCoroutine(UseAnimationTimer());
    }

    private IEnumerator UseAnimationTimer()
    {
        yield return new WaitForSeconds(useAnimationDuration);
        currentlyUsing = null;
        useAnimationRoutine = null;
    }

    private void Activate(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.Hermes:
                ActivateHermes();
                break;

            case PowerUpType.Athena:
                ActivateAthena();
                break;

            case PowerUpType.Zeus:
                ActivateZeus();
                break;

            case PowerUpType.Pegasus:
                ActivatePegasus();
                break;
        }
    }

    private void ActivatePegasus()
    {
        if (pegasusTimerRoutine != null)
            StopCoroutine(pegasusTimerRoutine);

        pegasusActive = true;
        if (PegasusEffect != null && playerController != null)
            playerController.PlaySound(PegasusEffect, 1f);

        pegasusTimerRoutine = StartCoroutine(PegasusTimer());
    }

    private IEnumerator PegasusTimer()
    {
        yield return new WaitForSeconds(pegasusDuration);
        pegasusActive = false;
        pegasusTimerRoutine = null;
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
        yield return new WaitForSeconds(hermesDuration);
        hermesActive = false;
        hermesTimerRoutine = null;
    }

    private void ActivateAthena()
    {
        if (athenaTimerRoutine != null)
            StopCoroutine(athenaTimerRoutine);

        athenaShieldActive = true;
        if (bubbleShieldOverlay != null)
        {
            bubbleShieldOverlay.SetActive(true);
            playerController.PlaySound(BubbleEffect, 1f);
        }

        athenaTimerRoutine = StartCoroutine(AthenaTimer());
    }

    private IEnumerator AthenaTimer()
    {
        yield return new WaitForSeconds(athenaDuration);
        athenaShieldActive = false;
        if (bubbleShieldOverlay != null)
            bubbleShieldOverlay.SetActive(false);

        athenaTimerRoutine = null;
    }

    private void ActivateZeus()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag(obstacleTag);
        int destroyedCount = 0;

        foreach (GameObject obstacle in obstacles)
        {
            Vector2 toObstacle = obstacle.transform.position - transform.position;

            bool isAhead = toObstacle.x > 0f;
            bool isWithinDistance = toObstacle.x <= zeusDestroyDistance;
            bool isWithinWidth = Mathf.Abs(toObstacle.y) <= zeusDestroyWidth;

            if (isAhead && isWithinDistance && isWithinWidth)
            {
                Destroy(obstacle);
                destroyedCount++;
            }
            playerController.PlaySound(StarEffect, 1f);
        }
    }
    public void ConsumeShield()
    {
    }
    private void RefreshUI()
    {
        try
        {
            if (holderUI != null)
                holderUI.SetSlots(slot0, slot1);
        }
        catch (System.Exception e)
        {
            Debug.LogError("UI ERROR: " + e);
        }
    }
}