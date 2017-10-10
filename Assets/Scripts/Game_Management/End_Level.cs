using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class End_Level : MonoBehaviour {

	public GameObject Finish_Menu;				//Empty Object holding all of the End of Level Menu
	public Button Next_Level;					//Button indicating "Next Level"
	public Button Retry_Level;					//Button indicating "Retry Level"
	public Button To_MainMenu;					//Button indicating "Return to Main Menu"
	public Button Level_Select;         		//Button indicating "Go to Level Select"	

	private GameObject player;
	public float end_minutes = 0;
	public float end_seconds = 0;
	public float end_millisecs = 0;

	bool finished = false;
	bool paused = false;

	// Use this for initialization
	void Awake () {

		Button NextLevelButton = Next_Level.GetComponent<Button>();         		//Assigns the UI element to its script counterpart
		Button RetryButton = Retry_Level.GetComponent<Button>();					//Assigns the UI element to its script counterpart
		Button MainExitButton = To_MainMenu.GetComponent<Button>();					//Assigns the UI element to its script counterpart
		Button LevelSelectButton = Level_Select.GetComponent<Button>(); 			//Assigns the UI element to its script counterpart

		NextLevelButton.onClick.AddListener(ToNextLevelOnClick);                	//To Next Level script
		RetryButton.onClick.AddListener(RestartOnClick);							//Restart level script
		MainExitButton.onClick.AddListener(MainMenuOnClick);						//Quit to Main Menu
		LevelSelectButton.onClick.AddListener(LevelSelectOnClick);                  //Quit to Level Select

		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {


		if (Input.GetButtonDown("Controller_Start"))
		{
			Time.timeScale = Time.timeScale == 1 ? 0 : 1;
			if (!paused) {
				Finish_Menu.SetActive (true);
				Next_Level.Select ();
				player.GetComponent<PlayerGamepad> ().enabled = false;
				paused = true;
			} else {
				Debug.Log ("monkey");
				Finish_Menu.SetActive (false);
				player.GetComponent<PlayerGamepad> ().enabled = true;
				paused = false;
			}
		}
		
	}

	//used to see when the player enters the exit gate
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") 
		{
			Finish_Level ();
			end_minutes = Start_Level_Timer.Instance.minutes_timer;
			end_seconds = Start_Level_Timer.Instance.seconds_timer;
			end_millisecs = Start_Level_Timer.Instance.millisec_timer;
			float[] finishTime = new float[3];
			finishTime[0] = end_minutes;
			finishTime[1] = end_seconds;
			finishTime[2] = end_millisecs;
			//ScoreSystem.Singleton_ScoreSystem.getCompletionTime (finishTime);
			finished = true;
		}
	}

	//used to activate end level menu
	void Finish_Level()
	{
		Finish_Menu.SetActive (true);
		Next_Level.Select ();
		player.GetComponent<PlayerGamepad> ().enabled = false;
	}

	//takes the player to the next level on selection
	void ToNextLevelOnClick()
	{
		int next_level = SceneManager.GetActiveScene().buildIndex + 1;
		if (next_level < SceneManager.sceneCountInBuildSettings) {
			if (paused) {
				Time.timeScale = 1;
			}
			SceneManager.LoadScene (next_level);
		}
	}

	//Restarts the level on selection
	void RestartOnClick()
	{
		Scene currentScene = SceneManager.GetActiveScene ();
		if (paused) {
			Time.timeScale = 1;
		}
		SceneManager.LoadScene (currentScene.name);
	}

	//Takes player to Main Menu on selection
	void MainMenuOnClick()
	{
		if (paused) {
			Time.timeScale = 1;
		}
		SceneManager.LoadScene ("main_menu");
		//SceneManager.LoadScene ("main_menu");
	}

	//takes player to the Level Select on selection
	void LevelSelectOnClick()
	{
		if (paused) {
			Time.timeScale = 1;
		}
		SceneManager.LoadScene ("main_menu");
		//Scene currentScene = SceneManager.GetActiveScene ();

		//SceneManager.LoadScene (currentScene.name);
	}
}
