using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class menu : MonoBehaviour
{
    public GameObject menupanel;
    public GameObject creditpanel;
    // Start is called before the first frame update
    void Start()
    {
        menupanel.SetActive(true);
        creditpanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreditButton()
    {
        menupanel.SetActive(false);
        creditpanel.SetActive(true);
    }

    public void BackButton()
    {
        menupanel.SetActive(true);
        creditpanel.SetActive(false);
    }
}
