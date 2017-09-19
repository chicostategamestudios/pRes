using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script works with the animation controller for the player character Brian Gideon.
public class Animations_BrianGideon : MonoBehaviour
{
    Animator animator;

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();
        //animator.SetBool("isGrinding", true);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
