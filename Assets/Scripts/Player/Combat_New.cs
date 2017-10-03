using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_New : MonoBehaviour
{
    Animator playerAnimator;
    private PlayerGamepad my_gamepad;
    public int comboChain;

    // Use this for initialization
    void Start ()
    {
        playerAnimator = GetComponent<Animator>();
        my_gamepad = GetComponent<PlayerGamepad>();
        comboChain = 0;
}

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Controller_Y"))
        {
            comboChain++;
            playerAnimator.SetInteger("attackCombo", comboChain);

            if(comboChain >= 4)
            {
                comboChain = 0;
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
