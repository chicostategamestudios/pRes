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

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
        //animator.SetBool("isGrinding", true);

        player = playerObject.GetComponent<PlayerGamepad>();

	}
	
	// Update is called once per frame
	void Update ()
    {
        // Idle
        if (player.grounded == true && player.current_speed == 0)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        // isWalking
        else if (player.grounded == true && player.current_speed > 0 && player.current_speed < 5)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
        }
        // isRunning
        else if (player.grounded == true && player.current_speed >= 5)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
        }
        // isJumping
        if ((Input.GetButton("Controller_A")))
        {
            animator.SetBool("isJumping", true);
        }
        // (Not in the air!)
        if (player.grounded == true)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("inTheAir", false);
        }
        // inTheAir
        else if (player.grounded == false)
        {
            animator.SetBool("inTheAir", true);
        }
        // Air Dash
        if(player.dashing == true)
        {
            animator.SetBool("isAirDashing", true);
        }
        else if (player.dashing == false)
        {
            animator.SetBool("isAirDashing", false);
        }
    }
}
