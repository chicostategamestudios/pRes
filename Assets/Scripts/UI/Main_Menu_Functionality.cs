//This script was written by Tony Alessio | Last edited by Tony Alessio | Modified on Sep 14, 2017
/*
SCRIPT DESCRIPTION
	The functionality of this script is to get the UI representation of the button linked with
the function call for that specific button. This script can be placed on any object in the 
scene, but one has been provided named "Main_Menu_Functionality."
	Once attached to an object, click that object and you will then be able to attach the UI 
elements to their respective fields:
- Start_menu_obj -> [Start_Menu]
- PLAY Button -> [PLAY (Button)]
- Play_menu_obj -> [Play_Menu)]
- Options Button -> [Options (Button)]
- Options_menu_obj -> [Options_Menu]
- Exit Button -> [Exit (Button)]
- Quit_menu_obj -> [Quit_BG]
- Quit_yes_Button -> [Quit_Yes (Button)]
- Quit_no_button -> [Quit_No (Button)]
- Level_select_menu_obj -> [Level_Select_Menu]
- Story Button -> [Story (Button)]
- Level 1 Button -> [Level1_Button (Button)]
- Save_select_menu_obj -> [Save_Select_Menu]
- Save 1 Button -> [Save1 (Button)]

	Once the editor knows which button is associated with its UI element, the player will be 
able to click the button to activate the function call. Most function calls simply change the 
current scene to the scene displayed on the UI button.
*/

