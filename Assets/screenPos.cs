using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenPos : MonoBehaviour {

	public float x;
	public float y;

	void Start () {

		if (x < 0) {
			x = Screen.height + (Screen.height * x);
		} else {
			x = Screen.width * x;
		}

		if (y < 0) {
			y = Screen.height + (Screen.height * y);
		} else {
			y = Screen.height * y;
		}

		Vector3 pos = new Vector3(x, y , 0);
		transform.position = pos;

	}

}
