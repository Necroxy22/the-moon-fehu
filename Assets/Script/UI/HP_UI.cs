using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HP_UI : MonoBehaviour
{
    public PlayerController player;
    public TextMeshProUGUI hpText;

    void Update()
    {
        if (player == null || hpText == null)
            return;

        hpText.text = player.GetCurrentHP() + " / " + player.maxHealth;
    }
}