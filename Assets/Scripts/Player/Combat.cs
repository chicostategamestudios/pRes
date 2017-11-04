//Created by Unknown - Last Modified by Thaddeus Thompson - 10/26/17
//This script controls the combat abilities of the player character.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {

	//Script Calls//
	private PlayerGamepad my_gamepad;
	private NewDynamicCameraBehavior my_camera;
	private Sword_Collision myBlade;
	private BasicAI_Attack enemy_attack;

	//Attack Variables//
	//public int lightAttackCombo;
	public int light_damage = 10;//damage enemy receives//change to private for final version
	public int heavy_damage = 15;//damage enemy receives//change to private for final version
	public float light_attack_distance = 10f; // distance travelled by light attack
	public float light_attack_time = .33f; // time it takes to move distance of light attack
	public float strong_attack_distance = 10f; // distance travelled by strong attack
	public float strong_attack_time = .33f; // time it takes to move distance of strong attack
	//private float distance_length;
	//private int combo_counter;
	private bool is_light_attacking = false; // check to see if player is light attacking
	private bool is_strong_attacking = false; // check to see if player is strong attacking
	private bool is_comboing = false;//used to check if combo is continuing
	private bool attack = false;//used by PlayerAttack to determine attack type
	public bool is_attacking = false;//used to control collision window
	//public int light_attack_number;
	//public int strong_attack_number;
	private int attack_number = 0;//new combo tracker
	private float attack_timer;//combo reset
	//[HideInInspector] public BoxCollider weapon_collider;

	//Dodge Variables//
	[HideInInspector]public bool is_invunerable = false; // check to see if player is invunerable from dodging
	public float dodge_distance = 80f; // distance travelled while dodging
	public float dodge_length = 0.1f;//duration of dodge
	private string dodge_button = "LT";//default controller button
	private float dodge_dir_x;//left and right
	private float dodge_dir_z;//forward and back
	private bool trigger_press;//check is trigger was pressed
	private bool is_dodging;//check if currently dodging
	private bool back_dodge = false;//used with no analog input
	//public Vector3 dodge_dir;
	//private Vector3 dodge_dir_rotated;

	//Counter Variables//
	[HideInInspector]public bool is_countering = false;//player is countering
	private float counter_timer = 0.2f;//delay for player to hit second attack button to activate counter
	private float counter_length = 0.5f;//counter duration
	[HideInInspector]public float counter_recovery = 1f;//recovery before player can act
	public float button_delay = 0.3f;

	//Air Strike Variables//
	public float speed = 75f;//control variable
	private float strike_speed;
	private bool striking = false;
	private Vector3 strike_target;//locked on enemy
	public float strike_time = 0.25f;//duration of strike

	//Animation Variables//
	public Animator swordAnimator;
	public GameObject swordObject;
	public Animator playerAnimator;
	public Animations_Sword my_anime;

	//Misc.//
	private bool control = true; //Gets player direction from input
    //public GameObject attack_prefab;
	//public GameObject target_prefab;
    private GameObject camera_anchor;
    //private Transform player_weapon;
    private RaycastHit hit;//Used to check for collisions
    //private Vector3 target;
    private Vector3 input_joystick_left, input_direction;//Direction for player movement
    private bool something_too_close = false; // an object is too close to move forward
    private float controller_drift = 0.1f;//Controller deadzone
	private float next_attack;// Placeholder value till animations are implemented
	private Vector3 forward;//Direction used for attack and dodge movement
	//private GameObject weapon;
	private GameObject my_collider;//Capsule collider of player gameobject
	[HideInInspector]public bool locked_on;//used when player attacks
	[HideInInspector]public GameObject targeted_enemy;//currently locked on enemy
	//public float test_variable;

    private void Start()
    {
		//counter_timer = Time.time;
        next_attack = 0;
		my_gamepad = GetComponent<PlayerGamepad> ();
        camera_anchor = GameObject.Find("Camera Anchor");
		playerAnimator = GetComponent<Animator>();
		//my_anime =
        //player_weapon = GameObject.Find("PlayerWeapon").transform;
		//weapon = GameObject.Find ("WeaponPivot");
		//weapon_collider = GameObject.Find ("WeaponPlaceHolder").GetComponent<BoxCollider> ();//need to change object name at some point
		swordAnimator = GetComponent<Animator>();
		myBlade = GetComponentInChildren<Sword_Collision> ();
		//lightAttackCombo = 0;

		my_camera = camera_anchor.GetComponent<NewDynamicCameraBehavior>();
		my_collider = GameObject.Find ("Player_Capsule");
		forward = transform.TransformDirection(Vector3.forward);
        something_too_close = false;
		//weapon_collider.enabled = false;
		trigger_press = false;
		is_dodging = false;
    }

    void FixedUpdate () {
		if (control) {
			//TJ// direction player is facing
			//Vector3 forward = transform.TransformDirection(Vector3.forward);

			//Debug.Log(something_too_close);
			input_joystick_left = new Vector3 (Input.GetAxisRaw ("LeftJoystickX"), 0, Input.GetAxisRaw ("LeftJoystickY"));

			input_direction = input_joystick_left.normalized;

			// Fast Attack//////////////////////////////////////////////////////////////////////////////////////////////////////
			if (PlayerGamepad.grounded && !is_light_attacking && PlayerGamepad.enable == true) {
				counter_timer -= Time.deltaTime;
				//Counter///////////
				if (is_countering == false && Input.GetButtonDown ("Controller_B") && counter_timer < button_delay) {
					StartCoroutine ("Counter");
				} else {
					counter_timer = 0.2f;
				//Counter End////////////////

					//!!!!!!!!!!!!!!!!!!!!!!!!!Call out to Animations_Sword
					my_gamepad.current_speed = 0;
					forward = transform.TransformDirection (Vector3.forward);
					//Debug.Log("Light Attack");
					is_attacking = true;
					attack_number++;
					attack_timer = 1f;
					locked_on = my_camera.GetLockOn ();
					// Disable Player movement
					PlayerGamepad.enable = false;
					// Check for Combo
					//if (is_comboing) {
					//	combo_counter++;
					//} else {
					//	combo_counter = 1;
					//}

					// Set Player rotation to Camera Anchor rotation
					//transform.eulerAngles = new Vector3 (transform.eulerAngles.x, camera_anchor.transform.eulerAngles.y, transform.eulerAngles.z);
					//Debug.Log("Rotation = " + transform.rotation);

					// check to see if something is in the way
					// bool Physics.SphereCast(Ray ray, float radius, out RaycastHit hitInfo, float maxDistance))

					if (Physics.Raycast (transform.position, forward, out hit, (light_attack_distance))) {
						if (hit.collider.tag == "Wall" || hit.collider.tag == "Enemy") {
							something_too_close = true;
						}
					}


					// Instantiate Move Target
					//Instantiate(target_prefab, transform.position + (transform.forward * light_attack_distance), transform.rotation); // create target marker
					//target_prefab.GetComponent<DestroyMove>().set_life = light_attack_time;

					//weapon_collider.enabled = true;
					is_light_attacking = true;
					myBlade.curDamage (light_damage);//sets attack damage in Sword_Collision

					// Start Animation Coroutine
					my_anime.StartCoroutine ("LightAttackAnim");//starts animation in Animations_Sword
					StartCoroutine (WaitForFastAttackAnimation ());
				}
			}
			//Fast Attack End//////////////////////////////////////////////////////////////////////////////////////////////

			// Strong Attack////////////////////////////////////////////////////////////////////////////////////////////////////
			if (Input.GetButtonDown ("Controller_B") && PlayerGamepad.grounded && !is_strong_attacking && PlayerGamepad.enable == true) {
				counter_timer -= Time.deltaTime;
				//Counter///////////
				if (is_countering == false && Input.GetButtonDown ("Controller_Y") && counter_timer < button_delay) {
					StartCoroutine ("Counter");
				} else {
					counter_timer = 0.2f;
				//Counter End////////////////////

					my_gamepad.current_speed = 0;
					forward = transform.TransformDirection (Vector3.forward);
					//Debug.Log("Heavy Attack");
					is_attacking = true;
					attack_number++;
					attack_timer = 1f;
					locked_on = my_camera.GetLockOn ();
					// Disable Player movement
					PlayerGamepad.enable = false;
					// Check for Combo
					//if (is_comboing) {
					//	combo_counter++;
					//} else {
					//	combo_counter = 1;
					//}

					// Set Player rotation to Camera Anchor rotation
					//transform.eulerAngles = new Vector3(transform.eulerAngles.x, camera_anchor.transform.eulerAngles.y, transform.eulerAngles.z);
					// Debug.Log("Rotation = " + transform.rotation);

					// check to see if something is in the way
					if (Physics.Raycast (transform.position, forward, out hit, (strong_attack_distance))) {
						if (hit.collider.tag == "Wall" || hit.collider.tag == "Enemy") {
							something_too_close = true;
						}
					}

					// Instantiate Move Target
					//Instantiate(target_prefab, transform.position + (transform.forward * strong_attack_distance), transform.rotation); // create target marker
					//target_prefab.GetComponent<DestroyMove>().set_life = strong_attack_time;

					//weapon_collider.enabled = true;
					is_strong_attacking = true;
					// Start Animation Coroutine
					myBlade.curDamage (heavy_damage);//sets attack damage in Sword_Collision
					StartCoroutine (WaitForStrongAttackAnimation ());
				}
			}
			//Strong Attack End//////////////////////////////////////////////////////////////////////////////////////////////

			// Attack movement///////////////////////////////////////////////////////////////////////////////////////////////////
			//if attacking move toward targeted enemy
			if (is_light_attacking || is_strong_attacking && !something_too_close) {
				forward = transform.TransformDirection (Vector3.forward);

				//locked_on = my_camera.GetTargetedEnemy ();
				if (locked_on) {//rotate to face targeted enemy
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, camera_anchor.transform.eulerAngles.y, transform.eulerAngles.z);
				}

				//GameObject target_move = GameObject.FindGameObjectWithTag("Player Move Target"); // find the location of target_prefab
				// move player x units in direction joystick is pointing if not in front of something
				if (!Physics.Raycast (transform.position, forward, out hit, 1.5f)) {//move if free of collision
					transform.Translate (forward * light_attack_distance * Time.smoothDeltaTime, Space.World);
				}
				//Debug.Log (transform.forward * light_attack_distance);
			}
			//Attack Movement End//////////////////////////////////////////////////////////////////////////////////////////////

			//Air Strike////////////////////////////////////////////////////////////////////////////////////////////////
			if (Input.GetButtonDown ("Controller_B") && !PlayerGamepad.grounded && locked_on && PlayerGamepad.enable == true) {
				targeted_enemy = my_camera.GetTargetedEnemy ();//gets currently locked on enemy from DynamicCamera
				//Debug.Log ("targeted");
				//striking = true;
				StartCoroutine ("AirStrike");
			}
			if (striking && locked_on) {
				my_gamepad.current_speed = 0;
				//target = targeted_enemy.transform.position;
				//strike_speed = speed * Time.deltaTime;
				if (locked_on) {//rotate to face targeted enemy
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, camera_anchor.transform.eulerAngles.y, transform.eulerAngles.z);
				}
				transform.position = Vector3.MoveTowards (transform.position, targeted_enemy.transform.position, strike_speed);
				//Debug.Log ("strike");
			}
			//Air Strike End//////////////////////////////////////////////////////////////////////////////////////////////

			// Dodge///////////////////////////////////////////////////////////////////////////////////////////////////////////
			//used for buttons
			if (Input.GetButtonDown ("Controller_" + dodge_button) && (Input.GetAxis ("LeftJoystickX") > controller_drift || Input.GetAxis ("LeftJoystickX") < -controller_drift || Input.GetAxis ("LeftJoystickY") > controller_drift || Input.GetAxis ("LeftJoystickY") < -controller_drift) && !is_invunerable) {
				//Checks in PlayerGamepad if the player is on the ground
				if (PlayerGamepad.grounded) {//can only dodge while on the ground
					dodge_dir_x = Input.GetAxis ("LeftJoystickX");
					dodge_dir_z = Input.GetAxis ("LeftJoystickY");
					forward = transform.TransformDirection (Vector3.forward);
					//Debug.Log("Dodging");
					//Debug.Log(Input.GetAxis("LeftJoystickY"));
					// if game controller is disabled
					//PlayerGamepad.enable = true;

					// check to see if something is in the way
					if (Physics.Raycast (transform.position, forward, out hit, (dodge_distance))) {
						//Debug.Log (hit);
						if (hit.collider.tag == "Wall" /*|| hit.collider.tag == "Enemy"*/) {
							//Debug.Log (hit.collider.tag);
							something_too_close = true;
						}
					}
					my_gamepad.current_speed = 0;
					//Instantiate (target_prefab, transform.position + (transform.forward * dodge_distance), transform.rotation); // create target marker
					is_invunerable = true; // make player invunerable
					//target_prefab.GetComponent<DestroyMove> ().set_life = .5f;

					//GetComponent<Rigidbody>().AddForce(transform.forward * 500000 * dodge_time * Time.deltaTime, ForceMode.Impulse);
					PlayerGamepad.enable = false;
					StartCoroutine (Invunerable ());
				}
			}

			//checks trigger activation so it acts like a button
			if (Input.GetAxis ("Controller_" + dodge_button) == 0) {
				trigger_press = false;
			}

			//button check for trigger
			if (Input.GetAxis ("Controller_" + dodge_button) == 1 && (Input.GetAxis ("LeftJoystickX") > controller_drift || Input.GetAxis ("LeftJoystickX") < -controller_drift || Input.GetAxis ("LeftJoystickY") > controller_drift || Input.GetAxis ("LeftJoystickY") < -controller_drift) && !is_dodging && !trigger_press) {

				playerAnimator.Play ("DodgeStart");
				//Checks in PlayerGamepad if the player is on the ground
				if (PlayerGamepad.grounded) {
					dodge_dir_x = Input.GetAxis ("LeftJoystickX");
					dodge_dir_z = Input.GetAxis ("LeftJoystickY");
					forward = transform.TransformDirection (Vector3.forward);
					//Debug.Log("Dodging");
					//Debug.Log(Input.GetAxis("LeftJoystickY"));
					// if game controller is disabled
					//PlayerGamepad.enable = true;

					// check to see if something is in the way
					if (Physics.Raycast (transform.position, forward, out hit, (dodge_distance))) {
						//Debug.Log (hit);
						if (hit.collider.tag == "Wall" /*|| hit.collider.tag == "Enemy"*/) {
							//Debug.Log (hit.collider.tag);
							something_too_close = true;
						}
					}
					my_gamepad.current_speed = 0;
					StartCoroutine ("DodgeMovement");
				}
			} else if (Input.GetAxis ("Controller_" + dodge_button) == 1 && !is_dodging && !trigger_press) {
				//if no analog input, dodge backwards
				my_gamepad.current_speed = 0;
				back_dodge = true;
				StartCoroutine ("DodgeMovement");
			}
			//Dodge End/////////////////////////////////////////////////////////////////////////////////////////////////////////

			// Dodge movement///////////////////////////////////////////////////////////////////////////////////////////////////
			//sets axis to 1 or 0 for consistent movement
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
				transform.Translate (forward * dodge_distance * Time.smoothDeltaTime, Space.World);
				//transform.Translate(dodge_dir * dodge_distance * Time.smoothDeltaTime, Space.World);
				//Debug.Log (dodge_dir_rotated);
			}
			//Dodge Movement End//////////////////////////////////////////////////////////////////////////////////////////////

			// Counter (One button)//////////////////////////////////////////////////////////////////////////////////////////////////////////
			if (is_countering == false && Input.GetButtonDown ("Controller_LB") && PlayerGamepad.enable == true) {
				//StartCoroutine ("Counter");
				//	is_countering = true;
				//	Debug.Log ("counter");
				//	StartCoroutine ("Counter");
				//	my_anime.StartCoroutine ("CounterAnim");
			}
			//Counter End//////////////////////////////////////////////////////////////////////////////////////////////

			//resets attack combo for animations
			attack_timer -= Time.deltaTime;
			if (attack_timer <= 0f) {
				//light_attack_number = 0;
				//strong_attack_number = 0;
				attack_number = 0;
				my_anime.attack_combo = 0;
			}
		}
    }

	IEnumerator DodgeMovement ()
	{
		
		Physics.IgnoreLayerCollision( 9, 10, ignore: true);//Allows phasing through enemies

		my_gamepad.SetSmoothedRotation(false);//Makes rotation instant to smooth out dodge
		yield return new WaitForSeconds (.1f);

		PlayerGamepad.enable = false;
		trigger_press = true;
		is_dodging = true;
		//Instantiate (target_prefab, transform.position + (transform.forward * dodge_distance), transform.rotation); // create target marker
		is_invunerable = true; // make player invunerable
		//target_prefab.GetComponent<DestroyMove> ().set_life = .5f;

		//GetComponent<Rigidbody>().AddForce(transform.forward * 500000 * dodge_time * Time.deltaTime, ForceMode.Impulse);
		//PlayerGamepad.enable = false;
		StartCoroutine (Invunerable ());
	}

    IEnumerator WaitForFastAttackAnimation ()
    {
		attack = true;
		//rotate weapon
		//weapon.transform.eulerAngles = new Vector3 (80, weapon.transform.eulerAngles.y, weapon.transform.eulerAngles.z);
        yield return new WaitForSeconds(.3f);
		//weapon.transform.eulerAngles = new Vector3 (0, weapon.transform.eulerAngles.y, weapon.transform.eulerAngles.z);
		//GetComponent<PlayerGamepad>().GamepadAllowed = true;
		is_light_attacking = false;//use to send hit damage to attack prefab
        something_too_close = false;
		is_attacking = false;
		myBlade.swordOff ();//turns off sword collider
		//weapon_collider.enabled = false;
        StartCoroutine(ComboTimerLight());
    }

    IEnumerator WaitForStrongAttackAnimation()
    {
		attack = false;
		//rotate weapon
		//weapon.transform.eulerAngles = new Vector3 (80, weapon.transform.eulerAngles.y, weapon.transform.eulerAngles.z);
        yield return new WaitForSeconds(strong_attack_time);
		//weapon.transform.eulerAngles = new Vector3 (80, weapon.transform.eulerAngles.y, weapon.transform.eulerAngles.z);
		//weapon_collider.enabled = true;
		is_strong_attacking = false;
		yield return new WaitForSeconds (.1f);
		//weapon.transform.eulerAngles = new Vector3 (0, weapon.transform.eulerAngles.y, weapon.transform.eulerAngles.z);
		//weapon_collider.enabled = false;
		//GetComponent<PlayerGamepad>().GamepadAllowed = true;
        something_too_close = false;
		is_attacking = false;
		//weapon_collider.enabled = false;
		myBlade.swordOff ();//turns off sword collider
        StartCoroutine(ComboTimerStrong());
    }
	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Needs to deferintiate between light and strong so there is a delay if changed mid combo
    IEnumerator ComboTimerLight()
    {
		if (attack_number <= 2) {
			is_light_attacking = false;
			is_comboing = true;
			PlayerGamepad.enable = true;
		} else {
			is_light_attacking = false;
			PlayerGamepad.enable = false;
			yield return new WaitForSeconds (.5f);
			is_comboing = false;
			attack_number = 0;
			my_anime.attack_combo = 0;
			PlayerGamepad.enable = true;
		}
    }

	IEnumerator ComboTimerStrong()
	{
		if (attack_number <= 2) {
			is_strong_attacking = false;
			is_comboing = true;
            PlayerGamepad.enable = true;
		} else {
			is_strong_attacking = false;
			PlayerGamepad.enable = false;
			yield return new WaitForSeconds (.5f);
			is_comboing = false;
			attack_number = 0;
			PlayerGamepad.enable = true;
		}
	}

    IEnumerator Invunerable()//Dodge Recovery
	{
        yield return new WaitForSeconds(dodge_length);
        is_invunerable = false;
        something_too_close = false;
		yield return new WaitForSeconds (.25f);
        is_dodging = false;

		Physics.IgnoreLayerCollision( 9, 10, ignore: false);//turn enemy/player physics back on

		PlayerGamepad.enable = true;
		my_gamepad.SetSmoothedRotation(true);
		back_dodge = false;
    }

	IEnumerator Counter()
	{
		//Debug.Log ("countering");
		PlayerGamepad.enable = false;
		is_countering = true;
		my_anime.StartCoroutine ("CounterAnim");//call animation
		yield return new WaitForSeconds(counter_length);
		//is_invunerable = false;
		//something_too_close = false;
		//yield return new WaitForSeconds (.25f);
		//is_dodging = false;

		//Physics.IgnoreLayerCollision( 9, 10, ignore: false);
		is_countering = false;
		yield return new WaitForSeconds (counter_recovery);
		//playerAnimator.SetBool("isGrinding", false);
		PlayerGamepad.enable = true;
		counter_recovery = 1f;//reset recovery time
		//my_gamepad.SetSmoothedRotation(true);
		//back_dodge = false;
	}

	IEnumerator AirStrike(){
		//strike_target = targeted_enemy.transform.position;
		strike_speed = speed * Time.deltaTime;
		striking = true;
		//transform.position = Vector3.MoveTowards (transform.position, target, strike_speed);
		//Debug.Log ("strike");
		my_anime.StartCoroutine ("AirStrikeAnim");// placeholder make seperate anime later
		yield return new WaitForSeconds (strike_time);
		targeted_enemy.GetComponent<BasicAI> ().StartCoroutine ("DamageEnemy", 15);
		striking = false;
		//locked_on = false;//
		//strike_target = new Vector3(0,0,0);
	}

	public bool GetAttackType(){
		return attack;
	}
}
//KEEP THIS FOR FUTURE USE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//this.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None; RAGDOLL