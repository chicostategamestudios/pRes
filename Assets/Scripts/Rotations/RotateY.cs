using UnityEngine;
using System.Collections;

public class RotateY : MonoBehaviour {

	public float turnSpeed = 25f;
	void FixedUpdate ()
	{
		transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
	}
	
}
