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

        // isJumping
        if ((Input.GetButton("Controller_A")) && player.jump_counter < player.jump_limit)
        {
            animator.SetBool("isJumping", true);
        }
        // inTheAir
        if (player.grounded == true)
        {
            animator.SetBool("inTheAir", false);
        }
        else if (player.grounded == false)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("inTheAir", true);
        }
    }
}
