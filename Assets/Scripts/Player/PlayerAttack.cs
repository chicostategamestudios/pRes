//Created by Unknown - Last Modified by Thaddeus Thompson - 10/06/17
//This script interacts with the Combat and BasicAI scripts and manages hit detection and weapon damage.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	private GameObject my_player;
	private Combat my_combat;
	private BasicAI my_ai;
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
		if (col.tag == "Enemy Hitbox"/* && my_combat.is_attacking*/)//checks if target is an enemy and player is attacking
        {
           // Debug.Log("Hit Enemy");
			my_ai = col.GetComponentInParent<BasicAI> ();

			attack_type = my_combat.GetAttackType();//calls function in Combat to determine light or heavy attack
			if (attack_type) {
				damage = 10;
				my_ai.StartCoroutine ("DamageEnemy", damage);
				my_combat.is_attacking = false;
			//	Debug.Log ("Deal light damage " + damage);
			} else {
				damage = 15;
				my_ai.StartCoroutine ("DamageEnemy", damage);
				my_combat.is_attacking = false;
			//	Debug.Log ("Deal strong damage " + damage);
			}
        }

		//my_combat.weapon_collider.enabled = false;
    }

	/*public void OnTriggerStay(Collider col)
	{
		if (col.tag == "Enemy" && my_combat.is_attacking)//checks if target is an enemy and player is attacking
		{
			Debug.Log("Hit Enemy");
			my_ai = col.GetComponent<BasicAI> ();

			attack_type = my_combat.GetAttackType();//calls function in Combat to determine light or heavy attack
			if (attack_type) {
				damage = 10;
				my_ai.StartCoroutine ("DamageEnemy", damage);
				Debug.Log ("Deal light damage " + damage);
			} else {
				damage = 15;
				my_ai.StartCoroutine ("DamageEnemy", damage);
				Debug.Log ("Deal strong damage " + damage);
			}
		}
		my_combat.is_attacking = false;
	}*/
}
