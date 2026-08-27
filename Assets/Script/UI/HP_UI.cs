using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HP_UI : MonoBehaviour
{
    public PlayerController player;
    public Image[] heartIcons;
    public Sprite fullHeartSprite;
    public Sprite emptyHeartSprite;
    public TextMeshProUGUI hpText;

    private int lastHP = -1;

    void Update()
    {
        if (player == null) return;

        int currentHP = player.GetCurrentHP();

        if (currentHP != lastHP)
        {
            lastHP = currentHP;
            UpdateHeartsDisplay(currentHP);
        }
    }

    private void UpdateHeartsDisplay(int currentHP)
    {
        if (heartIcons != null && heartIcons.Length > 0)
        {
            for (int i = 0; i < heartIcons.Length; i++)
            {
                if (heartIcons[i] == null) continue;

                if (i < currentHP)
                {
                    if (fullHeartSprite != null) heartIcons[i].sprite = fullHeartSprite;
                }
                else
                {
                    if (emptyHeartSprite != null) heartIcons[i].sprite = emptyHeartSprite;
                }
            }
        }

        if (hpText != null)
        {
            hpText.text = currentHP + " / " + player.maxHealth;
        }
    }
}