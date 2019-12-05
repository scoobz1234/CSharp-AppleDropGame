using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMasterScript : MonoBehaviour {

	public GameObject Apple, Player, Apple1, Apple2, Apple3, QuitYes;
	public float Timer, SongTimer, SpawnTimer = 2.5f, PlayerScore = 0, TillDeath;
	public Transform SpawnPoint;
	public AudioClip Theme;
	public int GameScore;
	public bool GameStart = false;
	public Text PlayerText, PlayerNameInput, HighScore, HighScore1, StartbuttonText, ExitButtonText, ScoreTxt;
	public Canvas StartGUI, ExitMenu, MainGUI, InGameExitMenu, GameOverGUI, DirectionsGUI;
	private string PlayerName;

	void Start () {
	
		Timer = SpawnTimer;
		//disables all menus needed not needed at start
		ExitMenu.enabled = false;
		MainGUI.enabled = false;
		InGameExitMenu.enabled = false;
		GameOverGUI.enabled = false;
		DirectionsGUI.enabled = false;
		//disables all objects not needed at start
		Player.SetActive (false);
		Apple1.SetActive (false);
		Apple2.SetActive (false);
		Apple3.SetActive (false);
		QuitYes.SetActive (false);
		//sets game score to 0
		GameScore = 0;
		// recalls high score info for display at start screen
		HighScore1.text = "HighScore: " + PlayerPrefs.GetString ("HighScoreName").ToString() + " - " + ((int)PlayerPrefs.GetFloat ("HighScore")).ToString ();

	}
	

	void FixedUpdate () {
		// assigns the input field text to the guitext of player name
		PlayerText.text = PlayerNameInput.text.ToString ();
		// assigns the input field to playername a private string for score saving purposes
		PlayerName = PlayerNameInput.text.ToString();
		// gui text assignment for the players score
		ScoreTxt.text = "My Score: " + GameScore.ToString ();

		if (GameStart == true) {
			// brings up "Pause" Menu
			if (Input.GetKeyUp (KeyCode.Escape)) {
				Destroy (GameObject.FindWithTag("Apple"));
				ExitPressInGame ();
			}
			// ends game if you miss too many apples.. 3 to be exact..
			if (TillDeath == 0) {
				GameOver ();
			}
		
			// sets the left most point of spawning
			Vector3 ScreenLeft = Camera.main.ViewportToWorldPoint (new Vector3 (0, 1, 10));
			// sets the right most point of spawning
			Vector3 ScreenRight = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 10));
			// randomly spawns between the left most point and right most points of spawning, locks the y and z axis to the ghost spawnpoint object.
			Vector3 SpawnPointLocation = new Vector3 (Random.Range (ScreenLeft.x, ScreenRight.x), SpawnPoint.position.y, SpawnPoint.position.z);
			//timer to spawn the objects if the timer drops less than 1 it instantiates an object
			if (GameStart == true) {
				if (Timer < 1) {
					//instantiates an apple.
					Instantiate (Apple, SpawnPointLocation, Quaternion.identity);
					//resets the timer.
					Timer = SpawnTimer;
		
				} 
				// else if the timer is greater than 1 it subtracts 1 per tick (frame)
			else {
					Timer = Timer - Time.deltaTime;
		
				}
				//timer to replay music
				if (SongTimer < 1) {
					AudioSource.PlayClipAtPoint (Theme, new Vector3 (0, -6, 0));
					SongTimer = 28.4f;
				} else {
					SongTimer = SongTimer - Time.deltaTime;
				}
			}
		}
	}

	public void StartGame (){
		// resets till death count just in case
		TillDeath = 3;
		// calls player high score from previous game to be displayed on player gui
		HighScore.text = "HighScore: " + PlayerPrefs.GetString ("HighScoreName").ToString() + " - " + ((int)PlayerPrefs.GetFloat ("HighScore")).ToString ();
		// start game bool toggle
		GameStart = !GameStart;
		//Makes the start menu go away
		StartGUI.enabled = false;
		MainGUI.enabled = true;
		//locks the cursor to the game window...or supposed to...havent tested this as i have not tried a build yet.
		Cursor.lockState = CursorLockMode.Confined;
		//hides the cursor so we dont have to look at its ugly self :-)
		Cursor.visible = false;
		// enables the player and his/her/it's lives
		Player.SetActive (true);
		Apple1.SetActive (true);
		Apple2.SetActive (true);
		Apple3.SetActive (true);
	}

	public void ExitPress (){
		StartbuttonText.enabled = false;
		ExitButtonText.enabled = false;
		ExitMenu.enabled = true;
	}

	public void NoPress(){
		ExitMenu.enabled = false;
		StartbuttonText.enabled = true;
		ExitButtonText.enabled = true;
	}

	public void ExitGame(){
		Application.Quit ();
	}

	public void ExitPressInGame(){
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Player.SetActive (false);
		GameStart = !GameStart;
		InGameExitMenu.enabled = true;
	}

	public void ExitInGamePressYes(){
		// if the players score is higher than the previous players high score, it saves name and score
		if (PlayerPrefs.GetFloat ("HighScore") < GameScore) {
			PlayerPrefs.SetString ("HighScoreName", PlayerName);
			PlayerPrefs.SetFloat ("HighScore", GameScore);
		}
		// loads the beginning scene
		SceneManager.LoadScene (0);
	}

	public void InGameNoPress(){
		InGameExitMenu.enabled = false;
		GameStart = !GameStart;
		Player.SetActive (true);
	}

	public void GameOver (){
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Player.SetActive (false);
		GameStart = !GameStart;
		GameOverGUI.enabled = true;
		QuitYes.SetActive (true);
		if (PlayerPrefs.GetFloat ("HighScore") < GameScore) {
			PlayerPrefs.SetString ("HighScoreName", PlayerName);
			PlayerPrefs.SetFloat ("HighScore", GameScore);
		}
	}

	public void RestartYesPress (){
		QuitYes.SetActive (false);
		RestartGame ();
	}

	public void RestartGame () {
		GameScore = 0;
		StartGame ();
		GameOverGUI.enabled = false;
	}

	public void ClosePress(){
		DirectionsGUI.enabled = false;
		StartbuttonText.enabled = true;
		ExitButtonText.enabled = true;
	}

	public void HowToPlayPress(){
		DirectionsGUI.enabled = true;
		StartbuttonText.enabled = false;
		ExitButtonText.enabled = false;
	}
				
}
