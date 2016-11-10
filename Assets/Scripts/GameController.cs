using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public KeyCode pause;
	public float totalTime;
		
	private bool paused = false;
	private bool endGame = false;

	void Start () {
	
	}

	public bool isPaused() {
		return paused;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (pause)) {
			if (paused) {
				Time.timeScale = 1;
				paused = false;
			} else {
				Time.timeScale = 0;
				paused = true;
			}
		}
		totalTime -= Time.deltaTime;
		if (totalTime < 0) {
			lose ();
		}
	}

	public void win()
	{
		Debug.Log ("you win");
	}

	public void lose()
	{
		Debug.Log ("you lose");
	}
}
