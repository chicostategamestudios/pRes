/*
 * Author: Alex Gorski
 * 
 * This script is the basis of the saving and loading. It uses serialization to turn any object passed in into json that is then saved
 * to the predetermined file. 
 * 
 * The basic uses of the script: The script will call a file opener to allow the player to pick which save file that they want to load.
 * After a file is opened then it will be used as the main file until a new file type is picked.
 *
 * The SaveData() will accept one Object of any type and save all of the public variables associated with that object.
 * 
 * ChangeList:
 * Alex Gorski 9-12-17: 
 *     Fixed bug that wouldn't increment the internal save files properly. Also, Added public variable to allow
 *     the number of save files to be determined by the user.
 * Alex Gorski 9-7-17: 
 *     Reworked script to handle the files internally and remove any Windows system calls.
 * 
 * 
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveLoad : MonoBehaviour {
	public static SaveLoad S;
	public string _filePath;
	public string _baseSaveFileName;
	public string _baseSaveDirName;
	public string _fileExt;
	public string _fileDir;
	public string _activeDataFileText;
	public string _activeFileName;
	public int _totalSaveFiles;
	public List<string> _fileNames;

	void Awake () 
	{
		// Establish the singleton.
		S = this;
		_fileExt = "pres";
		_baseSaveDirName = "Save Files";
		// This is the base of the file names. Not to be confused with the directory that holds them. This name will have the number appended on to it when saved.
		_baseSaveFileName = "Save File";
		/* Initialize the save files directory. If it doesn't exist then create it.
		 * persistentDataPath will lead us to persistent memory to ensure that we retain the data files. */
		_fileDir = Path.Combine(Application.persistentDataPath, _baseSaveDirName);
		// Make sure that the directory exists and that it has the save files created and ready.
		InitializeDirectory();
	}

	private string BuildCompleteFileName(string fileName)
	// This is just a simple way to combine the file name and path into the correct format.
	{
		return string.Format("{0}.{1}", Path.Combine(_fileDir, fileName), _fileExt);
	}

	private string GetNextFileName()
	/*
	 * This will grab the number of files in the save file directory and then increment that number by 1.
	 * This will be combined with the default save file name and returned.	 
	 */
	{
		int numOfFiles = _fileNames.Count + 1;
		return string.Format ("{0} #{1}", _baseSaveFileName, numOfFiles);
	}

	private void InitializeDirectory()
	/*
	 * 1. This will make sure that our save file directory exists. It will create it if it doesn't.
	 * 2. After the directory is created we will load or create save files until we have reached the
	 *    predetermined amount. If the file doesn't exist then it will be created. The file names
	 *    will then be stored internally.
	 */ 
	{
		// If the directory doesn't exist then make sure to create it.
		if (!Directory.Exists (_fileDir)) 
		{
			Directory.CreateDirectory (_fileDir);
		}
		// Initialize the directory and create save files until we have the predetermined amount.
		int numOfFiles = 0;
		while (numOfFiles < _totalSaveFiles)
		{
			// We will keep creating data files until we have the number that we want.
			string fileName = GetNextFileName ();
			string fileNameWithDir = BuildCompleteFileName (fileName);
			// If the file already exists then just add it as is. No need to create it.
			if (!File.Exists (fileNameWithDir))
				File.Create (fileNameWithDir);
			_fileNames.Add(fileName);
			// Increment the loop control.
			numOfFiles++;
		}
	}

	public int FileCount
	// This will return the count of the current files.
	{
		get{ return _fileNames.Count; }
	}

	public string GetFileName(int indexOfFileName)
	// This will return the list that contains all of the save files that are currently being monitored. Send those names back in to deal with the data.
	{
		string result = string.Empty;
		if (indexOfFileName >= 0 && indexOfFileName < _fileNames.Count) 
		{
			result = _fileNames[indexOfFileName];
		}
		return result;
	}

	public void LoadDataOfObject(Object itemToLoad)
	/*
	 * This function will load data of the passed object. It will search through the loaded data and overwrite any member variables that it finds.
	 * Data that does not belong to the object won't be loaded and member variables that the object has that are not in the saved data won't be affected.
	 */
	{
		if (itemToLoad)
		{
			// This will override all of the class variables of the passed item with whatever is found in the 
			JsonUtility.FromJsonOverwrite(_activeDataFileText, itemToLoad);
		}
	}

	public bool LoadSaveFile(string fileName)
	/* This will load the data from the passed file name and store it internally.
	 * 
	 */
	{
		bool result = false;
		if (!string.IsNullOrEmpty (fileName)) 
		{
			string fullName = BuildCompleteFileName (fileName);
			if (File.Exists (fullName))
			{
				_activeDataFileText = File.ReadAllText (fullName);
				_activeFileName = fileName;
				result = true;
			}
		}
		return result;
	}
		
	public void SaveDataOfObject(Object itemToSave)
	/*
	 * This function will take the passed Object and save all of its public variables to a json file.
	 */	
	{
		if (itemToSave)
		{			
			//Debug.Log ("saving");
			string jsonData = JsonUtility.ToJson(itemToSave);
			string fileName = BuildCompleteFileName (_activeFileName);
			//Debug.Log (fileName);
			File.WriteAllText(fileName, jsonData);
		}
	}
}