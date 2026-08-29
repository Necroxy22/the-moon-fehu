using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject tutorial1;
    public GameObject tutorial2;
    public GameObject tutorial3;
    public GameObject tutorial4;

    private int tutorialPage = 0;

    void Update()
    {
        // =========================
        // TUTORIAL 1
        // =========================
        if (tutorialPage == 1)
        {
            // ← Tutorial 1 → Main Menu
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                BackToMenu();
            }

            // → Tutorial 1 → Tutorial 2
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextTutorial();
            }
        }

        // =========================
        // TUTORIAL 2
        // =========================
        else if (tutorialPage == 2)
        {
            // ← Tutorial 2 → Tutorial 1
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousTutorial();
            }

            // → Tutorial 2 → Tutorial 3
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextTutorial();
            }
        }

        // =========================
        // TUTORIAL 3
        // =========================
        else if (tutorialPage == 3)
        {
            // ← Tutorial 3 → Tutorial 2
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousTutorial();
            }

            // → Tutorial 3 → Tutorial 4
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextTutorial();
            }
        }

        // =========================
        // TUTORIAL 4
        // =========================
        else if (tutorialPage == 4)
        {
            // ← Tutorial 4 → Tutorial 3
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousTutorial();
            }
        }
    }


    // ========================================
    // MAIN MENU → TUTORIAL 1
    // ========================================
    public void OpenTutorial()
    {
        menuPanel.SetActive(false);

        tutorial1.SetActive(true);
        tutorial2.SetActive(false);
        tutorial3.SetActive(false);
        tutorial4.SetActive(false);

        tutorialPage = 1;
    }


    // ========================================
    // NEXT TUTORIAL
    // ========================================
    public void NextTutorial()
    {
        if (tutorialPage == 1)
        {
            tutorial1.SetActive(false);
            tutorial2.SetActive(true);

            tutorialPage = 2;
        }
        else if (tutorialPage == 2)
        {
            tutorial2.SetActive(false);
            tutorial3.SetActive(true);

            tutorialPage = 3;
        }
        else if (tutorialPage == 3)
        {
            tutorial3.SetActive(false);
            tutorial4.SetActive(true);

            tutorialPage = 4;
        }
    }


    // ========================================
    // PREVIOUS TUTORIAL
    // ========================================
    public void PreviousTutorial()
    {
        if (tutorialPage == 4)
        {
            tutorial4.SetActive(false);
            tutorial3.SetActive(true);

            tutorialPage = 3;
        }
        else if (tutorialPage == 3)
        {
            tutorial3.SetActive(false);
            tutorial2.SetActive(true);

            tutorialPage = 2;
        }
        else if (tutorialPage == 2)
        {
            tutorial2.SetActive(false);
            tutorial1.SetActive(true);

            tutorialPage = 1;
        }
    }


    // ========================================
    // TUTORIAL 1 → MAIN MENU
    // ========================================
    public void BackToMenu()
    {
        tutorial1.SetActive(false);
        tutorial2.SetActive(false);
        tutorial3.SetActive(false);
        tutorial4.SetActive(false);

        menuPanel.SetActive(true);

        tutorialPage = 0;
    }
}