using UnityEngine;
using System.Collections;

public class RotateX : MonoBehaviour {
	
	
	public float turnSpeed = 50f;
	void FixedUpdate ()
	{
		transform.Rotate(Vector3.right, turnSpeed * Time.deltaTime);
	}
	
}
