using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArea_End : MonoBehaviour {

    public GameObject Wall1;

    public GameObject Wall2;

    public GameObject Wall3;

    public GameObject Wall4;

    public List<GameObject> enemyList;
    [HideInInspector]
    public BasicAI basicAI_scr;
    [HideInInspector]
    public AdvancedAI advAI_scr;

    [HideInInspector]
    public int speed = 10;

    public int raise_height;


    // Use this for initialization
    void Start ()
    { 
        Debug.Log(enemyList.Count);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(enemyList.Count == 1)
        {
            foreach (GameObject game_object in enemyList)
            {
                basicAI_scr = game_object.GetComponent<BasicAI>();
            }

            if(basicAI_scr != null)
            {
                basicAI_scr.berserk_mode = true;
            }
            else if (basicAI_scr == null)
            {
                foreach (GameObject game_object in enemyList)
                {
                    advAI_scr = game_object.GetComponent<AdvancedAI>();
                }

                advAI_scr.berserk_mode = true;
            }
        }


		if(enemyList.Count <= 0)
        {
            Destroy(gameObject);
        }
	}
}
