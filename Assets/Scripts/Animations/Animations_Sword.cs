using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

// This script works with the animation controller for the player character Brian Gideon, and his sword animations.
public class Animations_Sword : MonoBehaviour
{
    public Animator swordAnimator;
    public GameObject swordObject;

    public int lightAttackCombo;     // Max move speed is 48.

	// Use this for initialization
	void Start ()
    {
        swordAnimator = GetComponent<Animator>();
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
            swordAnimator.Play("Swing4 V1");
        }
    }
}
