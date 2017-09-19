/* Code made by Kyle Partlow | Last edited 9/4/17

	The point of this code is to activate a 3,2,1, GO countdown once the player enters a level.
	This code is also used to indicate when the level is over and the end level screen will pop up
	THIS NEEDS TO BE PUT ON THE OBJECT THAT SIGNIFIES THAT THE LEVEL IS OVER ONCE TOUCHED BY THE PLAYER
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Start_and_End_Level : MonoBehaviour {

	public GameObject Start_Countdown; 			//Empty object holding all of the countdown objects
	public GameObject One_Countdown;			//image indicating "1" in countdown
	public GameObject Two_Countdown;			//image indicating "2" in countdown
	public GameObject Three_Countdown;			//image indicating "3" in countdown
	public GameObject GO_Countdown;				//image indicating "GO" in countdown

	public GameObject Score_and_Time;			//Empty object holding all of the score and timer objects
	public Text Show_Time; 						//Text to show time

	public GameObject Finish_Menu;				//Empty Object holding all of the End of Level Menu
	public Button Next_Level;					//Button indicating "Next Level"
	public Button Retry_Level;					//Button indicating "Retry Level"
	public Button To_MainMenu;					//Button indicating "Return to Main Menu"
	public Button Level_Select;			//Button indicating "Go to Level Select"	

	private GameObject player;
	public float seconds_timer = 0;
	public float minutes_timer = 0;
	private bool finish_reached = false;
	Coroutine co = null;


	//once brought into scene
	void Awake()
	{
		co = StartCoroutine ("Count_Timer");
		player = GameObject.Find ("Player");

		Button NextLevelButton = Next_Level.GetComponent<Button>();         		//Assigns the UI element to its script counterpart
		Button RetryButton = Retry_Level.GetComponent<Button>();					//Assigns the UI element to its script counterpart
		Button MainExitButton = To_MainMenu.GetComponent<Button>();					//Assigns the UI element to its script counterpart
		Button LevelSelectButton = Level_Select.GetComponent<Button>(); 			//Assigns the UI element to its script counterpart

		NextLevelButton.onClick.AddListener(ToNextLevelOnClick);                	//To Next Level script
		RetryButton.onClick.AddListener(RestartOnClick);							//Restart level script
		MainExitButton.onClick.AddListener(MainMenuOnClick);						//Quit to Main Menu
		LevelSelectButton.onClick.AddListener(LevelSelectOnClick);                  //Quit to Level Select

		Show_Time.text = "Time: 0:00";
		StartCoroutine("Countdown");
	}

	public IEnumerator Countdown()
	{
		yield return new WaitForSeconds (0f);
		Start_Countdown.SetActive (true);	//Makes Start_Countdown visbile
		Two_Countdown.SetActive (false);	//Makes countdown 2 invisible
		Three_Countdown.SetActive (false);	//makes countdown 3 invisible
		GO_Countdown.SetActive (false);		//makes countdown GO invisible

		yield return new WaitForSeconds (1f);
		One_Countdown.SetActive (false);	//Makes countdown 1 invisible
		Two_Countdown.SetActive (true);		//Makes countdown 2 visible

		yield return new WaitForSeconds (1f);
		Two_Countdown.SetActive (false);	//Makes countdown 2 invisible
		Three_Countdown.SetActive (true);	//Makes countdown 3 visible

		yield return new WaitForSeconds (1f);
		Three_Countdown.SetActive (false);	//Makes countdown 3 invisible
		GO_Countdown.SetActive (true);		//Makes countdown GO visible

		yield return new WaitForSeconds (1f);
		Start_Countdown.SetActive (false);	//Makes all of Start_Countdown invisible
		Score_and_Time.SetActive (true);	//Makes Score and Timer visible
		//player.GetComponent<PlayerGamepad> ().enabled = true;

	}

	//used to see when the player enters the exit gate
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player") 
		{
			Debug.Log ("BOOP");
			StopCoroutine (co);
			Finish_Level ();
		}
	}

	IEnumerator Count_Timer()
	{
		yield return new WaitForSeconds (4f);
		while (true) {
			
			yield return new WaitForSeconds (1f);
			seconds_timer++;

			if (seconds_timer == 60) {
				seconds_timer = 0;
				minutes_timer++;
			}
			DisplayTimer (seconds_timer, minutes_timer);
		}
	}

	void DisplayTimer(float seconds_timer, float minutes_timer)
	{
		//counts minutes and seconds
		if (seconds_timer < 10 && minutes_timer < 1) {
			Show_Time.text = "Time: 0:0" + seconds_timer.ToString ();
		} else if (seconds_timer >= 10 && seconds_timer < 60 && minutes_timer < 1) {
			Show_Time.text = "Time: 0:" + seconds_timer.ToString ();
		} else if (seconds_timer < 10 && minutes_timer >= 1) {
			Show_Time.text = "Time: " + minutes_timer.ToString () + ":0" + seconds_timer.ToString ();
		} else if (seconds_timer >= 10 && seconds_timer < 60 && minutes_timer >= 1) {
			Show_Time.text = "Time: " + minutes_timer.ToString () + ":" + seconds_timer.ToString ();
		} else if (seconds_timer == 59 && minutes_timer == 59) {
			Show_Time.text = "Time: 59:59";
		}
	}

	//used to activate end level menu
	void Finish_Level()
	{
		finish_reached = true;
		Debug.Log (finish_reached);
		Finish_Menu.SetActive (true);
		Next_Level.Select ();
		//player.GetComponent<PlayerGamepad> ().enabled = false;
	}

	//takes the player to the next level on selection
	void ToNextLevelOnClick()
	{

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
		SceneManager.LoadScene ("main_menu");
	}

	//takes player to the Level Select on selection
	void LevelSelectOnClick()
	{

	}
}