using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUp_Holder : MonoBehaviour
{
    public Image slot0Icon;
    public Image slot1Icon;

    public Sprite hermesSprite;
    public Sprite athenaSprite;
    public Sprite zeusSprite;

    public Color emptyColor = new Color(1f, 1f, 1f, 0.15f);
    public Color filledColor = Color.white;

    public void SetSlots(PowerUpType? slot0, PowerUpType? slot1)
    {
        ApplyToIcon(slot0Icon, slot0);
        ApplyToIcon(slot1Icon, slot1);
    }

    private void ApplyToIcon(Image icon, PowerUpType? type)
    {
        if (icon == null)
            return;

        if (type == null)
        {
            icon.sprite = null;
            icon.enabled = false;
            return;
        }

        Sprite sprite = GetSprite(type.Value);
        if (sprite != null)
        {
            icon.sprite = sprite;
            icon.color = filledColor;
            icon.preserveAspect = true;
            icon.enabled = true;
        }
        else
        {
            icon.enabled = false;
        }
    }

    private Sprite GetSprite(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.Hermes: return hermesSprite;
            case PowerUpType.Athena: return athenaSprite;
            case PowerUpType.Zeus: return zeusSprite;
            default: return null;
        }
    }
}    