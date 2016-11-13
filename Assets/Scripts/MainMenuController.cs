using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour {
    public GameObject optionsPanel;
    public GameObject instructionsPanel;

    public void backToMainMenu()
    {
        instructionsPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void goToPanelInstructions()
    {
        optionsPanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    public void goToGame()
    {
        SceneManager.LoadScene("Main 1");
    }

    public void nextLevel()
    {

    }

}
