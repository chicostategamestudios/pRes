//This script was written by Tony Alessio | Last edited by Tony Alessio | Modified on Sep 14, 2017
/*
SCRIPT DESCRIPTION
	The functionality of this script is to get the UI representation of the button linked with
the function call for that specific button. This script can be placed on any object in the 
scene, but one has been provided named "Level_Select_Menu_Functionality."
	Once attached to an object, click that object and you will then be able to attach the UI 
elements to their respective fields:
- Level_Select_Menu_Obj -> [Level_Select_Menu]
- Level 1_Button -> [Level1_Button (Button)]
- Level 2_Button -> [Level2_Button (Button)]
- Level 3_Button -> [Level3_Button (Button)]
- Level 4_Button -> [Level4_Button (Button)]
- Level 5_Button -> [Level5_Button (Button)]

- BACK Button -> [BACK (Button)]

	Once the editor knows which button is associated with its UI element, the player will be 
able to click the button to activate the function call. Most function calls simply change the 
current scene to the scene displayed on the UI button.
*/

//Libraries
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Level_Select_Menu_Functionality : MonoBehaviour {
	//Member Variables
	/* This section will allow UI elements to be dragged and dropped in the inspector to assign UI elements to functions */

	[Tooltip("Drag and drop the 'Level_Select_Menu' UI element into this field")]
	//This gameobject will be used to hold the Level_Select_Menu canvas object so that it can be turned on and off
	public GameObject Level_Select_Menu_obj;		//This will be used by the inspector to dictate which UI element is the "Level_Select_Menu" parent
	//Holding this gameobject variable is necessary in order to hide the "Level_Select_Menu" until the MODE Button is selected
		[Tooltip("Drag and drop the 'Level1' UI element into this field")]
		public Button level1_Button;       //This will be used by the inspector to dictate which button is the "Level 1" button
		[Tooltip("Drag and drop the 'Level2' UI element into this field")]
		public Button level2_Button;       //This will be used by the inspector to dictate which button is the "Level 2" button
		[Tooltip("Drag and drop the 'Level3' UI element into this field")]
		public Button level3_Button;       //This will be used by the inspector to dictate which button is the "Level 3" button
		[Tooltip("Drag and drop the 'Level4' UI element into this field")]
		public Button level4_Button;       //This will be used by the inspector to dictate which button is the "Level 4" button
		[Tooltip("Drag and drop the 'Level5' UI element into this field")]
		public Button level5_Button;       //This will be used by the inspector to dictate which button is the "Level 5" button

	[Tooltip("Drag and drop the 'BACK' UI element into this field")]
	public Button BACKButton;		//This will be used by the inspector to dictate which button is the "BACK" button

	void Start () {
		Button Level1_Button = level1_Button.GetComponent<Button>();	//Assignes the UI element to its script counterpart
		Button Level2_Button = level2_Button.GetComponent<Button>();	//Assignes the UI element to its script counterpart
		Button Level3_Button = level3_Button.GetComponent<Button>();	//Assignes the UI element to its script counterpart
		Button Level4_Button = level4_Button.GetComponent<Button>();	//Assignes the UI element to its script counterpart
		Button Level5_Button = level5_Button.GetComponent<Button>();	//Assignes the UI element to its script counterpart

		Button BackButton = BACKButton.GetComponent<Button>();			//Assigns the UI element to its script counterpart

		Level1_Button.onClick.AddListener(Level1OnClick);				//Load Level 1 Script
		Level2_Button.onClick.AddListener(Level2OnClick);				//Load Level 2 Script
		Level3_Button.onClick.AddListener(Level3OnClick);				//Load Level 3 Script
		Level4_Button.onClick.AddListener(Level4OnClick);				//Load Level 4 Script
		Level5_Button.onClick.AddListener(Level5OnClick);				//Load Level 5 Script

		BackButton.onClick.AddListener(BackOnClick);					//Back to previous menu script
	}


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Level 1 - Loads Level 1 (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/* This function will allow the player to click on the "Level 1" button and move to the "Level_1" scene */
	void Level1OnClick()
	{
		SceneManager.LoadScene("Level1");		//Change this to whatever scene is the "Level1" scene
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Level 1 - Loads Level 1 (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Level 2 - Loads Level 2 (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/* This function will allow the player to click on the "Level 2" button and move to the "Level_2" scene */
	void Level2OnClick()
	{
		SceneManager.LoadScene("Level2");		//Change this to whatever scene is the "Level2" scene
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Level2OnClick() - (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Level 3 - Loads Level 3 (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/* This function will allow the player to click on the "Level 3" button and move to the "Level_3" scene */
	void Level3OnClick()
	{
		SceneManager.LoadScene("Level3");		//Change this to whatever scene is the "Level3" scene
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Level 3 - Loads Level 3 (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Level 4 - Loads Level 4 (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/* This function will allow the player to click on the "Level 4" button and move to the "Level_4" scene */
	void Level4OnClick()
	{
		SceneManager.LoadScene("Level4");		//Change this to whatever scene is the "Level4" scene
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Level 4 - Loads Level 4 (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Level 5 - Loads Level 5 (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/* This function will allow the player to click on the "Level 5" button and move to the "Level_5" scene */
	void Level5OnClick()
	{
		SceneManager.LoadScene("Level5");		//Change this to whatever scene is the "Level5" scene
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Level 5 - Loads Level 5 (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Back - Back Button Functionality (BEGIN)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/* This function will allow the player to click on the BACK button and move to the "play_menu" scene */
	void BackOnClick()
	{
		Main_Menu_Functionality.Singleton_Main_Menu_Functionality.level_select_menu_obj.SetActive(false);	//Sets Level_Select_Menu to become invisible
		Main_Menu_Functionality.Singleton_Main_Menu_Functionality.play_menu_obj.SetActive(true);			//Sets play_menu to become visible
		Main_Menu_Functionality.Singleton_Main_Menu_Functionality.StoryButton.Select();						//Sets the Story button as the active cursor
	}
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	Back - Back Button Functionality (END)
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



}
