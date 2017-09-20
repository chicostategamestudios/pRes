//Original Author: Alexander Stamatis || Last Edited: Alexander Stamatis | Modified on May 9, 2017
//Smart camera 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    private float distance_from_player, distance_from_player_awake;
    private Vector3 player_pos;
    private void Awake()
    {
        distance_from_player_awake = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);
        print(distance_from_player_awake);

    }

    void FixedUpdate()
    {
        //RaycastHit hit;

        //player_pos = GameObject.Find("Player").transform.position;
        //transform.LookAt(player_pos);

        //distance_from_player = Vector3.Distance(transform.position, player_pos);
        //print(distance_from_player);
        //if (Physics.Raycast(transform.position, transform.forward, out hit, distance_from_player))
        //{
        //    print(hit.transform.gameObject.name);

        //    if (hit.transform.gameObject.name != "Player")
        //    {
        //        transform.position += transform.forward * 8f * Time.fixedDeltaTime;
        //    }
        //    else
        //    {
        //        if (distance_from_player < distance_from_player_awake)
        //        {
        //            transform.position -= transform.forward * 8f * Time.fixedDeltaTime;
        //        }
        //    }
        //    print("There is something in front of the object!");
        //}

    }

    void OnCollisionEnter(Collision col)
    {
        
    }
}
