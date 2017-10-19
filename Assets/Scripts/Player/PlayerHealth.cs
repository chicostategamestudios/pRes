//This script was written by James | Last edited by James | Modified on September 7, 2017
//The purpose of this script is to manage the player's health and stagger whenever they are hit by an enemy.


using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100; //the health of the player.
    public int originalHealth; // Tracks orginal health of player
    private bool is_alive = true; //this is to keep track if the player is alive.
    public float stagger_duration = 0.5f; //the duration of the stagger when the player is hit by an attack.
    //[HideInInspector]
    public PlayerGamepad player_pad; //needed to access the player's movement script.
    public bool isBleeding = false; //Needed to tell when the player is bleeding and when they aren't
    public BleedScreenScript bScreen;


	[SerializeField]
	private HealthStat Health;

    public IEnumerator StaggerPlayer()
    {
        //turn off the player movement to simulate a stun.
        player_pad.SetPlayerMovement(false);
        yield return new WaitForSeconds(stagger_duration);
        //turn on the player movement to end the stun.
        player_pad.SetPlayerMovement(true);
       
    }
    
    public void DamageReceived(int damage) //function to apply the damage to the player's health.
    {
        health -= damage;
		Health.CurrentVal -= damage;
        StartCoroutine("StaggerPlayer");
        if(health > 0) //this is to display the health of the player in console.
        { 
            //print("Player health is now: " + health);
        }

        if (health <= 0 && is_alive)
        {
            is_alive = false;
            //this.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        player_pad = GameObject.Find("Player").GetComponent<PlayerGamepad>();
        originalHealth = health;
        bScreen = GameObject.Find("BleedScreen").GetComponent<BleedScreenScript>();  
    }
    private void OnGUI()
    {
        if (health <= originalHealth*0.8f && health > 0){
            isBleeding = true;
        }
        if (health <= 0 || health > originalHealth*0.8f)
        {
            isBleeding = false;
        }
        if (isBleeding == true){

            bScreen.screenBleed = true;
        }
        if (isBleeding == false)
        {
            bScreen.screenBleed = false;
        }
    }

	void Start(){
		//HealthStat.s = GameObject.FindGameObjectWithTag ("healthBar").GetComponent<HealthStat> ();
		Health.Initialize();
	}

	public void Heal(){
		health = 100;
		Health.CurrentVal = 100;
	}
}
