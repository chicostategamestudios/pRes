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
	void FixedUpdate () {

		if (Input.GetKeyUp ("space")) {
			Scene currentScene = SceneManager.GetActiveScene ();

			SceneManager.LoadScene (currentScene.name);
		}

		if (finished) {
			if (Input.GetButtonUp ("Controller_A")) {
				Scene currentScene = SceneManager.GetActiveScene ();

				SceneManager.LoadScene (currentScene.name);
			}
		}
	}

	//used to see when the player enters the exit gate
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player") 
		{
			Finish_Level ();
			end_minutes = Start_Level_Timer.Instance.minutes_timer;
			end_seconds = Start_Level_Timer.Instance.seconds_timer;
			end_millisecs = Start_Level_Timer.Instance.millisec_timer;
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
		Scene currentScene = SceneManager.GetActiveScene ();

		SceneManager.LoadScene (currentScene.name);
	}

	//Restarts the level on selection
	void RestartOnClick()
	{
		Scene currentScene = SceneManager.GetActiveScene ();

		SceneManager.LoadScene (currentScene.name);
	}

	//Takes player to Main Menu on selection
	void MainMenuOnClick()
	{
		Scene currentScene = SceneManager.GetActiveScene ();

		SceneManager.LoadScene (currentScene.name);
		//SceneManager.LoadScene ("main_menu");
	}

	//takes player to the Level Select on selection
	void LevelSelectOnClick()
	{
		Scene currentScene = SceneManager.GetActiveScene ();

		SceneManager.LoadScene (currentScene.name);
	}
}
