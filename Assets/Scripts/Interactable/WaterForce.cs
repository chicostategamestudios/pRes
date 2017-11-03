using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterForce : MonoBehaviour {

    public float waterforce;

    private Vector3 currentpos;
    private Vector3 targetpos;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponentInParent<PlayerGamepad>())
        {
            //Debug.Log("whoosh");
            float waterspeed = waterforce * Time.deltaTime;       
            
            GameObject player = other.transform.parent.gameObject;
            player.transform.Translate(gameObject.transform.forward * waterspeed);
        }
    }
} 

