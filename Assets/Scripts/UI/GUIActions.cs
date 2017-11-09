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
	private GameObject[] activeFrame;

	[SerializeField]
	private GameObject[] activeText;

	[SerializeField]
	private Vector3 moveFrame;

	public bool itsFastAtt = false;
	public bool itsStrongAtt = false;
	public bool itsCounter = false;
	public bool itsKill = false;

	public GameObject FastAttFramer;
	public GameObject StrongAttFramer;
	public GameObject CounterFramer;
	public GameObject KillFramer;

	// Use this for initialization
	void Start () {
		//actionText = GUIparent.transform.GetChild(0).gameObject.GetComponent<Text>();
		//actionText.text = null;
		//actionText.transform.position = new Vector3(340,160,0);
		actionPos = new Vector3(340, 160, 0);
		
	}
	
	// Update is called once per frame
	void Update () {
		//activeText = GameObject.FindGameObjectsWithTag("ActionText");
		activeFrame = GameObject.FindGameObjectsWithTag("ActionFrame");

		if(Input.GetKeyDown("up")) {
			itsFastAtt = true;
		} else if(Input.GetKeyDown("down")) {
			itsStrongAtt = true;
		}
		if(itsFastAtt == true) {
			actionGUI("Hit", 10, FastAttFramer);
			itsFastAtt = false;
		}
		if(itsStrongAtt == true) {
			actionGUI("Hit", 10, StrongAttFramer);
			itsStrongAtt = false;
		}
		if(itsCounter == true) {
			actionGUI("Hit", 10, CounterFramer);
			itsCounter = false;
		}
		if(itsKill == true) {
			actionGUI("Hit", 10, KillFramer);
			itsKill = false;
		}

	}

	public void actionGUI(string actionName, float points, GameObject obj )
	{
		//actionText.text = actionName + points;
		Instantiate(obj, actionPos, Quaternion.identity);
		if(activeFrame != null) {
			foreach(GameObject t in activeFrame) {
				moveFrame = t.transform.position;
				moveFrame.y -= 50;
				t.transform.position = moveFrame;
			}
		}
	}
}
