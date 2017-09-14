using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	private GameObject my_player;
	private Combat my_combat;
	private bool attack_type;
	[HideInInspector]public int damage;//used by enemy script to reduce health
	//private float light_damage = 10;
	//private float strong_damage = 15;

    // Use this for initialization
    void Awake () {
		//GetComponent<BoxCollider> ().enabled = false;
		my_player = GameObject.Find ("Player");
		my_combat = my_player.GetComponent<Combat> ();
	}
	
	// Update is called once per frame
	void Update () {
        //Destroy(gameObject, 0.3f);
	}

    //Detect if hitting an Enemy
    public void OnTriggerEnter(Collider col)
    {
		if (col.tag == "Enemy" && my_combat.is_attacking)//checks if target is an enemy and player is attacking
        {
            Debug.Log("Hit Enemy");

			attack_type = my_combat.GetAttackType();//calls function in Combat to determine light or heavy attack
			if (attack_type) {
				damage = 10;
				Debug.Log ("Deal light damage " + damage);
			} else {
				damage = 15;
				Debug.Log ("Deal strong damage " + damage);
			}
        }
		my_combat.is_attacking = false;
    }

}
