using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {
	public KeyCode pause;
	public float totalTime;
    public Text timerLabel;
    public GameObject deadPanel;
    public GameObject goalPanel;
    public GameObject pausePanel;

    private bool paused = false;
	private bool gameOver = false;

	public bool isPaused() {
		return paused;
	}
	
	void Update () {
		if (Input.GetKeyDown (pause)) {
			if (paused) {
                unPauseGame();
			} else {
                pauseGame(true);
            }
		}
		
        timeManager();
		if (totalTime < 0) {
			lose ();
		}
	}

	public void win()
	{
        gameOver = true;
        pauseGame(false);
        goalPanel.SetActive(true);
	}

	public void lose()
	{
        gameOver = true;
        pauseGame(false);
        deadPanel.SetActive(true);
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void pauseGame(bool panel)
    {
        Time.timeScale = 0;
        paused = true;
        pausePanel.SetActive(panel);
    }

    private void unPauseGame()
    {
        Time.timeScale = 1;
        paused = false;
        pausePanel.SetActive(false);
    }

    private void timeManager()
    {
        totalTime -= Time.deltaTime;
        var minutes = totalTime / 60;
        var seconds = totalTime % 60;

        timerLabel.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}