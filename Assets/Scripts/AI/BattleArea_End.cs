using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArea_End : MonoBehaviour {

	bool triggered = false;
	Vector3 frontPos;

	public GameObject trigger_wall;

    public int raise_height;

    public GameObject FrontWall;

    public GameObject Wall2;

    public GameObject Wall3;

    public GameObject BackWall;

    public GameObject endCamera;

    public List<GameObject> enemyList;

	public int enemiesDead = 0;

    [HideInInspector]
    public int speed = 10;

    private float lower_speed;

    private bool onlyOnce = true;

    private Vector3 currentpos;

    private Vector3 lowerpos;
	BasicAI BasicAI_scr;
	AdvancedAI adv_ai_scr;

    // Use this for initialization
    void Awake () {
		enemiesDead = enemyList.Count;
		frontPos = FrontWall.transform.position;
        lower_speed = 20f;
    }

	public void Trigger(){
		if (!triggered) {
			triggered = true;
		}
	}

	// Update is called once per frame
	void Update () {

		if (enemiesDead == 1) 
		{
			
			foreach (GameObject game_object in enemyList) 
			{
				BasicAI_scr = game_object.GetComponent<BasicAI> ();
			}

			if (BasicAI_scr != null) {
				Debug.Log ("not null");
				BasicAI_scr.berserk_mode = true;
			} else if (BasicAI_scr == null) {
				
				foreach (GameObject game_object in enemyList) {
					adv_ai_scr = game_object.GetComponent<AdvancedAI> ();
				}

				adv_ai_scr.berserk_mode = true;
			}

		}

		if(enemiesDead == 0)
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

    public void ResetWalls()
    {
		if (triggered) {
			if (BasicAI_scr != null) {
				BasicAI_scr.berserk_mode = false;
			} else if (adv_ai_scr != null) {
				adv_ai_scr.berserk_mode = false;
			}
			onlyOnce = true;
			enemiesDead = enemyList.Count;
			triggered = false;
			trigger_wall.SetActive (true);

			Wall2.SetActive (true);
			Wall3.SetActive (true);

			Vector3 TargetPosition = new Vector3 (0, FrontWall.transform.position.y - raise_height, 0);
			Vector3 currentPosition = FrontWall.transform.position;
			Vector3 directionOfTravel = TargetPosition - currentPosition;

			FrontWall.transform.position = frontPos;
			//FrontWall.transform.Translate (0, (directionOfTravel.y * speed * Time.deltaTime), 0, Space.World);
			Wall2.transform.Translate (0, (directionOfTravel.y * speed * Time.deltaTime), 0, Space.World);
			Wall3.transform.Translate (0, (directionOfTravel.y * speed * Time.deltaTime), 0, Space.World);
			BackWall.transform.Translate (0, (directionOfTravel.y * speed * Time.deltaTime), 0, Space.World);
		}
    }
}
