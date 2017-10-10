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
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy Hitbox" && swordRenderer.enabled == true)
        {
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

}
