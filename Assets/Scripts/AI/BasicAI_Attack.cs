//This script was written by James | Last edited by James | Modified on September 5, 2017
//The purpose of this script is to create a hit box that swings either left or right.
//The BasicAI script will call this script whenever the AI tries to attack.

/*How it works is that the script will be called to checked to attack in a direction. When it is checked to attack in one direction, it will turn the check off and start attacking from that direction
 *in the attacking state, it will instantiate a weapon. The weapon itself doesn't move, but it is parented to this weapon spawn game object. So the game object is rotated and makes the 
 * weapon rotate. After the duration of the slash, the game object stops rotating and resets. The weapon prefab's script will destroy itself with a timer.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI_Attack : MonoBehaviour {

	public float attackSpeed; //Tru added this to control how fast the attack moves/rotates.

    private Vector3 left_spawn_pos; //the position in which the object is instantiated when attacking from the left.

    private Vector3 right_spawn_pos;  //the position in which the object is instantiated when attacking from the right.

    private Vector3 top_spawn_pos;  //the position in which the object is instantiated when attacking from above.


    public float duration = 0.9f; //how long the attack animation takes. used to keep track for how long to rotate.

    public float wait_timer = 0.5f; //how long the attack waits for before starting to swing.

    public float current_wait_timer = 0f; //the current duration of the wait buffer.

    public float current_timer = 0f; //used to keep track of how long has passed since the start of the attack.
    [HideInInspector]
    public bool check_attack_left = false; //when set to true, it will start attacking from the ai's left arm.
    
    private bool attacking_left = false; //the bool to keep track if the ai is still in the attacking state.
    [HideInInspector]
    public bool check_attack_right = false; //when set to true, it will start attacking from the ai's right arm.

    private bool attacking_right = false;//the bool to keep track if the ai is still in the attacking state.
    [HideInInspector]
    public bool check_attack_top = false; //when set to true, it will start attacking from above.

    private bool attacking_top = false; //the bool to keep track if the ai is still in the attacking state.

    private bool done_attacking = false; //this is used to reset the rotation of the game object after the ai is done attacking.
    [HideInInspector]
    public bool staggered = false; //if the player hits the AI in the middle of the AI's attack, this will be set to true.

    [HideInInspector]
    public GameObject sword; //the weapon that is instantiated, used when the ai gets hit to destroy the weapon.

    public GameObject weapon_left_prefab; // the prefab for the hit box on left arm.

    public GameObject weapon_right_prefab; //the prefab for the hit box on right arm.

    public GameObject weapon_top_prefab; //the prefab for the hitbox when attacking diagonally.

    public GameObject brsrk_left_prefab, brsrk_right_prefab, brsrk_top_prefab;

    private GameObject left_arm_pos; //this is to keep the position of where to spawn the prefab.

    private GameObject right_arm_pos; // same as above

    private GameObject top_arm_pos; // same as above

    private Quaternion original_rotation; //this is used to preserve the old rotations after the weapon is done swinging/rotating.
    [HideInInspector]
    public bool berserk_mode = false;

    // Use this for initialization
    void Start()
    {
        GameObject body_go = this.transform.parent.gameObject; //this will find this object's parent to find the left, right, and top position to spawn the prefabs.
        left_arm_pos = body_go.transform.FindChild("Left Arm Position").gameObject;
        right_arm_pos = body_go.transform.FindChild("Right Arm Position").gameObject;
        top_arm_pos = body_go.transform.FindChild("Top Right Arm Position").gameObject;

    }

    void DoneAttacking() //when this ai is finished with its attack, then reset the rotation.
    {
        transform.rotation = original_rotation;
        done_attacking = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        left_spawn_pos = left_arm_pos.transform.position; //always keep the spawn positions updated to where the game object is.
        right_spawn_pos = right_arm_pos.transform.position;
        top_spawn_pos = top_arm_pos.transform.position;

        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0); //this will lock the x and z rotations to prevent weird rotating.

        if (check_attack_left && sword == null) //when this is set to true, instantiate the prefab and set it parented to this game object. sets this back to false immediately after to prevent non-stop spawning.
        {//this will be checked to attack from the BasicAI script.
            if(berserk_mode)
            {
                sword = Instantiate(brsrk_left_prefab, left_spawn_pos, transform.rotation);
            }
            else
            {
                sword = Instantiate(weapon_left_prefab, left_spawn_pos, transform.rotation);
            }
            sword.transform.parent = gameObject.transform;
            attacking_left = true;
            current_wait_timer = wait_timer;
            original_rotation = transform.rotation;
            check_attack_left = false;

        }

        if(attacking_left) //rotates the instantiated object to the right. this is how the weapon hitbox moves.
        {
            current_wait_timer -= Time.deltaTime;
            current_timer += Time.deltaTime;
            if (current_wait_timer <= 0)
            {
                transform.Rotate(Vector3.up, attackSpeed * Time.deltaTime);
            }

            
            if (staggered)
            {
                Destroy(sword);
                sword = null;
            }

            if (current_timer > duration)
            {
                current_timer = 0;
                done_attacking = true;
                sword = null;
                attacking_left = false;
            }
            
        }

        if (check_attack_right && sword == null) //when this is set to true, instantiate the prefab and set it parented to this game object. afterwards, 
                                //it will set this back to false immediately after to prevent non-stop spawning.
        {
            if (berserk_mode)
            {
                sword = Instantiate(brsrk_right_prefab, right_spawn_pos, transform.rotation);
            }
            else
            {
                sword = Instantiate(weapon_right_prefab, right_spawn_pos, transform.rotation);
            }
            sword.transform.parent = gameObject.transform;
            attacking_right = true;
            current_wait_timer = wait_timer;
            original_rotation = transform.rotation;
            check_attack_right = false;
        }

        if (attacking_right) //rotates the instantiated object to the left. this is how the weapon hitbox moves.
        {
            current_wait_timer -= Time.deltaTime;
            current_timer += Time.deltaTime;

            if (current_wait_timer <= 0)
            {
                transform.Rotate(Vector3.down, attackSpeed * Time.deltaTime);
            }

            if (staggered)
            {
                Destroy(sword);
                sword = null;
            }

            if (current_timer > duration)
            {
                current_timer = 0;
                attacking_right = false;
                done_attacking = true;
                sword = null;
            }
            
        }

        if (check_attack_top && sword == null) //when this is set to true, instantiate the prefab and set it parented to this game object. sets this back to false immediately after to prevent non-stop spawning.
        {
            if (berserk_mode)
            {
                sword = Instantiate(brsrk_top_prefab, top_spawn_pos, transform.rotation);
            }
            else
            {
                sword = Instantiate(weapon_top_prefab, top_spawn_pos, transform.rotation);
            }
            sword.transform.parent = gameObject.transform;
            attacking_top = true;
            current_wait_timer = wait_timer;
            original_rotation = transform.rotation;
            check_attack_top = false;
        }

        if (attacking_top) //rotates the instantiated object to the left. the game object moves downwards on its own from the script.
        {
            current_wait_timer -= Time.deltaTime;
            current_timer += Time.deltaTime;

            if (current_wait_timer <= 0)
            {
                transform.Rotate(Vector3.down, attackSpeed * Time.deltaTime);
            }

            if (staggered)
            {
                Destroy(sword);
                sword = null;
            }

            if (current_timer >= duration)
            {
                current_timer = 0;
                attacking_top = false;
                done_attacking = true;
                sword = null;
            }
            
        }


        if(done_attacking) //this is used to reset the rotation of the spawner after it is done attacking.
        {
            Invoke("DoneAttacking", .1f);
        }


        
    }
}
