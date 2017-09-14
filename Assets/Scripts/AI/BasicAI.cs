//This script was written by James | Last edited by James | Modified on September 5, 2017
//The purpose of this script is to have an enemy chase the player. This is implemented by using a Nav Mesh Agent.
//It will then decide what to do with its list of actions.

/* The way this script works is that it requires a nav mesh agent to follow the player. The ai will use the nav mesh when it is "alerted" 
 * so if you don't want the ai to follow the player anymore set "alerted" to false. Once the AI is within a certain distance
 * to the player, it will randomly decide whether to attack or dodge. If it attacks, it will access the BasicAI_Attack script.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ai_state //state number starts at 0. So for example ai_state[2] is dodge
{
    idle,
    walking,
    dodging,
    attack_1,
    attack_2,
    attack_3,
    staggered,
    dying
} // states of the AI: idle is when the ai will not be moving, walking is when it is on patrol, dodging is when the ai moves to the side
  // attacks 1, 2, and 3 are different ways of the ai's slash attacks. staggered is when the ai is hit in the middle of his attacks
  // dying is when the ai is about to die after reaching 0 health.

public enum dodge_direction //these are the states to determine whether the ai dodges to the left or right.
{
    left,
    right,
    not_dodging
}

public class BasicAI : MonoBehaviour 
{
    public float performing_time = 0.5f; //this is how long the action takes to perform.

    public float dodge_time = 1f; //the amount of time it takes to dodge.

    public float cooldown_action = 0.5f; //how long the AI will take before it performs another action.

    public float turn_speed = 1f; //how fast the AI can rotate to look at the player.

    public float stagger_duration = 0.5f; //how long the AI is staggered for when they are hit.

    public float death_duration = 1f;  //the amount of time after the ai reaches 0 hp to be destroyed.

    private float distance_to_player; //used to keep track between this AI and the player. useful for 
                                      //seeing how far the AI is from the player before deciding
                                      //to chase the player or when to attack/dodge.

    public bool check_to_damage = false; //used for testing, check this to apply damage to the enemy.

    private bool first_alert = false; //used to keep track if the AI has been alerted the first time.

    private bool alerted = false; //once the AI has been alerted, it will start chasing the player.

    private bool performing_action = false; //this is to keep track if the AI is performing an action.

    private bool staggering = false; //used to set the AI into the staggered state

    public int enemy_health = 100; //the health of the enemy.

    public int incoming_damage = 0; //the damage that will be applied to the enemy.

    private dodge_direction dodge = dodge_direction.not_dodging; //AI will decide to move left or right in the dodge.

    private Quaternion look_rotation; //used to rotate the direction the AI is looking at when the player is nearby.

    private Vector3 direction; //also used for the rotation of where the AI is looking at when the player is nearby.

    public GameObject left_dodge, right_dodge; //used to move to these game objects when left or right dodging.

    private BasicAI_Attack attack_script; //to access its attack script.

    private PlayerAttack player_attack; //for the AI to receive damage from the player's attacks.

    private Transform target; //the target the AI will be chasing

    public GameObject basic_ai; //the weapon spawner that is in charge of the weapon swinging.

    ai_state current_state = ai_state.idle; //instantiates the ai with an idle state.



    public IEnumerator Dodge() //this is the dodge of the AI, it will randomly choose left or right dodges.
    {
        alerted = false; //set alert to false to make the AI stop chasing the player to allow dodging.
        dodge = (dodge_direction)Random.Range(0, 2); //randomly picks a direction to dodge. 0 is left, 1 is right.
		yield return new WaitForSeconds(dodge_time); 
        alerted = true; //set alert back to true to make the AI chase the player again.
        dodge = dodge_direction.not_dodging;
		yield return new WaitForSeconds(cooldown_action);
		performing_action = false;  //the AI is done with the action
    }

    public IEnumerator Attack_1() //this is the right swing attack. starts from the left arm position
    {
        attack_script.check_attack_left = true; //this will turn on the attack script swinging to the left.
        
        yield return new WaitForSeconds(performing_time);
        //waits for performing time to be done... then add more things here if needed for after action
		yield return new WaitForSeconds(cooldown_action);
        //this is the time to wait for the next action to be performed.
		performing_action = false;  //the AI is done with the action
    }

    public IEnumerator Attack_2() //this is the left swing attack. starts from the right arm position
    {
        attack_script.check_attack_right = true; //this will turn on the attack script swinging to the right.
        
        yield return new WaitForSeconds(performing_time);
        //waits for performing time to be done... then add more things here if needed for after action
        yield return new WaitForSeconds(cooldown_action);
        //this is the time to wait for the next action to be performed.
        performing_action = false;  //the AI is done with the action
    }

    public IEnumerator Attack_3() //this is the diagonal swing attack. stops from the top right arm position 
    {
        attack_script.check_attack_top = true; //this will turn on the attack script swinging diagonally.
        
        yield return new WaitForSeconds(performing_time);
        //waits for performing time to be done... then add more things here if needed for after action
        yield return new WaitForSeconds(cooldown_action);
        //this is the time to wait for the next action to be performed.
        performing_action = false;  //the AI is done with the action
    }

    //set up for the AI attacks.
    private void Awake()
    {
        basic_ai = this.transform.FindChild("WeaponSpawn").gameObject; //finds the child object to access its script for attacks.
        attack_script = basic_ai.GetComponent<BasicAI_Attack>();  //allows access to the attack script.
        target = GameObject.Find("Player").transform; //sets the target of this AI as the player

    }

    private void OnTriggerEnter(Collider col) //if the player lands an attack on the enemy, then it will apply the damage.
    {
         if(col.gameObject.tag == "PlayerAttack")
        {
            player_attack = col.GetComponent<PlayerAttack>(); //get the player's attack script
            incoming_damage = player_attack.damage;  //incomplete code for the moment until we get the player combat done.
            check_to_damage = true;
        }
    }

    public IEnumerator DamageEnemy() //first will apply damage, and then stagger the enemy for a certain duration
    {
        
        //apply damage and checks if the enemy dies from the damage.
        enemy_health -= incoming_damage;
        attack_script.staggered = true;
        //if they aren't dead, then stagger the basic enemy
        if(enemy_health > 0)
        {
            current_state = ai_state.staggered;
            staggering = true;
            yield return new WaitForSeconds(stagger_duration);
            //after the duration is over, set the enemy back up for attacking again.
            staggering = false;
            attack_script.staggered = false;
            alerted = true;
            current_state = ai_state.idle;
        }
    }

    public IEnumerator Death() //the death coroutine that will play when the enemy hits 0 health.
    {
        //print("the enemy is dying...");
        yield return new WaitForSeconds(death_duration);
        this.gameObject.SetActive(false);
    }


    void FixedUpdate () 
	{
        distance_to_player = Vector3.Distance(target.position, transform.position); //calculate distance to player

        if (check_to_damage) //used for checking purposes. if the enemy is damaged then it must get stunned
        {
            StartCoroutine("DamageEnemy");
            check_to_damage = false;
        }
        
        if (distance_to_player < 50 && !first_alert) //if the player is close enough, this will set the AI to be alerted. 
                                                     //if the enemy is alerted then it will chase the player. AKA aggro range
        {
            alerted = true;
            first_alert = true;
        }

        //if the ai is staggered, then dont do anything for this frame.
        if (staggering)
        {
            current_state = ai_state.staggered;
            alerted = false;
			//Debug.Log ("staggering");
            return;
        }

        if (enemy_health <= 0)
        {
            current_state = ai_state.dying;
            StartCoroutine("Death");
        }



        if (distance_to_player <= 20) //if the player is close, this will keep the AI rotated towards the player
        {
            direction = (target.position - transform.position).normalized;
            look_rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, look_rotation, Time.deltaTime * turn_speed);

            //legacy code below for a really good aim bot.
            /*
            Vector3 look_pos = target.position;
            look_pos.y = transform.position.y;
            Quaternion rotate = Quaternion.LookRotation(look_pos -  transform.position);
            transform.rotation = rotate;
            */
        }

        if (alerted)  //if the AI is aggro, then it will chase the player.
        {
            //telling this object to chase after the target's position.
            transform.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = target.position; 
        }


        if (distance_to_player <= 5.5f && !performing_action) 
		{
            //if the ai is close enough and not already performing an action, then it will pick a random action to do
            //and then perform the selected action.

            performing_action = true;  //the AI is now doing an action
            ai_state current_state = (ai_state)Random.Range(2, 6);  //will pick from dodge through attack 3
			switch (current_state) //based on the choice, do the corresponding coroutines
			{
			case ai_state.dodging:
				StartCoroutine ("Dodge");
				break;
			case ai_state.attack_1:
				StartCoroutine ("Attack_1");
				break;
			case ai_state.attack_2:
				StartCoroutine ("Attack_2");
				break;
            case ai_state.attack_3:
                StartCoroutine("Attack_3");
                break;
            }// at the end of each coroutines, it will set performing_action back to false to allow for a loop if the ai is still within range.
		}


        if (dodge == dodge_direction.left) //this will make the ai move to the left or right when they are in that dodge state.
        {
            transform.position = Vector3.Lerp(transform.position, left_dodge.transform.position, 1f * Time.deltaTime);
        }
        else if (dodge == dodge_direction.right)
        {
            transform.position = Vector3.Lerp(transform.position, right_dodge.transform.position, 1f * Time.deltaTime);
        }
        // the dodge works by moving towards transform positions of empty game objects that are attached to the ai.



        
    }

}