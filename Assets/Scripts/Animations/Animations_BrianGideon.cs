using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

// This script works with the animation controller for the player character Brian Gideon.
public class Animations_BrianGideon : MonoBehaviour
{
    public Animator animator;
    public GameObject playerObject;
    public PlayerGamepad player;

    float maxWalkSpeed;

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();    // Here we can refer to the animator controller we need to use.
        player = playerObject.GetComponent<PlayerGamepad>();    // We will also need certain variables from the gamepad script.

        maxWalkSpeed = 10F;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Most of these animation conditions use the boolean parameters set up in the animation controller.

        // Idle (neither walking nor running, not there is no Idle parameter in the animation controller)
        if (player.grounded == true && player.current_speed == 0)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }
        // isWalking
        else if (player.grounded == true && player.current_speed > 0 && player.current_speed <= maxWalkSpeed)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
        }
        // isRunning
        else if (player.grounded == true && player.current_speed > maxWalkSpeed)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
        }

        // isJumping
        if ((Input.GetButton("Controller_A") && player.grounded == true))
        {
            animator.SetBool("isJumping", true); 
        }

        // Landed!
        if (player.grounded == true)
        {
            animator.SetBool("inTheAir", false);
        }
        // inTheAir (not grounded)
        else if (player.grounded == false)
        {
            animator.SetBool("inTheAir", true);
            animator.SetBool("isJumping", false);   // The player is done jumping the moment his feet leaves the ground. 
        }

        // Air Dashing!
        if(player.dashing == true)
        {
            animator.SetBool("isAirDashing", true);
        }
        // (not) Air Dashing!
        else if (player.dashing == false)
        {
            animator.SetBool("isAirDashing", false);
        }

        // Rail Grinding
        if (player.grinding == true)
        {
            animator.SetBool("isGrinding", true);
        }
        else if (player.grinding == false)
        {
            animator.SetBool("isGrinding", false);
        }
    }
}
