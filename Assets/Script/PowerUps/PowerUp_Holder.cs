using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp_Holder : MonoBehaviour
{
    [Header("Inventory Icons")]
    public Image slot0Icon;
    public Image slot1Icon;

    [Header("Radial Cooldown / Expiry Ring Images")]
    public Image slot0CooldownRing;
    public Image slot1CooldownRing;

    [Header("Sprites")]
    public Sprite hermesSprite;
    public Sprite zeusSprite;
    public Sprite pegasusSprite;

    private PowerUp_Manager powerUpManager;

    void Awake()
    {
        // Pakai Awake agar instan dimatikan sebelum frame pertama dirender
        if (slot0CooldownRing != null) slot0CooldownRing.gameObject.SetActive(false);
        if (slot1CooldownRing != null) slot1CooldownRing.gameObject.SetActive(false);
    }

    void Start()
    {
        powerUpManager = FindObjectOfType<PowerUp_Manager>();

        if (slot0CooldownRing != null) slot0CooldownRing.gameObject.SetActive(false);
        if (slot1CooldownRing != null) slot1CooldownRing.gameObject.SetActive(false);
    }

    void Update()
    {
        if (powerUpManager == null) return;

        UpdateSlotRing(slot0CooldownRing, powerUpManager.IsSlot0Occupied, powerUpManager.Slot0RemainingRatio);
        UpdateSlotRing(slot1CooldownRing, powerUpManager.IsSlot1Occupied, powerUpManager.Slot1RemainingRatio);
    }

    private void UpdateSlotRing(Image ring, bool isOccupied, float remainingRatio)
    {
        if (ring == null) return;

        if (isOccupied && remainingRatio > 0f)
        {
            if (!ring.gameObject.activeSelf)
                ring.gameObject.SetActive(true);

            ring.type = Image.Type.Filled;
            ring.fillMethod = Image.FillMethod.Radial360;
            ring.fillOrigin = (int)Image.Origin360.Top;
            ring.fillClockwise = false;
            ring.fillAmount = remainingRatio;
        }
        else
        {
            if (ring.gameObject.activeSelf)
                ring.gameObject.SetActive(false);
        }
    }

    public void SetSlots(PowerUpType? slot0, PowerUpType? slot1)
    {
        ApplyToIcon(slot0Icon, slot0);
        ApplyToIcon(slot1Icon, slot1);
    }

    private void ApplyToIcon(Image icon, PowerUpType? type)
    {
        if (icon == null) return;

        if (type == null)
        {
            icon.sprite = null;
            icon.color = new Color(1f, 1f, 1f, 0f); // Transparan jika kosong
            return;
        }

        Sprite sprite = GetSprite(type.Value);
        if (sprite != null)
        {
            icon.sprite = sprite;
            icon.color = Color.white;
            icon.preserveAspect = true;
            icon.enabled = true;
        }
        else
        {
            icon.sprite = null;
            icon.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    private Sprite GetSprite(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.Hermes: return hermesSprite;
            case PowerUpType.Zeus: return zeusSprite;
            case PowerUpType.Pegasus: return pegasusSprite != null ? pegasusSprite : hermesSprite;
            default: return null;
        }
    }
}
