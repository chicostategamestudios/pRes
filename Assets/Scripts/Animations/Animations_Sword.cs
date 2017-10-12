using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
//using UnityEditor.Animations;
=======
using UnityEditor.Animations;
>>>>>>> 1f7661afd728c8dd44a0ff53f616fd3bdcf61b36

// This script works with the animation controller for the player character Brian Gideon, and his sword animations.
public class Animations_Sword : MonoBehaviour
{
    public Animator swordAnimator;
    public GameObject swordObject;

    public int lightAttackCombo;     // Max move speed is 48.

	// Use this for initialization
	void Start ()
    {
<<<<<<< HEAD
        swordAnimator = GetComponentInChildren<Animator>();
=======
        swordAnimator = GetComponent<Animator>();
>>>>>>> 1f7661afd728c8dd44a0ff53f616fd3bdcf61b36
        lightAttackCombo = 0;
	}

    // Update is called once per frame
    void Update()
    {
        // Light Attack!
        if (Input.GetButtonDown("Controller_Y"))
        {
            if(swordAnimator.GetBool("isAttacking") == false)
            {
                swordAnimator.SetBool("isAttacking", true);
            }
            
            switch (lightAttackCombo)
            {
                case 0:
                    swordAnimator.Play("Swing1 V1");
                    lightAttackCombo = 1;
                    break;
                case 1:
                    swordAnimator.Play("Swing2 V1");
                    lightAttackCombo = 2;
                    break;
                case 2:
                    swordAnimator.Play("Swing3 V1");
                    lightAttackCombo = 0;
                    break;
            }
        }
        // Heavy Attack!
        if (Input.GetButtonDown("Controller_B"))
        {
<<<<<<< HEAD
            if (swordAnimator.GetBool("isAttacking") == false)
            {
                swordAnimator.SetBool("isAttacking", true);
            }
=======
>>>>>>> 1f7661afd728c8dd44a0ff53f616fd3bdcf61b36
            swordAnimator.Play("Swing4 V1");
        }
    }
}
