//This script was written by Tony Alessio | Last edited by Tony Alessio | Modified on Sep 14, 2017
/*
SCRIPT DESCRIPTION
	The functionality of this script is to get the UI representation of the button linked with
the function call for that specific button. This script can be placed on any object in the 
scene, but one has been provided named "Save_Select_Menu_Functionality."
	Once attached to an object, click that object and you will then be able to attach the UI 
elements to their respective fields:
- Save1 Button -> [Save1 (Button)]
- Save2 Button -> [Save2 (Button)]
- Save3 Button -> [Save3 (Button)]
- Back Button -> [BACK (Button)]
	Once the editor knows which button is associated with its UI element, the player will be 
able to click the button to activate the function call. Most function calls simply change the 
current scene to the scene displayed on the UI button.
*/

//Libraries
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Save_Select_UI_Functionality : MonoBehaviour {
	//Member Variables
	/* This section will allow UI elements to be dragged and dropped in the inspector to assign UI elements to functions */
	[Tooltip("Drag and drop the 'Save #1' UI element into this field")]
	public Button save1Button;              //This will be used by the inspector to dictate which button is the "Save1" button
	[Tooltip("Drag and drop the 'Save #2' UI element into this field")]
	public Button save2Button;				//This will be used by the inspector to dictate which button is the "Save2" button
	[Tooltip("Drag and drop the 'Save #3' UI element into this field")]
	public Button save3Button;             //This will be used by the inspector to dictate which button is the "Save3" button

	[Tooltip("Drag and drop the 'BACK' UI element into this field")]
	public Button backButton;				//This will be used by the inspector to dictate which button is the "Back" button

	void Start () {
		Button Save1Button = save1Button.GetComponent<Button>();	//Assigns the UI element to its script counterpart
		Button Save2Button = save2Button.GetComponent<Button>();	//Assigns the UI element to its script counterpart
		Button Save3Button = save3Button.GetComponent<Button>();	//Assigns the UI element to its script counterpart

		Button BackButton = backButton.GetComponent<Button>();		//Assigns the UI element to its script counterpart

		Save1Button.onClick.AddListener(Save1OnClick);		//Save1 Script
		Save2Button.onClick.AddListener(Save2OnClick);		//Save2 Script
		Save3Button.onClick.AddListener(Save3OnClick);		//Save3 Script

		BackButton.onClick.AddListener(BackOnClick);		//Back to previous menu Script
	}


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Save#OnClick - Save on Click Functions (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/* This function will allow the player to click on the Save1 button and move to the "Play_Menu" scene */
	void Save1OnClick(){SaveSlotOnClick(0);}

	/* This function will allow the player to click on the Save2 button and move to the "Play_Menu" scene */
	void Save2OnClick(){SaveSlotOnClick(1);}

	/* This function will allow the player to click on the Save3 button and move to the "Play_Menu" scene */
	void Save3OnClick(){SaveSlotOnClick(2);}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Save#OnClick - Save on Click Functions (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	SaveSlotOnClick - Save on Click Functions (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	void SaveSlotOnClick(int SaveSlotNumber)
	{
		Main_Menu_Functionality.Singleton_Main_Menu_Functionality.save_select_menu_obj.SetActive(false);	//Sets Save_Menu to become invisible
		Main_Menu_Functionality.Singleton_Main_Menu_Functionality.play_menu_obj.SetActive(true);			//Sets Play_Menu to become visible
		Main_Menu_Functionality.Singleton_Main_Menu_Functionality.StoryButton.Select();						//Sets the Story button as the active cursor
		Debug.Log(SaveLoad.S.GetFileName (SaveSlotNumber));
		SaveLoad.S.LoadSaveFile (SaveLoad.S.GetFileName (SaveSlotNumber));
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	SaveSlotOnClick - Save on Click Functions (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Back - Back Button Functionality (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/* This function will allow the player to click on the BACK button and move to the "Start_Menu" scene */
	void BackOnClick()
	{
		Main_Menu_Functionality.Singleton_Main_Menu_Functionality.play_menu_obj.SetActive(false);		//Sets Play_Menu to become invisible
		Main_Menu_Functionality.Singleton_Main_Menu_Functionality.start_menu_obj.SetActive(true);		//Sets Start_Menu to become visible
		Main_Menu_Functionality.Singleton_Main_Menu_Functionality.PLAYButton.Select();					//Sets the PLAY button as the active cursor
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Back - Back Button Functionality (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



}
