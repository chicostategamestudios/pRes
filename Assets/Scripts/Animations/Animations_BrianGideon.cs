using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.Animations;

// This script works with the animation controller for the player character Brian Gideon, and his sword animations.
public class Animations_BrianGideon : MonoBehaviour
{
    public Animator playerAnimator;
    string currentStateName;

    public GameObject playerObject;
    public GameObject swordObject;
    public PlayerGamepad player;
    public PlayerHealth currentHP;

    bool isDead;
    int lightAttackCombo;     // Max move speed is 48.
    int heavyAttackCombo;

    // Use this for initialization
    void Start ()
    {
        player = playerObject.GetComponent<PlayerGamepad>();    // We will also need certain variables from the gamepad script.
        swordObject = GetComponent<GameObject>();

        playerAnimator = GetComponent<Animator>();    // Here we can refer to the playerAnimator controller we need to use.

        isDead = false;
        lightAttackCombo = 0;
        heavyAttackCombo = 0;

    }

    // Update is called once per frame
    void Update()
    {
        // Player Death!
        if (currentHP.health <= 0 && playerAnimator.GetBool("isDead") == false)
        {
            playerAnimator.SetBool("isDead", true);
            playerAnimator.Play("Death");
        }
        else if (currentHP.health > 0 && playerAnimator.GetBool("isDead") == true)
        {
            playerAnimator.SetBool("isDead", false);
        }

        if (playerAnimator.GetBool("isDead") == true)
        {
            return; // If the player is dead, don't play any player animations!
        }

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
            playerAnimator.SetBool("isAirDashing", false);
        }

        // Air Dashing!
        if (player.dashing == true)
        {
            playerAnimator.SetBool("isAirDashing", true);
        }
        else if (player.dashing == false)
        {
            playerAnimator.SetBool("isAirDashing", false);
        }


        // Dodge
        if (Input.GetButtonDown("Controller_X") && player.grounded == true)
        {
            //playerAnimator.Play("DodgeStart");
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
            switch (heavyAttackCombo)
            {
                case 0:

                    playerAnimator.Play("HeavySwing1 V1");
                    // playerAnimator.Play("Sword1 V1");
                    heavyAttackCombo = 1;
                    break;
                case 1:
                    playerAnimator.Play("HeavySwing2 V1");
                    //playerAnimator.Play("Sword2 V1");
                    heavyAttackCombo = 2;
                    break;
                case 2:
                    playerAnimator.Play("HeavySwing3 V1");
                    //playerAnimator.Play("Sword3 V1");
                    heavyAttackCombo = 0;
                    break;
            }
        }
    }
}
