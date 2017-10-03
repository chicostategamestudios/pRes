//Created by Unknown - Last Modified by Thaddeus Thompson - 9/28/17
//This script controls the combat abilities of the player character.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {

    public GameObject attack_prefab;
	public GameObject target_prefab;
    private GameObject camera_anchor;
    private Transform player_weapon;
    private RaycastHit hit;
    private Vector3 target;
    private Vector3 input_joystick_left, input_direction;
	public float light_attack_distance = 10f; // distance travelled by light attack
    public float light_attack_time = .33f; // time it takes to move distance of light attack
    public float strong_attack_distance = 10f; // distance travelled by strong attack
    public float strong_attack_time = .33f; // time it takes to move distance of strong attack
    public float dodge_distance = 50f; // distance travelled while dodging
    public float dodge_time = .75f; // time it take to move distance of dodge
    private float distance_length;
    private int combo_counter;
    private bool is_light_attacking; // check to see if player is light attacking
    public bool is_strong_attacking; // check to see if player is strong attacking
	public bool is_comboing;
    public bool is_invunerable; // check to see if player is invunerable from dodging
    private bool something_too_close; // an object is too close to move forward

    private float controller_drift = 0.1f;

    // Placeholder value till animations are implemented
    private float next_attack;

	//TJ add-ons
	public float dodge_length;
	public string dodge_button = "LT";
	private PlayerGamepad my_gamepad;
	private Vector3 forward;
	public Vector3 dodge_dir;
	private Vector3 dodge_dir_rotated;
	private NewDynamicCameraBehavior my_camera;
	private GameObject weapon;
	private GameObject my_collider;
	private bool attack;//used by PlayerAttack to determine attack type
	 public bool is_attacking = false;//used to control collision window
	public int light_attack_number;
	public int strong_attack_number;
	public float attack_timer;
	[HideInInspector] public BoxCollider weapon_collider;
	private float dodge_dir_x;
	private float dodge_dir_z;
	private bool trigger_press;
	private bool is_dodging;
	private bool targeted_enemy;
	private bool back_dodge = false;

	public float test_variable;

    private void Start()
    {
        next_attack = 0;
		my_gamepad = GetComponent<PlayerGamepad> ();
        camera_anchor = GameObject.Find("Camera Anchor");
        player_weapon = GameObject.Find("PlayerWeapon").transform;
		weapon = GameObject.Find ("WeaponPivot");
		weapon_collider = GameObject.Find ("WeaponPlaceHolder").GetComponent<BoxCollider> ();//need to change object name at some point
		my_camera = camera_anchor.GetComponent<NewDynamicCameraBehavior>();
		my_collider = GameObject.Find ("Player_Capsule");
		forward = transform.TransformDirection(Vector3.forward);
        something_too_close = false;
		weapon_collider.enabled = false;
		trigger_press = false;
		is_dodging = false;
    }

   /* void Update () {
		//TJ// direction player is facing
		Vector3 forward = transform.TransformDirection(Vector3.forward);

        //Debug.Log(something_too_close);
        input_joystick_left = new Vector3(Input.GetAxisRaw("LeftJoystickX"), 0, Input.GetAxisRaw("LeftJoystickY"));

        /* Dodge movement
		if (is_invunerable && !something_too_close) {
            GameObject target_move = GameObject.FindGameObjectWithTag("Player Move Target"); // find the location of target_prefab

            // move player 10 units in direction joystick is pointing
			transform.Translate(Vector3.forward * dodge_time);
			//Debug.Log (transform.forward * dodge_distance);
		}

        // Attack movement
		if (is_light_attacking && !something_too_close) {
			GameObject target_move = GameObject.FindGameObjectWithTag("Player Move Target"); // find the location of target_prefab
			// move player x units in direction joystick is pointing if not in front of something
			if (!Physics.Raycast (transform.position, forward, out hit, 1.5f)) {
				transform.position = Vector3.MoveTowards (transform.position, target_move.transform.position, light_attack_time);
			}
				//Debug.Log (transform.forward * light_attack_distance);
		}


    }*/

    void FixedUpdate () {
		//TJ// direction player is facing
		//Vector3 forward = transform.TransformDirection(Vector3.forward);

		//Debug.Log(something_too_close);
		input_joystick_left = new Vector3(Input.GetAxisRaw("LeftJoystickX"), 0, Input.GetAxisRaw("LeftJoystickY"));

        input_direction = input_joystick_left.normalized;

        // Fast Attack//////////////////////////////////////////////////////////////////////////////////////////////////////
		if (Input.GetButtonDown("Controller_Y") && !is_light_attacking && GetComponent<PlayerGamepad>().GamepadAllowed == true)
        {
			forward = transform.TransformDirection(Vector3.forward);
            Debug.Log("Light Attack");
			is_attacking = true;
			light_attack_number++;
			attack_timer = 1f;
			targeted_enemy = my_camera.GetLockOn ();
			// Disable Player movement
            GetComponent<PlayerGamepad>().GamepadAllowed = false;
            // Check for Combo
            if (is_comboing)
            {
                combo_counter++;
            }
            else
            {
                combo_counter = 1;
            }

			// Set Player rotation to Camera Anchor rotation
			//transform.eulerAngles = new Vector3 (transform.eulerAngles.x, camera_anchor.transform.eulerAngles.y, transform.eulerAngles.z);
			//Debug.Log("Rotation = " + transform.rotation);

            // check to see if something is in the way
            // bool Physics.SphereCast(Ray ray, float radius, out RaycastHit hitInfo, float maxDistance))
            if (Physics.Raycast (transform.position, forward, out hit, (light_attack_distance)))
            {
                if (hit.collider.tag == "Wall" || hit.collider.tag == "Enemy")
                {
                    something_too_close = true;
                }
            }


            // Instantiate Move Target
            //Instantiate(target_prefab, transform.position + (transform.forward * light_attack_distance), transform.rotation); // create target marker
            //target_prefab.GetComponent<DestroyMove>().set_life = light_attack_time;

			weapon_collider.enabled = true;
			is_light_attacking = true;
			// Start Animation Coroutine
            StartCoroutine(WaitForFastAttackAnimation());
        }

        // Strong Attack////////////////////////////////////////////////////////////////////////////////////////////////////
		if (Input.GetButtonDown("Controller_B") && !is_strong_attacking && GetComponent<PlayerGamepad>().GamepadAllowed == true)
        {
			forward = transform.TransformDirection(Vector3.forward);
            Debug.Log("Heavy Attack");
			is_attacking = true;
			strong_attack_number++;
			attack_timer = 1f;
			targeted_enemy = my_camera.GetLockOn ();
            // Disable Player movement
            GetComponent<PlayerGamepad>().GamepadAllowed = false;
            // Check for Combo
            if (is_comboing)
            {
                combo_counter++;
            }
            else
            {
                combo_counter = 1;
            }

            // Set Player rotation to Camera Anchor rotation
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, camera_anchor.transform.eulerAngles.y, transform.eulerAngles.z);
           // Debug.Log("Rotation = " + transform.rotation);

            // check to see if something is in the way
			if (Physics.Raycast(transform.position, forward, out hit, (strong_attack_distance)))
            {
                if (hit.collider.tag == "Wall" || hit.collider.tag == "Enemy")
                {
                    something_too_close = true;
                }
            }

            // Instantiate Move Target
            //Instantiate(target_prefab, transform.position + (transform.forward * strong_attack_distance), transform.rotation); // create target marker
            //target_prefab.GetComponent<DestroyMove>().set_life = strong_attack_time;
           
            is_strong_attacking = true;
            // Start Animation Coroutine
            StartCoroutine(WaitForStrongAttackAnimation());
        }

		// Attack movement///////////////////////////////////////////////////////////////////////////////////////////////////
		if (is_light_attacking || is_strong_attacking && !something_too_close) {
			forward = transform.TransformDirection(Vector3.forward);

			//targeted_enemy = my_camera.GetTargetedEnemy ();
			if (targeted_enemy) {
				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, camera_anchor.transform.eulerAngles.y, transform.eulerAngles.z);
			}

			//GameObject target_move = GameObject.FindGameObjectWithTag("Player Move Target"); // find the location of target_prefab
			// move player x units in direction joystick is pointing if not in front of something
			if (!Physics.Raycast (transform.position, forward, out hit, 1.5f)) {
				transform.Translate(forward * light_attack_distance * Time.smoothDeltaTime, Space.World);
			}
			//Debug.Log (transform.forward * light_attack_distance);
		}
		/*if (is_strong_attacking) {
			//Transform from;
			//Transform to;
			weapon.transform.Rotate (Vector3.right, test_variable * Time.deltaTime); //new Vector3 (test_variable+Time.deltaTime, 0, 0);

		} else {
			weapon.transform.eulerAngles = new Vector3 (0, weapon.transform.eulerAngles.y, weapon.transform.eulerAngles.z); 
		}*/

        // Dodge///////////////////////////////////////////////////////////////////////////////////////////////////////////
		if (Input.GetButtonDown("Controller_"+dodge_button) && (Input.GetAxis("LeftJoystickX") > controller_drift || Input.GetAxis("LeftJoystickX") < -controller_drift || Input.GetAxis("LeftJoystickY") > controller_drift || Input.GetAxis("LeftJoystickY") < -controller_drift) && !is_invunerable)
        {
			//Checks in PlayerGamepad if the player is on the ground
			if (my_gamepad.CheckGrounded ()) {
				dodge_dir_x = Input.GetAxis ("LeftJoystickX");
				dodge_dir_z = Input.GetAxis ("LeftJoystickY");
				forward = transform.TransformDirection (Vector3.forward);
				//Debug.Log("Dodging");
				//Debug.Log(Input.GetAxis("LeftJoystickY"));
				// if game controller is disabled
				//GetComponent<PlayerGamepad> ().GamepadAllowed = true;

				// check to see if something is in the way
				if (Physics.Raycast (transform.position, forward, out hit, (dodge_distance))) {
					//Debug.Log (hit);
					if (hit.collider.tag == "Wall" /*|| hit.collider.tag == "Enemy"*/) {
						//Debug.Log (hit.collider.tag);
						something_too_close = true;
					}
				}

				//Instantiate (target_prefab, transform.position + (transform.forward * dodge_distance), transform.rotation); // create target marker
				is_invunerable = true; // make player invunerable
				//target_prefab.GetComponent<DestroyMove> ().set_life = .5f;

				//GetComponent<Rigidbody>().AddForce(transform.forward * 500000 * dodge_time * Time.deltaTime, ForceMode.Impulse);
				GetComponent<PlayerGamepad> ().GamepadAllowed = false;
				StartCoroutine (Invunerable ());
			}
        }

		if (Input.GetAxis ("Controller_" + dodge_button) == 0) {
			trigger_press = false;
		}

		//button check for trigger
		if (Input.GetAxis ("Controller_" + dodge_button) == 1 && (Input.GetAxis ("LeftJoystickX") > controller_drift || Input.GetAxis ("LeftJoystickX") < -controller_drift || Input.GetAxis ("LeftJoystickY") > controller_drift || Input.GetAxis ("LeftJoystickY") < -controller_drift) && !is_dodging && !trigger_press) {
			//Checks in PlayerGamepad if the player is on the ground
			if (my_gamepad.CheckGrounded ()) {
				dodge_dir_x = Input.GetAxis ("LeftJoystickX");
				dodge_dir_z = Input.GetAxis ("LeftJoystickY");
				forward = transform.TransformDirection (Vector3.forward);
				//Debug.Log("Dodging");
				//Debug.Log(Input.GetAxis("LeftJoystickY"));
				// if game controller is disabled
				//GetComponent<PlayerGamepad> ().GamepadAllowed = true;

				// check to see if something is in the way
				if (Physics.Raycast (transform.position, forward, out hit, (dodge_distance))) {
					//Debug.Log (hit);
					if (hit.collider.tag == "Wall" /*|| hit.collider.tag == "Enemy"*/) {
						//Debug.Log (hit.collider.tag);
						something_too_close = true;
					}
				}
				StartCoroutine ("DodgeMovement");
			}
		} else if (Input.GetAxis ("Controller_" + dodge_button) == 1 && !is_dodging && !trigger_press) {
			back_dodge = true;
			StartCoroutine ("DodgeMovement");
		}

		// Dodge movement///////////////////////////////////////////////////////////////////////////////////////////////////
		if (is_invunerable && !something_too_close) {
			if (dodge_dir_x > 0) {
				dodge_dir_x = 1;
			} else if (dodge_dir_x < 0) {
				dodge_dir_x = -1;
			}
			if (dodge_dir_z > 0) {
				dodge_dir_z = 1;
			} else if (dodge_dir_z < 0) {
				dodge_dir_z = -1;
			}

			if (back_dodge) {
				forward = transform.TransformDirection (Vector3.back);
			} else {
				forward = transform.TransformDirection (Vector3.forward);
			}
			//GameObject target_move = GameObject.FindGameObjectWithTag("Player Move Target"); // find the location of target_prefab
				//dodge_dir = new Vector3(dodge_dir_x,0,dodge_dir_z);
			//dodge_dir_rotated = Quaternion.AngleAxis (my_camera.transform.rotation.y, Vector3.up) * dodge_dir;
				//dodge_dir_rotated = new Vector3(0, Camera.main.transform.rotation.y, 0);
				//dodge_dir = dodge_dir_rotated * dodge_dir;
				//dodge_dir.y = 0;
			// move player ~ 10 units in direction joystick is pointing
			transform.Translate(forward * dodge_distance * Time.smoothDeltaTime, Space.World);
				//transform.Translate(dodge_dir * dodge_distance * Time.smoothDeltaTime, Space.World);
			Debug.Log (dodge_dir_rotated);
		}

        // Counter//////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (Input.GetButtonDown("Controller_Y") && Input.GetButtonDown("Controller_B"))
        {

        }

		attack_timer -= Time.deltaTime;
		if (attack_timer <= 0f) {
			light_attack_number = 0;
			strong_attack_number = 0;
		}
    }

	IEnumerator DodgeMovement ()
	{
		
		Physics.IgnoreLayerCollision( 9, 10, ignore: true);//Allows phasing through enemies

		my_gamepad.SetSmoothedRotation(false);//Makes rotation instant to smooth out dodge
		yield return new WaitForSeconds (.1f);

		GetComponent<PlayerGamepad> ().GamepadAllowed = false;
		trigger_press = true;
		is_dodging = true;
		//Instantiate (target_prefab, transform.position + (transform.forward * dodge_distance), transform.rotation); // create target marker
		is_invunerable = true; // make player invunerable
		//target_prefab.GetComponent<DestroyMove> ().set_life = .5f;

		//GetComponent<Rigidbody>().AddForce(transform.forward * 500000 * dodge_time * Time.deltaTime, ForceMode.Impulse);
		//GetComponent<PlayerGamepad> ().GamepadAllowed = false;
		StartCoroutine (Invunerable ());
	}

    IEnumerator WaitForFastAttackAnimation ()
    {
		attack = true;
		//rotate weapon
		weapon.transform.eulerAngles = new Vector3 (80, weapon.transform.eulerAngles.y, weapon.transform.eulerAngles.z);
        yield return new WaitForSeconds(.3f);
		weapon.transform.eulerAngles = new Vector3 (0, weapon.transform.eulerAngles.y, weapon.transform.eulerAngles.z);
		//GetComponent<PlayerGamepad>().GamepadAllowed = true;
		is_light_attacking = false;//use to send hit damage to attack prefab
        something_too_close = false;
		weapon_collider.enabled = false;
        StartCoroutine(ComboTimerLight());
    }

    IEnumerator WaitForStrongAttackAnimation()
    {
		attack = false;
		//rotate weapon
		//weapon.transform.eulerAngles = new Vector3 (80, weapon.transform.eulerAngles.y, weapon.transform.eulerAngles.z);
        yield return new WaitForSeconds(strong_attack_time);
		weapon.transform.eulerAngles = new Vector3 (80, weapon.transform.eulerAngles.y, weapon.transform.eulerAngles.z);
		weapon_collider.enabled = true;
		is_strong_attacking = false;
		yield return new WaitForSeconds (.1f);
		weapon.transform.eulerAngles = new Vector3 (0, weapon.transform.eulerAngles.y, weapon.transform.eulerAngles.z);
		weapon_collider.enabled = false;
		//GetComponent<PlayerGamepad>().GamepadAllowed = true;
        something_too_close = false;
        StartCoroutine(ComboTimerStrong());
    }
	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Needs to deferintiate between light and strong so there is a delay if changed mid combo
    IEnumerator ComboTimerLight()
    {
		if (light_attack_number <= 4) {
			is_light_attacking = false;
			is_comboing = true;
			GetComponent<PlayerGamepad>().GamepadAllowed = true;
		} else {
			is_light_attacking = false;
			GetComponent<PlayerGamepad> ().GamepadAllowed = false;
			yield return new WaitForSeconds (.5f);
			is_comboing = false;
			light_attack_number = 0;
			GetComponent<PlayerGamepad> ().GamepadAllowed = true;
		}
    }

	IEnumerator ComboTimerStrong()
	{
		if (strong_attack_number <= 4) {
			is_strong_attacking = false;
			is_comboing = true;
			GetComponent<PlayerGamepad>().GamepadAllowed = true;
		} else {
			is_strong_attacking = false;
			GetComponent<PlayerGamepad> ().GamepadAllowed = false;
			yield return new WaitForSeconds (.5f);
			is_comboing = false;
			strong_attack_number = 0;
			GetComponent<PlayerGamepad> ().GamepadAllowed = true;
		}
	}

    IEnumerator Invunerable()//Dodge Recovery
	{
        yield return new WaitForSeconds(dodge_length);
        is_invunerable = false;
        something_too_close = false;
		yield return new WaitForSeconds (.25f);
        is_dodging = false;

		Physics.IgnoreLayerCollision( 9, 10, ignore: false);

		GetComponent<PlayerGamepad> ().GamepadAllowed = true;
		my_gamepad.SetSmoothedRotation(true);
		back_dodge = false;
    }

	public bool GetAttackType(){
		return attack;
	}
}
//KEEP THIS FOR FUTURE USE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//this.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None; RAGDOLL