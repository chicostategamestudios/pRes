using UnityEngine;
using System.Collections;

public class RotateZ: MonoBehaviour {


		public float turnSpeed = 50f;
		void FixedUpdate ()
		{
				transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime);
		}

}
	
	