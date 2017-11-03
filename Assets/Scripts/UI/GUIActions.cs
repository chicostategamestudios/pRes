using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUIActions : MonoBehaviour {

	GUIStyle style = new GUIStyle();
	public GameObject GUIparent;
	public Text actionText;
	public Vector3 actionPos;
	[SerializeField]
	private GameObject[] activeText;
	[SerializeField]
	private Vector3 moveText;

	// Use this for initialization
	void Start () {
		actionText = GUIparent.transform.GetChild(0).gameObject.GetComponent<Text>();
		actionText.text = null;
		actionText.transform.position = new Vector3(340,160,0);
		actionPos = new Vector3(340, 160, 0);
		
	}
	
	// Update is called once per frame
	void Update () {
		activeText = GameObject.FindGameObjectsWithTag("ActionText");

		/*
		if(Input.GetKeyDown("up")) {
			actionGUI("Hit ", 10);
		} else if(Input.GetKeyDown("down")) {
			actionGUI("dodge ", 15);
		}
		*/
	}

	public void actionGUI(string actionName, float points )
	{
		actionText.text = actionName + points;
		Instantiate(GUIparent, actionPos, Quaternion.identity);
		if(activeText != null) {
			foreach(GameObject t in activeText) {
				moveText = t.transform.position;
				moveText.y -= 20;
				t.transform.position = moveText;
			}
		}
	}
}
