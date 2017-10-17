﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArea_Start: MonoBehaviour {

    private BattleArea_End end;

    // Use this for initialization
    void Start () {
        end = transform.parent.GetComponent<BattleArea_End>();
	}
	
	// Update is called once per frame
	void Update () {
            
	}

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            Vector3 TargetPosition = new Vector3(0, end.Wall1.transform.position.y + end.raise_height, 0);
            Vector3 currentPosition = end.Wall1.transform.position;
            Vector3 directionOfTravel = TargetPosition - currentPosition;

            end.Wall1.transform.Translate(0, (directionOfTravel.y * end.speed * Time.deltaTime), 0, Space.World);
            end.Wall2.transform.Translate(0, (directionOfTravel.y * end.speed * Time.deltaTime), 0, Space.World);
            end.Wall3.transform.Translate(0, (directionOfTravel.y * end.speed * Time.deltaTime), 0, Space.World);
            end.Wall4.transform.Translate(0, (directionOfTravel.y * end.speed * Time.deltaTime), 0, Space.World);
            Destroy(gameObject);
        }
    }
}
