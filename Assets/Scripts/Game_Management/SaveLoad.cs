/*
 * Author: Alex Gorski
 * 
 * This script is the basis of the saving and loading. It uses serialization to turn any object passed in into json that is then saved
 * to the predetermined file. 
 * 
 * The basic uses of the script: Once a file is loaded, it will be parsed and have its data stored internally. You can save any object
 * in the scene or load the data of any object previously saved.
 *
 * The SaveData() will accept one Object of any type and save all of the public variables associated with that object.
 * 
 * ChangeList:
 * Alex Gorski 9-14-17:
 *		Rewrote this script so that it could handle mutliple layers of objects. It will now be able to store any number of
 *		objects within any number of levels.
 * Alex Gorski 9-12-17: 
 *      Fixed bug that wouldn't increment the internal save files properly. Also, Added public variable to allow
 *      the number of save files to be determined by the user.
 * Alex Gorski 9-7-17: 
 *      Reworked script to handle the files internally and remove any Windows system calls.
 * 
 * 
 */ 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System.IO;

public class SaveLoad : MonoBehaviour {
	public static SaveLoad S;
	public string _filePath;
	public string _baseSaveFileName;
	public string _baseSaveDirName;
	public string _fileExt;
	public string _fileDir;
	public string _activeFileName;
	public int _totalSaveFiles;
	public int _currentLevelLoaded;
	public List<string> _fileNames;
	public Dictionary<int, Dictionary<string, string>> _levelData;
	public Dictionary<string, string> _objectData;

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
		_levelData = new Dictionary<int, Dictionary<string, string>> ();
		_objectData = new Dictionary<string, string> ();
	}

	private string BuildCompleteFileName(string fileName)
	// This is just a simple way to combine the file name and path into the correct format.
	{
		return string.Format("{0}.{1}", Path.Combine(_fileDir, fileName), _fileExt);
	}

	private string GetLevelSaveData()
	/*
	 * This will convert all of the internal data into a format that we can use for saving.
	 */ 
	{
		// Initialize the data with an empty string.
		string result = "";
		// Each value in the level dictionary will have the level number and a dictionary containing all of the objects associated with that level.
		foreach (KeyValuePair<int, Dictionary<string, string>> item in _levelData) 
		{
			// Each level iteration will have the level number first followed by the objects saved during that level.
			// The level data will only be the level number at first.
			result = string.Format("{0}{1}", result, item.Key.ToString());
			// Each level will have all of the objects saved in another dictionary. We'll iterate through that.
			foreach (KeyValuePair<string, string> objItem in item.Value) 
			{
				// Separate all of the object items with pipes. This will help us to easily differentiate between them.
				result = string.Format("{0}|{1}|{2}|", result, objItem.Key, objItem.Value);
			}
			// End each level with an asterisk. This will help us to easily differentiate between the levels when parsing.
			result = result + "*";
		}
		return result;
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
	// This will return the name of the file stored in the index. Send this back when loading files.
	{
		string result = string.Empty;
		if (indexOfFileName >= 0 && indexOfFileName < _fileNames.Count) 
		{
			result = _fileNames[indexOfFileName];
		}
		return result;
	}

	public void LoadDataOfObject(int currentLevel, Object itemToLoad)
	/*
	 * This function will load data of the passed object. It will search through the loaded data and overwrite any member variables that it finds.
	 * Data that does not belong to the object won't be loaded and member variables that the object has that are not in the saved data won't be affected.
	 */
	{
		if (itemToLoad)
		{
			// Check the level dictionary to see if we have the object saved for the current level. If we don't then there's nothing more that we can do.
			_levelData.TryGetValue(_currentLevelLoaded, out _objectData);
			string jsonData;
			if (_objectData.TryGetValue(itemToLoad.name, out jsonData))
			{
				// This will overwrite the passed object with any data that we have saved. This will
				// dynamically search through the objects variables and populate any variables that have a matching name with what
				// we have saved. If the variable doesn't exist in the object but does in the data, nothing happens. If the object has a variable that the
				// data doesn't know about, nothing happens.
				JsonUtility.FromJsonOverwrite(jsonData, itemToLoad);
			}
		}
	}

	public bool LoadSaveFile(string fileName)
	/*
	 * This will load the file from the passed file name and format it in the way that we want. Each file
	 * is formatted by this script so that we know exactly where everything is at.
	 */ 
	{
		bool result = false;
		if (!string.IsNullOrEmpty (fileName)) 
		{
			string fullName = BuildCompleteFileName (fileName);
			if (File.Exists (fullName))
			{
				string fileText = File.ReadAllText (fullName);
				// Each level will be split up by searching for an asterisk. We have defined the save data so that an asterisk is all that
				// separates the different level's save data. This will allow us to easily iterate through it all.
				string[] levelDataItems = fileText.Split (new char[]{'*'}, System.StringSplitOptions.RemoveEmptyEntries);
				foreach (string levelDataItem in levelDataItems) 
				{
					// For each level, we will have the value be another dictionary. This dictionary will handle all of the objects of the level.
					Dictionary<string, string> objectDataDic = new Dictionary<string, string>();
					// Each item within the level data will be separated by a pipe. This allows us to easily iterate through all of the saved items
					// for each level.
					string[] dataObjectItems = levelDataItem.Split (new char[]{'|'}, System.StringSplitOptions.RemoveEmptyEntries);
					// The first item will be the level number.
					int levelValue = int.Parse(dataObjectItems[0]);
					string dataObjectKey = "";
					string dataObjectValue = "";
					// All other items will either be the object name (odd indices) or the json data (even indices)
					for (int i = 1; i < dataObjectItems.Length; i++) 
					{
						// Odd numbers for the key.
						if (i % 2 != 0)
							dataObjectKey = dataObjectItems[i];
						// Even numbers for the data and storage in the dictionary.
						else
						{
							// This method works because of how we defined the save file data. We will always have the name of the object
							// come before the data, so dataObjectKey will always be initialized at this point. It never hurts to check though, 
							// in case some hooligan messes with the save file.
							if (!string.IsNullOrEmpty(dataObjectKey))
							{
								dataObjectValue = dataObjectItems[i];
								objectDataDic[dataObjectKey] = dataObjectValue;
							}
						}
					}
					// The dictionary that stores the level data will have the dictionary that stores the objects saved in the level as its value.
					_levelData[levelValue] = objectDataDic;
				}
				_activeFileName = fileName;
				result = true;
			}
		}
		return result;
	}
			
	public void SaveDataOfObject(int currentLevel, Object itemToSave)
	/*
	 * This function will take the passed Object and save all of its public variables to a json file.
	 */	
	{
		if (itemToSave)
		{			
			string jsonData;
			string levelData;
			string fileName;
			jsonData = JsonUtility.ToJson (itemToSave);
			// Check to see if we already have data for the current level.
			if (_levelData.TryGetValue(currentLevel, out _objectData))
				_objectData[itemToSave.name] = jsonData;
			else
			{
				// If we don't then add a new area of the data objects.
				Dictionary<string, string> tempDic = new Dictionary<string, string>();
				tempDic[itemToSave.name] = jsonData;
				_levelData[_currentLevelLoaded] = tempDic;
			}
			// Grab the save data string for the entire game. This will have all the objects for each level stored within it.
			levelData = GetLevelSaveData();
			fileName = BuildCompleteFileName (_activeFileName);
			File.WriteAllText(fileName, levelData);
		}
	}
}