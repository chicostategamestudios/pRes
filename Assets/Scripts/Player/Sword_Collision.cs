using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Collision : MonoBehaviour
{
    public Collider swordCollision;
    public MeshRenderer swordRenderer;

    public GameObject swordObject;
    public Animator swordAnimator;
<<<<<<< HEAD
	int damage;
=======

>>>>>>> 1f7661afd728c8dd44a0ff53f616fd3bdcf61b36
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
<<<<<<< HEAD
		swordRenderer.enabled = false;
		swordCollision.enabled = false;
=======
        swordRenderer.enabled = false;
>>>>>>> 1f7661afd728c8dd44a0ff53f616fd3bdcf61b36
    }

    void Update()
    {
        //currentBaseState = swordAnimator.GetCurrentAnimatorStateInfo(0);
        //isAttacking = swordAnimator.GetBool("isAttacking");
<<<<<<< HEAD
		//Debug.Log(swordAnimator.GetBool("isAttacking"));
        if(swordAnimator.GetBool("isAttacking") == true && swordRenderer.enabled == false)
        {
            swordRenderer.enabled = true;
			swordCollision.enabled = true;
        }
		else if (swordAnimator.GetBool("isAttacking") == false)// && swordRenderer.enabled == true)
        {
			//Debug.Log ("dwjdjwkdj");
            swordRenderer.enabled = false;
			swordCollision.enabled = false;
=======

        if(swordAnimator.GetBool("isAttacking") == true && swordRenderer.enabled == false)
        {
            swordRenderer.enabled = true;
        }
        else if (swordAnimator.GetBool("isAttacking") == false && swordRenderer.enabled == true)
        {
            swordRenderer.enabled = false;
>>>>>>> 1f7661afd728c8dd44a0ff53f616fd3bdcf61b36
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && swordRenderer.enabled == true)
        {
<<<<<<< HEAD
			//Debug.Log ("triggered");
			other.GetComponent<BasicAI> ().StartCoroutine("DamageEnemy", damage); //.enemy_health -= damage;   // Placeholder variable!
        }
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

=======
            other.GetComponent<BasicAI>().enemy_health -= 10;   // Placeholder variable!
        }
    }
>>>>>>> 1f7661afd728c8dd44a0ff53f616fd3bdcf61b36
}
