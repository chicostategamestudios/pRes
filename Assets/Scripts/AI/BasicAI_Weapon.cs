﻿//This script was written by James | Last edited by James | Modified on September 5, 2017
//Purpose of this script is to destroy the weapon after a few seconds. Purely for hitbox of AI attacks.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI_Weapon : MonoBehaviour {

    public int damage = 5; //damage of the weapon to hurt the player.
    public float destroy_time = 0.5f;
    public float fall_speed = 6f;

    public bool falling = false; //this is used for the weapon's diagonal attack. If set to true it will move
                                 //downwards as the enemy slashes.

    private PlayerHealth player_hp_script; //this is to access the player's health script

	private Combat player_combat;

    private void OnTriggerEnter(Collider other) //if the weapon hits the player, apply the damage to the player's health script
	{
		if (other.gameObject.tag == "Player") {
			//TJ///
			player_combat = other.GetComponentInParent<Combat> ();
			if (player_combat.is_countering) {
				player_combat.counter_recovery = 0f;
				this.GetComponentInParent<BasicAI> ().player_countering = true;
				this.GetComponentInParent<BasicAI> ().StartCoroutine ("DamageEnemy", 0f);
			} else {
				//TJ_End///
				player_hp_script = other.GetComponentInParent<PlayerHealth> ();
				player_hp_script.DamageReceived (damage);
			}
		}
	}

    private void DestroySelf() //this is to turn off the weapon after it is done with attacking.
    {
        Destroy(this.gameObject);
    }

    private void Start()
    {
        Invoke("DestroySelf", destroy_time); //destroy the game object after 0.5 seconds
    }


    private void FixedUpdate()  //if the sword is set to move downwards, this will apply the downward motion.
    {
        if(falling)
        {
            Vector3 temp = this.transform.position;
            temp.y -= fall_speed * Time.deltaTime;
            this.transform.position = temp;
        }

    }
}
