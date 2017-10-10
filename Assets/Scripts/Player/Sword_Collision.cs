using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Collision : MonoBehaviour
{
    public Collider swordCollision;
    public MeshRenderer swordRenderer;

    public GameObject swordObject;
    public Animator swordAnimator;

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
    }

    void Update()
    {
        //currentBaseState = swordAnimator.GetCurrentAnimatorStateInfo(0);
        //isAttacking = swordAnimator.GetBool("isAttacking");

        if(swordAnimator.GetBool("isAttacking") == true && swordRenderer.enabled == false)
        {
            swordRenderer.enabled = true;
        }
        else if (swordAnimator.GetBool("isAttacking") == false && swordRenderer.enabled == true)
        {
            swordRenderer.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && swordRenderer.enabled == true)
        {
            other.GetComponent<BasicAI>().enemy_health -= 10;   // Placeholder variable!
        }
    }
}
