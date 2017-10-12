using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
//using UnityEditor.Animations;
=======
using UnityEditor.Animations;
>>>>>>> 1f7661afd728c8dd44a0ff53f616fd3bdcf61b36

// This script works with the animation controller for the player character Brian Gideon, and his sword animations.
public class Animations_BrianGideon : MonoBehaviour
{
    public Animator playerAnimator;
<<<<<<< HEAD
    AnimatorStateInfo currentStateInfo;
    string currentStateName;

=======
>>>>>>> 1f7661afd728c8dd44a0ff53f616fd3bdcf61b36
    public GameObject playerObject;
    public GameObject swordObject;
    public PlayerGamepad player;

    int lightAttackCombo;     // Max move speed is 48.


	// Use this for initialization
	void Start ()
    {
        player = playerObject.GetComponent<PlayerGamepad>();    // We will also need certain variables from the gamepad script.
        swordObject = GetComponent<GameObject>();

        playerAnimator = GetComponent<Animator>();    // Here we can refer to the playerAnimator controller we need to use.

        lightAttackCombo = 0;
	}

<<<<<<< HEAD
    void OnStateEnter()
    {
        currentStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        //currentStateInfo = currentStateInfo[0].clip.name;
    }

    // Update is called once per frame
    void Update()
    {
        //Fetch the current Animation clip information for the base layer
       // currentStateInfo = this.playerAnimator.GetCurrentAnimatorClipInfo(0);
        //Access the Animation clip name
      //  currentStateInfo = currentClipInfo[0].clip.name;


=======
    // Update is called once per frame
    void Update()
    {
>>>>>>> 1f7661afd728c8dd44a0ff53f616fd3bdcf61b36
        // Most of these animation conditions use the boolean parameters set up in the animation controller.
        if (player.current_speed > 0)
        {
            playerAnimator.SetFloat("MoveBlend", ((player.current_speed / 48f) * 0.025f));
        }
        else if (player.current_speed == 0)
        {
            playerAnimator.SetFloat("MoveBlend", 0);
        }

        // inTheAir
        if (player.grounded == false)
        {
            playerAnimator.SetBool("inTheAir", true);
        }
        // Landed!
        else if (player.grounded == true)
        {
            playerAnimator.SetBool("inTheAir", false);
<<<<<<< HEAD
            playerAnimator.SetBool("isAirDashing", false);
=======
>>>>>>> 1f7661afd728c8dd44a0ff53f616fd3bdcf61b36
        }

        // Air Dashing!
        if (player.dashing == true)
        {
<<<<<<< HEAD
            playerAnimator.SetBool("isAirDashing", true);
        } 

        // Dodge
        if (Input.GetButtonDown("Controller_X") && player.grounded == true)
        {
            //playerAnimator.Play("DodgeStart");
=======
            playerAnimator.Play("Start Air Dash");
        } 
        // Dodge
        if (Input.GetButtonDown("Controller_X") && player.grounded == true)
        {
            playerAnimator.Play("DodgeStart");
>>>>>>> 1f7661afd728c8dd44a0ff53f616fd3bdcf61b36
        }

        // Rail Grinding
        if (player.grinding == true)
        {
            playerAnimator.SetBool("isGrinding", true);
        }
        else if (player.grinding == false)
        {
            playerAnimator.SetBool("isGrinding", false);
        }

        if (Input.GetButtonDown("Controller_Y"))
        {
            switch (lightAttackCombo)
            {
                case 0:

                    playerAnimator.Play("Swing1 V1");
                   // playerAnimator.Play("Sword1 V1");
                    lightAttackCombo = 1;
                    break;
                case 1:
                    playerAnimator.Play("Swing2 V1");
                    //playerAnimator.Play("Sword2 V1");
                    lightAttackCombo = 2;
                    break;
                case 2:
                    playerAnimator.Play("Swing3 V1");
                    //playerAnimator.Play("Sword3 V1");
                    lightAttackCombo = 0;
                    break;
            }

        }
        if (Input.GetButtonDown("Controller_B"))
        {
            playerAnimator.Play("Swing4 V1");
        }
    }
}
