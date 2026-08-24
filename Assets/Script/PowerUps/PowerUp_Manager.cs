using System.Collections;
using UnityEngine;

public class PowerUp_Manager : MonoBehaviour
{
    [SerializeField] private PowerUpType? slot0 = null;
    [SerializeField] private PowerUpType? slot1 = null;

    public float zeusDestroyDistance = 4f;
    public float zeusDestroyWidth = 1.5f;
    public string obstacleTag = "Obstacle";

    public float athenaDuration = 30f;
    private bool athenaShieldActive = false;
    private Coroutine athenaTimerRoutine;

    public PowerUp_Holder holderUI;
    public bool HasShield => athenaShieldActive;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
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
        PowerUpType? type = slotIndex == 0 ? slot0 : slot1;

        if (type == null)
            return;

        Activate(type.Value);

        if (slotIndex == 0)
            slot0 = null;
        else
            slot1 = null;

        RefreshUI();
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
        }
    }

    private void ActivateHermes()
    {
        if (playerController != null)
            playerController.GrantDoubleJump();
    }

    private void ActivateAthena()
    {
        if (athenaTimerRoutine != null)
            StopCoroutine(athenaTimerRoutine);

        athenaShieldActive = true;
        Debug.Log("Athena shield activated for " + athenaDuration + " seconds.");
        athenaTimerRoutine = StartCoroutine(AthenaTimer());
    }

    private IEnumerator AthenaTimer()
    {
        yield return new WaitForSeconds(athenaDuration);
        athenaShieldActive = false;
        athenaTimerRoutine = null;
    }

    public void ConsumeShield()
    {
        if (athenaTimerRoutine != null)
        {
            StopCoroutine(athenaTimerRoutine);
            athenaTimerRoutine = null;
        }
        athenaShieldActive = false;
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
        }
    }

    private void RefreshUI()
    {
        // if (holderUI != null)
        //     holderUI.SetSlots(slot0, slot1);

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