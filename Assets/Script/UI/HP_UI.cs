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
        hpText.text = player.GetCurrentHP() + " / " + player.maxHealth;
    }
}