using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour {
    public GameObject optionsPanel;
    public GameObject instructionsPanel;
    public GameObject settingsPanel;

    public Text difficultyText;
    public Text sizeText;

    void Start()
    {
        setDifficulty(2);
        setSize(2);
    }

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

    public void goToSettings()
    {
        optionsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void goToGame()
    {
        SceneManager.LoadScene("Main 1");
    }

    public void setDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 1:
                setDifficultyText("Easy");
                Application.difficulty = 0.25f;
                break;
            case 2:
                setDifficultyText("Normal");
                Application.difficulty = 0.5f;
                break;
            case 3:
                setDifficultyText("Hard");
                Application.difficulty = 0.75f;
                break;
            case 4:
                setDifficultyText("Insane");
                Application.difficulty = 1f;
                break;
        }
    }

    public void setSize(int size)
    {
        switch (size)
        {
            case 1:
                setSizeText("Small");
                break;
            case 2:
                setSizeText("Medium");
                break;
            case 3:
                setSizeText("Big");
                break;
            case 4:
                setSizeText("Huge");
                break;
        }
        Application.size = size;
    }

    private void setDifficultyText(string diff)
    {
        difficultyText.text = string.Format("Difficulty: {0}", diff);
    }

    private void setSizeText(string size)
    {
        sizeText.text = string.Format("Size: {0}", size);
    }
}
