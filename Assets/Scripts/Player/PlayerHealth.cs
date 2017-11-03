//This script was written by James | Last edited by James | Modified on September 7, 2017
//The purpose of this script is to manage the player's health and stagger whenever they are hit by an enemy.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerHealth : MonoBehaviour
{
    public int health = 100; //the health of the player.
    public float stagger_duration = 0.5f; //the duration of the stagger when the player is hit by an attack.
    //[HideInInspector]
    public PlayerGamepad player_pad; //needed to access the player's movement script.

    public GameObject damaged_effect;

	[SerializeField]
	private HealthStat Health;

	private Combat my_combat;

    public IEnumerator StaggerPlayer()
    {
        //turn off the player movement to simulate a stun.
        player_pad.SetPlayerMovement(false);
        //create damaged effect
		if (!my_combat.is_countering) {
			GameObject effect = Instantiate (damaged_effect, transform.position, transform.rotation);
			Destroy (effect, 1f);
		}
        yield return new WaitForSeconds(stagger_duration);
        //turn on the player movement to end the stun.
        player_pad.SetPlayerMovement(true);
    }
    
    public void DamageReceived(int damage) //function to apply the damage to the player's health.
    {
        health -= damage;
		Health.CurrentVal -= damage;
        StartCoroutine("StaggerPlayer");
    }

    private void Awake()
    {
		my_combat = this.GetComponentInParent<Combat> ();
        player_pad = GameObject.Find("Player").GetComponent<PlayerGamepad>();

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
