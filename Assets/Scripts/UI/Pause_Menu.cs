/* Code made by Kyle Partlow | Lasted Edited 9/4/17

	This code is used to bring up a pause screen, giving players options, and stopping the game
	
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour {

	public GameObject Pause_Screen;				//canvas for the Pause Screen
	public GameObject PMenu;					//image of the Pause Menu
	public GameObject Exit_Prompt_Menu;			//image of the Exit Game Menu
	public Button Resume_Game;					//button to resume game
	public Button Checkpoint_Retry;				//button used for checkpoints
	public Button Restart_Level;				//button to indicate restart level
	public Button To_MainMenu;					//button used to go back to main menu
	public Button To_Desktop;					//button used to exit the entire game, back to desktop
	public Button quit_yes_Button;				//buttton to show "yes" in Exit Game Menu
	public Button quit_no_Button;				//Button to show "no" in Exit Game Menu

	private GameObject player;

	void Awake(){
		player = GameObject.Find ("Player");
	}
	void Start () {
		Button ResumeButton = Resume_Game.GetComponent<Button>();                   //Assigns the UI element to its script counterpart
		Button CheckpointButton = Checkpoint_Retry.GetComponent<Button>();          //Assigns the UI element to its script counterpart
		Button RestartButton = Restart_Level.GetComponent<Button>();				//Assigns the UI element to its script counterpart
		Button MainExitButton = To_MainMenu.GetComponent<Button>();					//Assigns the UI element to its script counterpart
		Button DesktopExitButton = To_Desktop.GetComponent<Button>(); 				//Assigns the UI element to its script counterpart

		Button QuitYesButton = quit_yes_Button.GetComponent<Button>();          //Assigns the UI element to its script counterpart
		Button QuitNoButton = quit_no_Button.GetComponent<Button>();            //Assigns the UI element to its script counterpart


		ResumeButton.onClick.AddListener(ResumeOnClick);                    	//Resume Script
		CheckpointButton.onClick.AddListener(CheckpointOnClick);                //Checkpoint system script
		RestartButton.onClick.AddListener(RestartOnClick);						//Restart level script
		MainExitButton.onClick.AddListener(QuitToDesktop);						//Quit to Main Menu
		DesktopExitButton.onClick.AddListener(QuitToDesktop);                   //Quit to Desktop Script

		QuitYesButton.onClick.AddListener(QuitDesktopYesOnClick);              //Quit - Yes Script
		QuitNoButton.onClick.AddListener(QuitDesktopNoOnClick);                //Quit - No Script
	}
		
	// Update is called once per frame
	void Update () 
	{
		//pauses game once START on the controller is pressed
		if (Input.GetButtonDown("Controller_Start")) 							
		{
			PauseGame ();
		}
	}

	//when paused game time stops
	public void PauseGame()
	{
		if (Pause_Screen.gameObject.activeInHierarchy == false) 
		{
			Pause_Screen.SetActive (true);				//Set pause_screen to visible
			Exit_Prompt_Menu.SetActive (false);	//Set quit_menu to invisible
			Resume_Game.Select();
			player.GetComponent<PlayerGamepad> ().enabled = false;

		} 
		else 
		{
			Pause_Screen.SetActive (false);				//Quit Pause_screen
			player.GetComponent<PlayerGamepad> ().enabled = true;
		}
	}

	//Resumes game once selected on
	void ResumeOnClick()
	{
		Pause_Screen.SetActive (false);					//sets pause_screen to invisble
		Time.timeScale = 1;								//object can move again
	}

	//Should take players back to the last encountered checkpoint once selected
	void CheckpointOnClick()
	{
		Time.timeScale = 1;
	}

	//Restarts level from beginning once selected
	void RestartOnClick()
	{
		Scene currentScene = SceneManager.GetActiveScene ();

		SceneManager.LoadScene (currentScene.name);

		Time.timeScale = 1;
	}

	/* This function will allow the player to click on the Quit button and will create a confirmation prompt */
	void QuitToMainMenu()
	{
		/* This will make the Quit_Menu appear */
		Exit_Prompt_Menu.SetActive(true);			//Sets quit_menu to become visible
		PMenu.SetActive(false);						//Sets pause_menu to become invisible
		quit_no_Button.Select ();					//Sets the focus of the cursor to Quit's "No" button
	}
		
	void QuitMainMenuYesOnClick()
	{
		/* This will make the game Quit out */
		SceneManager.LoadScene ("main_menu");
	}

	void QuitMainMenuNoOnClick()
	{
		/* This will make the game Quit menu disappear */
		Exit_Prompt_Menu.SetActive(false);			//Sets quit_menu to become invisible
		PMenu.SetActive(true);						//Sets pause_menu to become visible
		To_Desktop.Select ();						//Sets the focus of the cursor to "Exit" button
	}

	/* This function will allow the player to click on the Quit button and will create a confirmation prompt */
	void QuitToDesktop()
	{
		/* This will make the Quit_Menu appear */
		Exit_Prompt_Menu.SetActive(true);			//Sets quit_menu to become visible
		PMenu.SetActive(false);						//Sets pause_menu to become invisible
		quit_no_Button.Select ();					//Sets the focus of the cursor to Quit's "No" button
	}

	void QuitDesktopYesOnClick()
	{
		/* This will make the game Quit out */
		Application.Quit ();
	}

	void QuitDesktopNoOnClick()
	{
		/* This will make the game Quit menu disappear */
		Exit_Prompt_Menu.SetActive(false);			//Sets quit_menu to become invisible
		PMenu.SetActive(true);						//Sets pause_menu to become visible
		To_Desktop.Select ();						//Sets the focus of the cursor to "Exit" button
	}
}
