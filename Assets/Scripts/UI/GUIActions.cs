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
		GUIparent = GameObject.Find ("playerUI");
		if (!GUIparent) {
			Debug.LogError ("player ui not found");
		}
	}
	
	// Update is called once per frame
	void Update () {
		//activeText = GameObject.FindGameObjectsWithTag("ActionText");
		activeFrame = GameObject.FindGameObjectsWithTag("ActionFrame");

		if(Input.GetKeyDown("up")) {
			actionGUI(0);
		} else if(Input.GetKeyDown("down")) {
			actionGUI(1);
		}

	}

	public void actionGUI( int index )
	{
		//actionText.text = actionName + points;
		GameObject obj = null;
		if(index == 0) {
			obj = FastAttFramer;
		}
		if(index == 1) {
			obj = StrongAttFramer;
		}
		if(index == 2) {
			obj = CounterFramer;
		}
		if(index == 3) {
			obj = KillFramer;
		}
		Debug.Log ("poo");
		Instantiate(obj, actionPos, Quaternion.identity);
		if(activeFrame != null) {
			foreach(GameObject t in activeFrame) {
				moveFrame = t.transform.position;
				moveFrame.x += .195f;
				moveFrame.y -= t.transform.localScale.y * 2;
				t.transform.position = moveFrame;
			}
		}
	}
}
