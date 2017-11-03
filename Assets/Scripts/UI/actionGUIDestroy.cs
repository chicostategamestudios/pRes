using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class actionGUIDestroy : MonoBehaviour {

	public float timer = 0f;
	public Text fadeObj;
	public float fadeColor = 0.8f;


	// Use this for initialization
	void Start () {
		fadeObj = gameObject.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(timer > 1) {
			fadeObj.color = new Color(50,50,50,fadeColor);
			fadeColor -= 0.1f;
			//Destroy(gameObject);	
		}
	}
}
