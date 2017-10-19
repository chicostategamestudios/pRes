using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArea_End : MonoBehaviour {

    public GameObject endCamera;

    public int raise_height;

    public GameObject FrontWall;

    public GameObject Wall2;

    public GameObject Wall3;

    public GameObject BackWall;

    public List<GameObject> enemyList;

    [HideInInspector]
    public int speed = 10;

    private float lower_speed;

    private bool onlyOnce = true;

    private Vector3 currentpos;

    private Vector3 lowerpos;

    //private Quaternion cameraRotation;

    //private Vector3 relativePos;

    // Use this for initialization
    void Awake () {
        //Debug.Log(enemyList.Count);
        //relativePos = RecenterTarget.transform.position - Camera.main.transform.position;
        //cameraRotation = Quaternion.LookRotation(relativePos) * Quaternion.Euler(0, 90, 0);
        lower_speed = 20f;
        
    }
	
	// Update is called once per frame
	void Update () {
		if(enemyList.Count <= 0)
        {
            if (onlyOnce)
            {
                onlyOnce = false;
                StartCoroutine("EndBattle");
                Wall2.SetActive(false);
                Wall3.SetActive(false);

            }
            currentpos = FrontWall.transform.position;
            lowerpos = new Vector3(FrontWall.transform.position.x, FrontWall.transform.position.y - raise_height, FrontWall.transform.position.z);
            FrontWall.transform.position = Vector3.MoveTowards(currentpos, lowerpos, Time.deltaTime * lower_speed);


        }
	}

    IEnumerator EndBattle()
    {
        endCamera.SetActive(true);
        yield return new WaitForSeconds(3f);
        endCamera.SetActive(false);
    }
}