//Libraries
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Main_Menu_Functionality : MonoBehaviour {
	//Member Variables
	/* This section will allow UI elements to be dragged and dropped in the inspector to assign UI elements to functions */

	[Tooltip("Drag and drop the 'Start_Menu' UI element into this field")]
	//This gameobject will be used to hold the Start_Menu canvas object so that it can be turned on and off
	public GameObject start_menu_obj;		//This will be used by the inspector to dictate which UI element is the "Start_Menu" parent
											//Holding this gameobject variable is necessary in order to hide the "Start_Menu" when any other button is selected
		public GameObject Start_Menu_Obj{ get{ return start_menu_obj;} }		//Getter/Setter for start_menu_obj

	[Tooltip("Drag and drop the 'PLAY' UI element into this field")]
	public Button PLAYButton;       //This will be used by the inspector to dictate which button is the "Play" button
		[Tooltip("Drag and drop the 'Play_Menu' UI element into this field")]
		//This gameobject will be used to hold the Play_Menu canvas object so that it can be turned on and off
		public GameObject play_menu_obj;		//This will be used by the inspector to dictate which UI element is the "Play_Menu" parent
												//Holding this gameobject variable is necessary in order to hide the "Play_Menu" until the PLAY Button is selected

    [Tooltip("Drag and drop the 'Options' UI element into this field")]
	public Button optionsButton;        //This will be used by the inspector to dictate which button is the "Options" button
		[Tooltip("Drag and drop the 'Play_Menu' UI element into this field")]
		//This gameobject will be used to hold the Options_Menu canvas object so that it can be turned on and off
		public GameObject options_menu_obj;		//This will be used by the inspector to dictate which UI element is the "Options_Menu" parent object
												//Holding this gameobject variable is necessary in order to hide the "Options_Menu" until the Options Button is selected
			public GameObject Options_Menu_Obj{ get{ return options_menu_obj;} }		//Getter/Setter for options_menu_obj

    [Tooltip("Drag and drop the 'Exit' UI element into this field")]
	public Button exitButton;		//This will be used by the inspector to dictate which button is the "Exit" button
		[Tooltip("Drag and drop the 'Quit_Menu' UI element into this field")]
		public GameObject quit_menu_obj;		//This will be used by the inspector to dictate which UI element is the "Quit_Menu" parent
												//Holding this gameobject variable is necessary in order to hide the Quit Menu until the Quit Button is selected
		[Tooltip("Drag and drop the 'Quit_Yes' UI element into this field")]
		public Button quit_yes_Button;		//This will be used by the inspector to dictate which button is the "Yes" button, under the Quit parent
		[Tooltip("Drag and drop the 'Quit_No' UI element into this field")]
		public Button quit_no_Button;		//This will be used by the inspector to dictate which button is the "No" button, under the Quit parent

	[Tooltip("Drag and drop the 'Level_Select_Menu' UI element into this field")]
	public GameObject level_select_menu_obj;			//This will be used by the inspector to dictate which UI element is the "Level_Select_Menu" parent
		[Tooltip("Drag and drop the 'Story' UI element into this field")]
		public Button StoryButton;       //This will be used by the inspector to dictate which button is the "Story" button
		[Tooltip("Drag and drop the 'Level1' UI element into this field")]
		public Button Level1Button;       //This will be used by the inspector to dictate which button is the "Level 1" button

	[Tooltip("Drag and drop the 'Level_Select_Menu' UI element into this field")]
	public GameObject save_select_menu_obj;			//This will be used by the inspector to dictate which UI element is the "Save_Select_Menu" parent
		[Tooltip("Drag and drop the 'Save #1' UI element into this field")]
		public Button Save1Button;       //This will be used by the inspector to dictate which button is the "Save1" button



	static public Main_Menu_Functionality Singleton_Main_Menu_Functionality;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Awake() is being used for error checking of incorrectly attatched components in the inspector (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void Awake(){
		Singleton_Main_Menu_Functionality = this;
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Awake() (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	void Start () {
		Button PlayButton = PLAYButton.GetComponent<Button>();                  //Assigns the UI element to its script counterpart
		Button OptionsButton = optionsButton.GetComponent<Button>();            //Assigns the UI element to its script counterpart
		Button ExitButton = exitButton.GetComponent<Button>();                  //Assigns the UI element to its script counterpart

		Button QuitYesButton = quit_yes_Button.GetComponent<Button>();          //Assigns the UI element to its script counterpart
		Button QuitNoButton = quit_no_Button.GetComponent<Button>();            //Assigns the UI element to its script counterpart

		PlayButton.onClick.AddListener(PlayOnClick);                    //Play Script
		OptionsButton.onClick.AddListener(OptionsOnClick);              //Options Script
		ExitButton.onClick.AddListener(QuitOnClick);                    //Quit Script

		QuitYesButton.onClick.AddListener(QuitYesOnClick);              //Quit - Yes Script
		QuitNoButton.onClick.AddListener(QuitNoOnClick);                //Quit - No Script
	}



	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	PLAY - Play Button Functionality (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/* This function will allow the player to click on the Play button and trigger the "Play_Menu" object */
	void PlayOnClick()
    {
		start_menu_obj.SetActive(false);			//Sets Start_Menu to become invisible
		save_select_menu_obj.SetActive(true);		//Sets Save_Select_Menu to become visible
		Save1Button.Select();						//Sets the focus of the cursor to Save1Button
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	PLAY - Play Button Functionality (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Options - Options Button Functionality (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/* This function will allow the player to click on the Options button and move to the "options_menu" scene */
	void OptionsOnClick()
    {
		/* This will make the Options_Menu appear */
		start_menu_obj.SetActive(false);																//Sets start_menu to become invisible
		options_menu_obj.SetActive(true);																//Sets Options_Menu to become visible
		options_menu_obj.transform.parent.GetComponent<Options_UI_Functionality>().BACKButton.Select();	//Sets the focus of the cursor to the BackButton
		/*
		 * This allows the parent of options_menu_obj, which is Options_Menu_Functionality, 
		 * to be called and then the component that is the BACKButton within the 
		 * Options_UI_Functionality script can be accessed.
		 */
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Options - Options Button Functionality (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Exit - Quit Button Functionality (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/* This function will allow the player to click on the Quit button and will create a confirmation prompt */
	void QuitOnClick()
    {
		/* This will make the Quit_Menu appear */
		quit_menu_obj.SetActive(true);			//Sets quit_menu to become visible
		start_menu_obj.SetActive(false);		//Sets start_menu to become invisible
		quit_no_Button.Select ();				//Sets the focus of the cursor to Quit's "No" button
	}

	void QuitYesOnClick()
    {
		/* This will make the game Quit out */
		Application.Quit ();		/* WARNING: Does NOT work while in Unity Editor */
	}

	void QuitNoOnClick()
    {
		/* This will make the game Quit menu disappear */
		quit_menu_obj.SetActive(false);			//Sets quit_menu to become invisible
		start_menu_obj.SetActive(true);			//Sets start_menu to become visible
		exitButton.Select ();					//Sets the focus of the cursor to "Exit" button
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Exit - Quit Button Functionality (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
