using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : MonoBehaviour {

    float vert;
	// Update is called once per frame
	void FixedUpdate () {

        transform.position += Vector3.right * 5f * Time.deltaTime;

        vert += 1;
        if (vert < 100)
        {
            transform.position += Vector3.up * Mathf.Log(vert, 0.4f) * Time.deltaTime;
            //transform.position += Vector3.up * Mathf.Sqrt(vert) * Time.deltaTime;
        }

    }



}
