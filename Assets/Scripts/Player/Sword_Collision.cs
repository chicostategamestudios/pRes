//Created by Neil - Last Modified by Thaddeus Thompson - 10/12/17

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Collision : MonoBehaviour
{
    public Collider swordCollision;
    public MeshRenderer swordRenderer;

    public GameObject swordObject;
    public Animator swordAnimator;
	int damage;

	//Added by TJ
	public Combat my_combat;
	private BasicAI my_ai;
	private bool attack_type;
    //static int attackState = Animator.StringToHash("Base.Combat");
    //public AnimatorControllerParameter animatorInfo;
    //public bool isAttacking;

    // Use this for initialization
    void Start ()
    {
        swordCollision = GetComponent<Collider>();
        swordRenderer = GetComponent<MeshRenderer>();
        swordAnimator = swordObject.GetComponent<Animator>();
        //isAttacking = false;
		swordRenderer.enabled = false;
		swordCollision.enabled = false;
    }

    void Update()
    {
        //currentBaseState = swordAnimator.GetCurrentAnimatorStateInfo(0);
        //isAttacking = swordAnimator.GetBool("isAttacking");
		//Debug.Log(swordAnimator.GetBool("isAttacking"));
        if(my_combat.is_attacking == true && swordRenderer.enabled == false)
        {
            swordRenderer.enabled = true;
			swordCollision.enabled = true;
        }
		else if (my_combat.is_attacking == false)// && swordRenderer.enabled == true)
        {
			//Debug.Log ("dwjdjwkdj");
            swordRenderer.enabled = false;
			swordCollision.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
		Debug.Log ("triggered");
        if(other.tag == "Enemy Hitbox" && swordRenderer.enabled == true)
        {
			
			my_ai = other.GetComponentInParent<BasicAI> (); //.enemy_health -= damage;   // Placeholder variable!
        	
			attack_type = my_combat.GetAttackType();//calls function in Combat to determine light or heavy attack
			if (attack_type) {
				damage = 10;
				my_ai.StartCoroutine ("DamageEnemy", damage);
				my_combat.is_attacking = false;
				//	Debug.Log ("Deal light damage " + damage);
			} else {
				damage = 15;
				my_ai.StartCoroutine ("DamageEnemy", damage);
				my_combat.is_attacking = false;
				//	Debug.Log ("Deal strong damage " + damage);
			}
		}//turn off collider after attacks

		swordOff ();
    }

	public void curDamage(int newDam)
	{
		damage = newDam;
	}

	public void swordOff(){
		swordRenderer.enabled = false;
		swordCollision.enabled = false;
		swordAnimator.SetBool("isAttacking", false);
	}

}
