using UnityEngine;
using System.Collections;

public class Rotate: MonoBehaviour {
    /*
     * Alex & Trudid this fuck you
     */

		public float turnSpeed = 50f;
        public Vector3 rotate;
		void FixedUpdate ()
		{
				transform.Rotate(rotate, turnSpeed * Time.deltaTime);
		}

}
	
	