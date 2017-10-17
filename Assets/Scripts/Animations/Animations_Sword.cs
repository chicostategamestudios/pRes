//Created by Neil - Last Modified by Thaddeus Thompson - 10/12/17
//This script controls the animation of the player attacks.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Animations;

// This script works with the animation controller for the player character Brian Gideon, and his sword animations.
public class Animations_Sword : MonoBehaviour
{
    public Animator swordAnimator;
	public Animator playerAnimator;
    public GameObject swordObject;
	//changed from lightAttackCombo
    public int attack_combo;     // Max move speed is 48.

	// Use this for initialization
	void Start ()
    {
        swordAnimator = GetComponentInChildren<Animator>();
		playerAnimator = GameObject.Find ("Player").GetComponent<Animator> ();
        attack_combo = 0;
	}

	IEnumerator LightAttackAnim(){
		switch (attack_combo)
		{
		case 0:
			playerAnimator.Play ("Swing1 V1");
			swordAnimator.Play("Swing1 V1");
			attack_combo = 1;
			break;
		case 1:
			playerAnimator.Play ("Swing2 V1");
			swordAnimator.Play("Swing2 V1");
			attack_combo = 2;
			break;
		case 2:
			playerAnimator.Play ("Swing3 V1");
			swordAnimator.Play("Swing3 V1");
			attack_combo = 0;
			break;
		}
		yield return null;
	}

	IEnumerator CounterAnim(){
		Debug.Log ("animate");
		playerAnimator.Play ("Start Air Dash");

		yield return null;
	}

    // Update is called once per frame
    void Update()
    {
		//Going to make these into coroutines that will be called from Combat
        // Light Attack!
       /* if (Input.GetButtonDown("Controller_Y"))
        {
            if(swordAnimator.GetBool("isAttacking") == false)
            {
                swordAnimator.SetBool("isAttacking", true);
            }
            
            switch (attack_combo)
            {
                case 0:
                    swordAnimator.Play("Swing1 V1");
                    attack_combo = 1;
                    break;
                case 1:
                    swordAnimator.Play("Swing2 V1");
                    attack_combo = 2;
                    break;
                case 2:
                    swordAnimator.Play("Swing3 V1");
                    attack_combo = 0;
                    break;
            }
        }
        // Heavy Attack!
        if (Input.GetButtonDown("Controller_B"))
        {
			if(swordAnimator.GetBool("isAttacking") == false)
			{
				swordAnimator.SetBool("isAttacking", true);
			}
			//needs player animator too
            swordAnimator.Play("Swing4 V1");
        }*/
    }
}
