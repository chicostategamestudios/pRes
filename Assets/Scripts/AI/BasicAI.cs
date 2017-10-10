﻿/*This script was written by James | Last edited by James | Modified on October 4, 2017
 *The purpose of this script is to have an enemy chase the player. This is implemented by using a Nav Mesh Agent.
 *It will then decide what to do with its list of actions.
 *

 * The way this script works is that it requires a nav mesh agent to follow the player. The ai will use the nav mesh when it is "alerted" 
 * so if you don't want the ai to follow the player anymore set "alerted" to false. Once the AI is within a certain distance
 * to the player, it will randomly decide whether to attack or dodge. If it attacks, it will access the BasicAI_Attack script.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ai_state //state number starts at 0. So for example ai_state[2] is dodge
{
    walking,
    idle,
    dodging,
    moveback,
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
    [Header("Timers for actions")]
    [Tooltip("This is how long the action takes to perform.")]
    public float performing_time = 0.5f;
    [Tooltip("This is the amount of time the AI dodges for.")]
    public float dodge_time = 0.5f;
    [Tooltip("how long the AI will take before it performs another action.")]
    public float cooldown_action = 1f;
    [Tooltip("The maximum amount of time it will take to perform another action.")]
    public float cooldown_action_max = 3f;
    [Tooltip("The minimum amount of time it will take to perform another action.")]
    public float cooldown_action_min = 1f;
    [Tooltip("The duration of the AI getting knocked back.")]
    public float knockback_duration = 0.2f;
    [Tooltip("The duration of staggering once the knockback is done.")]
    public float stagger_after_knockback_dur = 0.3f;
    [Tooltip("The duration of the AI when it chooses to move backwards.")]
    public float moveback_duration = 0.3f;
    [Tooltip("The amount of time after the ai reaches 0 hp to be destroyed.")]
    public float death_duration = 0.001f;
    private float current_stagger_dur = 0f; //the current time of being staggered.
    public bool getting_knockback; //used to stop the knockback once knockback_duration is done.
   

    private float distance_to_player; //used to keep track between this AI and the player. useful for 
                                      //seeing how far the AI is from the player before deciding
                                      //to chase the player or when to attack/dodge.

    [Header("Attributes")]

    [Tooltip("The maximum speed of the enemy's movement.")]
    public float movement_speed = 15;
    [Tooltip("The rate of acceleration of the enemy. Higher number is faster time to max movement speed.")]
    public float acceleration = 50;
    [Tooltip("This is the speed in which the AI moves when deciding to step backwards.")]
    public float moveback_speed = 5f;
    [Tooltip("This is the speed in which the AI moves when dodging.")]
    public float dodge_speed = 1f;
    [Tooltip("This is how fast the AI can rotate to look at the player.")]
    public float turn_speed = 1f;
    [Tooltip("The health of the AI. It will die when it is 0.")]
    public int enemy_health = 100; //the health of the enemy.
    [Tooltip("The force that pushes back the AI when it is hit.")]
    public float knockback_force = 18f;
    [Tooltip("This is used for testing. Check this to damage the enemy.")]
    public bool check_to_damage = false;


    [Header("Don't touch!")]
    //used to move to the game object when left or right dodging.
    public GameObject left_dodge;
    public GameObject right_dodge;
    [Tooltip("Used to determine what action the AI will take. 35 and below will make the AI dodge, anything else will make it attack.")]
    public int action_selection = 0;

    private bool first_alert = false; //used to keep track if the AI has been alerted the first time.
    [HideInInspector]
    public bool alerted = false; //once the AI has been alerted, it will start chasing the player.
    protected bool performing_action = false; //this is to keep track if the AI is performing an action.
    protected bool staggering = false; //used to set the AI into the staggered state
    [HideInInspector]
    public int incoming_damage = 0; //the damage that will be applied to the enemy.
    protected dodge_direction dodge = dodge_direction.not_dodging; //AI will decide to move left or right in the dodge.
    private Quaternion look_rotation; //used to rotate the direction the AI is looking at when the player is nearby.
    private Vector3 direction; //also used for the rotation of where the AI is looking at when the player is nearby.
    protected BasicAI_Attack attack_script; //to access its attack script for AI attacks.
    private PlayerAttack player_attack; //for the AI to receive damage from the player's attacks.
    private Transform target; //the target the AI will be chasing
    [HideInInspector]
    public GameObject basic_ai; //the weapon spawner that is in charge of the weapon swinging.
    private Vector3 backward_dir; //the backward_dir of the AI. Used whenever the AI needs to get knocked back.
    protected ai_state current_state = ai_state.idle; //instantiates the ai with an idle state.

    protected IEnumerator Idle() //this is the dodge of the AI, it will randomly choose left or right dodges.
    {
        cooldown_action = Random.Range(cooldown_action_min, cooldown_action_max);
        yield return new WaitForSeconds(cooldown_action);
        performing_action = false;  //the AI is done with the action
    }

    protected IEnumerator Dodge() //this is the dodge of the AI, it will randomly choose left or right dodges.
    {
        alerted = false; //set alert to false to make the AI stop chasing the player to allow dodging.
        dodge = (dodge_direction)Random.Range(0, 2); //randomly picks a direction to dodge. 0 is left, 1 is right.
        cooldown_action = Random.Range(cooldown_action_min, cooldown_action_max);
		yield return new WaitForSeconds(dodge_time); 
        alerted = true; //set alert back to true to make the AI chase the player again.
        dodge = dodge_direction.not_dodging; //reset the dodge's direction to no direction otherwise known as not_dodging.
		yield return new WaitForSeconds(cooldown_action);
		performing_action = false;  //the AI is done with the action
    }

    protected IEnumerator Attack_1() //this is the right swing attack. starts from the left arm position
    {
        //move towards the player while the attack goes through.
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 5f;
        cooldown_action = Random.Range(cooldown_action_min, cooldown_action_max);
        //move for half a second, then attack
        yield return new WaitForSeconds(0.5f);
        attack_script.check_attack_left = true;
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 8f;
        yield return new WaitForSeconds(performing_time);
        //waits for performing time to be done... then add more things here if needed for after action
		yield return new WaitForSeconds(cooldown_action);
        //this is the time to wait for the next action to be performed.
		performing_action = false;  //the AI is done with the action
    }

    protected IEnumerator Attack_2() //this is the left swing attack. starts from the right arm position
    {
        //move towards the player while the attack goes through.
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 5f;
        cooldown_action = Random.Range(cooldown_action_min, cooldown_action_max);
        //move for half a second, then attack
        yield return new WaitForSeconds(0.5f);
        attack_script.check_attack_right = true; //this will turn on the attack script swinging to the right.
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 8f;
        yield return new WaitForSeconds(performing_time);
        //waits for performing time to be done... then add more things here if needed for after action
        yield return new WaitForSeconds(cooldown_action);
        //this is the time to wait for the next action to be performed.
        performing_action = false;  //the AI is done with the action
    }

    protected IEnumerator Attack_3() //this is the diagonal swing attack. stops from the top right arm position 
    {
        //move towards the player while the attack goes through.
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 5f;
        cooldown_action = Random.Range(cooldown_action_min, cooldown_action_max);
        //move for half a second, then attack
        yield return new WaitForSeconds(0.5f);
        attack_script.check_attack_top = true; //this will turn on the attack script swinging diagonally.
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 8f;
        yield return new WaitForSeconds(performing_time);
        //waits for performing time to be done... then add more things here if needed for after action
        yield return new WaitForSeconds(cooldown_action);
        //this is the time to wait for the next action to be performed.
        performing_action = false;  //the AI is done with the action
    }

    protected IEnumerator MoveBack()
    {
        //set alert to false to make the AI stop chasing the player to allow moving backwards and set nav mesh speed/acceleration to 0 to stop the AI completely.
        alerted = false;
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().acceleration = 0;
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().velocity = Vector3.zero;
        cooldown_action = Random.Range(cooldown_action_min, cooldown_action_max);
        //start moving backwards
        getting_knockback = true;
        yield return new WaitForSeconds(moveback_duration);
        getting_knockback = false;
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance = 8f;
        alerted = true; //set alert back to true to make the AI chase the player again.
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().acceleration = acceleration;
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = movement_speed;
        yield return new WaitForSeconds(cooldown_action);
        performing_action = false;  //the AI is done with the action
    }

    //set up for the AI attacks and movement.
    private void Awake()
    {
        //find the backward direction for knockbacks.
        backward_dir = transform.TransformDirection(Vector3.back);
        //find the child object to access its script for attacks.
        basic_ai = this.transform.FindChild("WeaponSpawn").gameObject;
        //access to the attack script.
        attack_script = basic_ai.GetComponent<BasicAI_Attack>();
        //sets the target of this AI as the player
        target = GameObject.Find("Player").transform;
        //apply the acceleration variable of this to the nav mesh agent.
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().acceleration = acceleration;
        //apply the speed variable to the nav mesh agent.
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = movement_speed;
    }

	public IEnumerator DamageEnemy(int incoming_damage) //first will apply damage, and then stagger the enemy for a certain duration
    {
		Debug.Log ("damage");
        //apply damage and checks if the enemy dies from the damage.
        enemy_health -= incoming_damage;
        //set the bools to allow knockback and prevent actions/movements.
        getting_knockback = true;
        //attack script's staggered is set to true to uninstantiate any attacks that are already created or about to be instantiated.
        attack_script.staggered = true;
        //change the AI state to staggered for animations.
        current_state = ai_state.staggered;
        //set staggering to true to affect fixedupdate to prevent the ai from doing any actions.
		staggering = true;
        //set alerted to false to stop the AI from following the player with the navmesh.
        alerted = false;
        //stop the ai from moving by setting the acceleration, speed, and velocity to 0.
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().acceleration = 0;
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
        transform.GetComponent<UnityEngine.AI.NavMeshAgent>().velocity = Vector3.zero;
        yield return new WaitForSeconds(0.01f);
    }

    public IEnumerator Death() //the death coroutine that will play when the enemy hits 0 health.
    {
        //print("the enemy is dying...");
        yield return new WaitForSeconds(death_duration);
        this.gameObject.SetActive(false);
    }

	public void reset()
	{
		Debug.Log (transform.name);
		enemy_health = 100;
		first_alert = false;
	}

    void FixedUpdate () 
	{
        //check first if the enemy is staggered.
        if (staggering)
        {
            current_stagger_dur += Time.deltaTime;
            //move the ai state to staggered, and set alerted to false to stop their movement.
            current_state = ai_state.staggered;
            alerted = false;

            //get knocked backwards.
            if (getting_knockback)
            {
                transform.Translate(backward_dir * knockback_force * Time.smoothDeltaTime, Space.Self);
            }

            //once the current_stagger_dur is done with knockback, stop the knockback by turning off getting_knockback.
            if (current_stagger_dur >= knockback_duration && getting_knockback)
            {
                getting_knockback = false;
            }

            //once the stagger duration is up, restore all of the old values of the AI to move and attack.
            if (current_stagger_dur >= knockback_duration + stagger_after_knockback_dur)
            {
                transform.GetComponent<UnityEngine.AI.NavMeshAgent>().acceleration = acceleration;
                transform.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = movement_speed;
                staggering = false;
                alerted = true;
                current_stagger_dur = 0;
                current_state = ai_state.idle;
                attack_script.staggered = false;
            }
        }
        //it is not staggering so run through the usual routines.
        else
        {
            distance_to_player = Vector3.Distance(target.position, transform.position); //calculate distance to player

            //used for checking and testing purposes.
            if (check_to_damage)
            {
                StartCoroutine("DamageEnemy", 11);
                check_to_damage = false;
            }


            if (distance_to_player < 30 && !first_alert) //if the player is close enough, this will set the AI to be alerted. 
                                                         //if the enemy is alerted then it will chase the player. AKA aggro range
            {
                alerted = true;
                first_alert = true;
                //remaining_enemies++; this will be added once kyle finishes his script for battle arena.
            }

            //if the enemy reached 0 hp, it is dead so it will be put into the dying state then go through the death coroutine.
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
            }

            if (getting_knockback)
            {
                transform.Translate(backward_dir * moveback_speed * Time.smoothDeltaTime, Space.Self);
            }


            if (alerted)  //if the AI is aggro, then it will chase the player.
            {
                //telling this object to chase after the target's position, otherwise known as the player's position.
                transform.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = target.position;
            }


            //if the ai is close enough and not already performing an action
            //then it will pick a random action to do
            //and then perform the selected action.
            if (distance_to_player <= 8f && !performing_action)
            {
                Debug.Log("Deciding to attack or move.");
                performing_action = true;  //the AI is now doing an action, used to make sure it is only doing one action.
                //randomly choose between an movement or attack behavior, there is a 35% chance of being a movement behavior and 65% chance of an attack.
                //comment the line below to "control" the AI.
                action_selection = Random.Range(0, 100);

                if(action_selection <= 35)
                {
                    ai_state current_state = (ai_state)Random.Range(2, 4); //randomly select to either dodge or moveback.
                    switch (current_state) //based on the choice, do the corresponding coroutines
                    {
                        case ai_state.dodging:
                            StartCoroutine("Dodge");
                            break;
                        case ai_state.moveback:
                            if (distance_to_player > 6f)
                            {
                                StartCoroutine("Dodge");
                            }
                            else
                            {
                                StartCoroutine("MoveBack");
                            }
                            break;
                    }// at the end of each coroutines, it will set performing_action back to false to allow for a loop if the ai is still within range.
                }
                else
                {
                    ai_state current_state = (ai_state)Random.Range(4, 7); //randomly select within the attack states.
                    switch (current_state) //based on the choice, do the corresponding coroutines
                    {
                        case ai_state.attack_1:
                            StartCoroutine("Attack_1");
                            break;
                        case ai_state.attack_2:
                            StartCoroutine("Attack_2");
                            break;
                        case ai_state.attack_3:
                            StartCoroutine("Attack_3");
                            break;
                    }
                }

                
            }

            // the dodge works by moving towards transform positions of empty game objects that are attached to the ai.
            if (dodge == dodge_direction.left) //this will make the ai move to the left or right when they are in that dodge state.
            {
                transform.position = Vector3.Lerp(transform.position, left_dodge.transform.position, dodge_speed * Time.deltaTime);
            }
            else if (dodge == dodge_direction.right)
            {
                transform.position = Vector3.Lerp(transform.position, right_dodge.transform.position, dodge_speed * Time.deltaTime);
            }
            
        }
    }

}