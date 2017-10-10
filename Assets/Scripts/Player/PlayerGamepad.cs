//Original Author: Alexander Stamatis || Last Edited: Alexander Stamatis | Modified on Oct 5, 2017
//This script deals with player movement, camera, collisions and trigger interactions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//shoot raycast when dashing, not when not dashing
//implement a scalable knockback system
//should dash when not moving
//should boost under max running speed

public class PlayerGamepad : MonoBehaviour
{

    //MOVEMENT
    [Tooltip("How fast the player gets to max speed. Value between 0 and 1.")]
    public float acceleration;
    [Tooltip("How fast the player slows down. Value between 0 and 1.")]
    public float deacceleration;
    public float current_speed, speed_smooth_velocity, current_speed_multiplier;    // Neil: Also made this public for animation controller.
    public float max_running_speed, original_max_speed;
    private bool disable_left_joystick, disable_right_joystick; // left stick is movement, right stick is camera
    Vector3 move_direction;

    //PLAYER
    private float player_direction;
    private float player_rotation_speed;
    private Rigidbody player_rigidbody;
    public bool grounded;
    private bool PlayerDied = false;
    public float running_acceleration_multiplier;

    //RAIL
    public bool grinding;   // Neil: Also made public so my animation script can detect grinding.
    private Vector3 grinding_direction;

    //Wall
    public bool on_wall;
    private Vector3 wall_contact_position;
    private float wall_timer;

    //JUMP
    public int jump_counter, jump_limit;
    [Tooltip("Value between 10 and 50 for jump_force.")]
    public float jump_force;
    private bool can_jump;

    //CAMERA
    private float camera_rotation_speed, turn_smooth_velocity, turn_smooth_time;
    public bool camera_recenter;
    private bool gamepad_allowed;
    private GameObject camera_anchor; //grabbing this from the hierarchy to override camera rotation
    public bool use_camera_type_1;

    //GAMEPAD
    private Vector3 input_joystick_left, input_joystick_right, input_direction, last_direction;
    public bool GamepadAllowed
    {
        get { return gamepad_allowed; }
        set { gamepad_allowed = value; }
    }
    private bool allow_gamepad_camera_movement, allow_gamepad_player_movement;

    private float delta_before, delta_now;
    private float difference_in_degrees;

    //RING 
    private bool ring_transit;
    private Quaternion ring_rotation;
    private Vector3 ring_direction;
    private bool in_ring;
    //Get script from col.gameObject
    RingManager ring_manager_script;
    private bool exiting_ring;
    private float exiting_ring_timer;

    //CHECK LAST FRAME DIRECTION
    private float delayed_player_direction;
    float timer_direction;

    private Ray ray;
    private RaycastHit hit, hit_down;

    //DASH
    [Tooltip("How long will thedashinglast. Recommend values under 5 seconds")]
    public float dash_duration, percentage_of_dash_duration_on_accelerate, percentage_of_dash_duration_on_deaccelerate;
    private float dash_timer;
    public bool dashing;
    private Vector3 last_captured_player_direction;
    private TrailRenderer dash_trail_renderer;
    private int dash_counter;
    [Tooltip("The speed of the dash acceleration")]
    public float dash_acceleration, dash_deacceleration;
    public float grinding_speed;
    public float dash_rotation_speed;

    //BOOSTER
    public float booster_force;
    public float turning_speed;

    //Booster
    private float booster_timer ;
    private bool can_boost, boosting;
    private float booster_meter_max_x;
    public float booster_rotation_speed, booster_speed;
    private GameObject booster_meter_obj;

	public bool smoothed_rotation;

    void Awake()
    {

        if (GameObject.Find("SurgeMeter")) booster_meter_obj = GameObject.Find("SurgeMeter");

        if (Physics.gravity.y > -80f)
            Physics.gravity = new Vector3(0, -100f, 0);

        //Controller initalization
        allow_gamepad_camera_movement = true;
        allow_gamepad_player_movement = true;
        disable_left_joystick = false;

        can_jump = true;

        if (use_camera_type_1 == false)
            use_camera_type_1 = true;

        if (booster_force == 0)
            booster_force = 11f;

        //Get camera
        if (camera_anchor == null)
            camera_anchor = GameObject.Find("Camera Anchor");

        //Get player rigidbody component
        if (player_rigidbody == null)
            player_rigidbody = GetComponent<Rigidbody>();

        //Get trail component
        if (dash_trail_renderer == null)
            dash_trail_renderer = GetComponent<TrailRenderer>();

        if (dash_trail_renderer.enabled == true)
            dash_trail_renderer.enabled = false;

        //Dash duraton
        if (dash_duration == 0)
            dash_duration = .4f;

        //25% of dash duration will be deacceleration
        percentage_of_dash_duration_on_deaccelerate = 15;

        //50% of dash duration will be acceleration
        percentage_of_dash_duration_on_accelerate = 75;

        //how fast should the dash accelerate to max speed (which is declared below)
        dash_acceleration = 400f;

        //slowing down the player from dash speed
        dash_deacceleration = 20f;

        dash_rotation_speed = 1.25f;

        if (dash_trail_renderer == false)
            dash_trail_renderer = GetComponent<TrailRenderer>();

    }

    void Start()
    {

        if (GameObject.Find("SurgeMeter")) booster_meter_max_x = GameObject.Find("SurgeMeter").transform.localScale.x;
        booster_timer = 3f;
        booster_speed = 100f;
        booster_rotation_speed = 1.5f;

        //This will enable player control, for example gamepad_allowed is set to false when the player is in the sonic rings
        gamepad_allowed = true;

        if (acceleration == 0)
            acceleration = 0.5f;

        if (deacceleration == 0)
            deacceleration = 1.5f;

        if (camera_rotation_speed == 0)
            camera_rotation_speed = 200;

        if (turn_smooth_time == 0)
            turn_smooth_time = 0.4f;

        if (max_running_speed == 0)
            max_running_speed = 48f;

        original_max_speed = max_running_speed;

        can_boost = true;

        if (jump_limit == 0)
        {
            jump_counter = 0;
            jump_limit = 1; //change to 2 for double jump
        }

        if (jump_force == 0)
        {
            jump_force = 35;
            jump_force *= 100000f;
        }

		smoothed_rotation = true;
        if (running_acceleration_multiplier == 0)
            running_acceleration_multiplier = .6f;

        if (grinding_speed == 0)
            grinding_speed = 125f;

        if (turning_speed == 0)
            turning_speed = 57.5f;
    }

    void Update()
    {

        if (in_ring)
        {
            //Make the rigidbody of the player y set to zero, in order for the velocity.y to not affect player height transformation
            player_rigidbody.velocity = new Vector3(player_rigidbody.velocity.x, 0, player_rigidbody.velocity.z);


        }


        //---------------------------------------------------------------------------
        //	PAUSE                         
        //---------------------------------------------------------------------------

        //Toggle time
		/*
        if (Input.GetButtonDown("Controller_Start"))
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        }
        */

        if (!gamepad_allowed)
            return;

        if (!disable_left_joystick)
            input_joystick_left = new Vector3(Input.GetAxisRaw("LeftJoystickX"), 0, Input.GetAxisRaw("LeftJoystickY"));

        if (!disable_right_joystick)
            input_joystick_right = new Vector3(Input.GetAxisRaw("RightJoystickY"), Input.GetAxisRaw("RightJoystickX"), 0);

        //---------------------------------------------------------------------------
        //	RING                           
        //---------------------------------------------------------------------------

        if (ring_transit)
        {
            transform.eulerAngles = new Vector3(0, ring_rotation.eulerAngles.y, 0);
            camera_anchor.transform.rotation = Quaternion.Slerp(camera_anchor.transform.rotation, Quaternion.Euler(camera_anchor.transform.eulerAngles.x, ring_rotation.eulerAngles.y, 0), 5 * Time.deltaTime);
        }

    }

    void FixedUpdate()
    {
        //If not true, don't run any of the code below this statement
        if (!gamepad_allowed)
            return;


        //---------------------------------------------------------
        //  AIR
        //---------------------------------------------------------

        //Check to see if the player is grounded
        if(!grinding)
            grounded = Physics.Raycast(transform.position, -transform.up, out hit_down, 2f);

        //Slow down rotation speed of player while on air
        player_rotation_speed = grounded ? 10f : 18f;

        //Sometimes touching the ground doesnt reset the counter, raycast is more reliable
        if (grounded)
        {
            RestrictVerticalMovement(false);
            dash_counter = 0;
            ExitDash();
        }
     

        //---------------------------------------------------------------------------
        //	JUMP                         
        //---------------------------------------------------------------------------

        //Resets the jump counter if grounded and not touching jump button
        if((grounded || on_wall) && !Input.GetButton("Controller_A"))
        {
            jump_counter = 0;
        }

        //Check if you can jump
        if ((Input.GetButton("Controller_A")) && jump_counter < jump_limit && (grounded || on_wall))
        {
            on_wall = false;

            SetPlayerKinematic(false);

            //Limit jump 
            if (player_rigidbody.velocity.y < 40f)
            {
                //Player is forced up using rigidbody physics
                player_rigidbody.AddForce(Vector3.up * jump_force * Time.fixedDeltaTime, ForceMode.Impulse);
                jump_counter++;
            }
        }

        //---------------------------------------------------------
        //  MOVEMENT                                             
        //---------------------------------------------------------


        if (UsingLeftJoystick())
        {

            //Clips input_joystick_left as a normalized to shorten the vectors length to max value of 1
            input_direction = input_joystick_left.normalized;


            //Used by multiple events
            player_direction = GetCurrentDirection();

            //Calculate joystick rotation sensitivity, this will calculate the difference between GetDelayedDirection() and GetCurrentDirection()
            difference_in_degrees = Mathf.Abs(player_direction - delayed_player_direction);

			if (!grinding && smoothed_rotation) {
				//Slowly rotate from the initial rotation to the player rotation, adding camera_anchor.eulerAngles to make it so the axis is based of the camera rotation
				if (dashing) {
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, player_direction + camera_anchor.transform.eulerAngles.y, 0), dash_rotation_speed * Time.deltaTime);
				} else if (boosting) {
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, player_direction + camera_anchor.transform.eulerAngles.y, 0), booster_rotation_speed * Time.deltaTime);
				} else {
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, player_direction + camera_anchor.transform.eulerAngles.y, 0), player_rotation_speed * Time.deltaTime);
				}

			} else {
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, player_direction + camera_anchor.transform.eulerAngles.y, 0), 1);
			}
            current_speed += input_joystick_left.sqrMagnitude * running_acceleration_multiplier;

        } else {
            if (!dashing)
            {
                //This slows down the player, when they let go of the movement joystick
                if (current_speed > 0)
                {
                    current_speed -= deacceleration;
                }
                else if (current_speed < 0)
                {
                    //this just clamps the speed to zero if its less than zero
                    current_speed = 0;
                }
            }
        }

        //The higher the difference of GetCurrentDirection from GetDelayedDirection, the slower the player
        current_speed -= difference_in_degrees / turning_speed;

        //Slow down the player while on air
        current_speed_multiplier = grounded && !dashing ? 33.0f : 48.0f;

        //---------------------------------------------------------
        //SURGE
        //---------------------------------------------------------

        //if (Input.GetButton("Controller_RB") && can_boost && !grinding && !in_ring && current_speed >= original_max_speed && grounded)
		if (Input.GetAxis ("Controller_RT") == 1 && can_boost && !grinding && !in_ring && current_speed >= original_max_speed && grounded)
        {
            if (booster_timer > 0f)
            {
                boosting = true;
                booster_timer -= Time.fixedDeltaTime;
                max_running_speed = booster_speed;
            }
            else if(booster_timer <= 0f)
            {
                max_running_speed = original_max_speed;
                can_boost = false;
                boosting = false;

            }

        }

        if (in_ring || grinding)
        {
            max_running_speed = original_max_speed;
        }


        //if (Input.GetButtonUp("Controller_RB") || !can_boost || !grounded)
		if (Input.GetAxis ("Controller_RT") == 0 || !can_boost || !grounded)
        {
            max_running_speed = original_max_speed;
            boosting = false;

        }


        //if (!Input.GetButton("Controller_RB") || !can_boost)
		if (Input.GetAxis ("Controller_RT") != 1 || !can_boost)
        {

            max_running_speed = original_max_speed;

            if (booster_timer < 3f)
            {
                booster_timer += Time.deltaTime;
            }
            else if(booster_timer >= 3f && !can_boost )
            {
                can_boost = true;
                booster_timer = 3f;
            }
        }
       
	    if (GameObject.Find("SurgeMeter")){
			Vector3 new_booster_meter_scale = GameObject.Find("SurgeMeter").transform.localScale;
			new_booster_meter_scale.x = (booster_timer / 3f) * booster_meter_max_x;
			GameObject.Find("SurgeMeter").transform.localScale = new_booster_meter_scale;
	    }
      

        if (current_speed > max_running_speed && !dashing && !grinding)
        {
            //Speed limit when running
            current_speed = max_running_speed;

        }

        if (grinding)
        {
            move_direction = grinding_direction;
            grounded = true;
        }
        else
            move_direction = transform.forward;

        if (dashing)
        { 
            move_direction = transform.forward;
            SetCurrentSpeed(dash_unary, dash_unary_max_speed);

            if (current_speed > dash_acceleration)
            {
                current_speed = dash_acceleration;
            }
        }

        move_direction *= current_speed * Time.fixedDeltaTime;

        if (!allow_gamepad_player_movement && !dashing)
            move_direction = Vector3.zero;

        //Prevents player from drifting backwards
        //Checking for collision, prevents taleporting through objects when dashing 
            if (DetectCollision(.25f, transform.forward) && dashing)
                if (hit.transform != null && (hit.transform.GetComponent<Collider>().isTrigger != true)) //Essentially imitating a layer mask to ignore certain colliders 
                    move_direction = Vector3.zero;

        //Clamp speed down to zero
        if (current_speed <= 0)
        {
            move_direction = Vector3.zero;
            current_speed = 0;
        }

        //Transform the player (the line that moves the player) 
        transform.Translate(move_direction, Space.World);

        //---------------------------------------------------------------------------
        //  CAMERA        
        //  This is the only thing that moves the camera with the right joystick                
        //---------------------------------------------------------------------------

        //Do the following if the right joystick is moving
        if (use_camera_type_1)
        {
            if (Input.GetAxisRaw("RightJoystickX") != 0 || Input.GetAxisRaw("RightJoystickY") != 0)
            {
                //Get the input from the right joystick and start rotating...
                Vector3 target_rotation = input_joystick_right * camera_rotation_speed * Time.deltaTime;
                //...the camera
                camera_anchor.transform.eulerAngles += target_rotation;
            }
        }

        //-------------------------------------------------
        //	RAIL                           
        //-------------------------------------------------

        if (grinding)
            current_speed = grinding_speed;
        else
            SetPlayerKinematic(false);


        //-------------------------------------------------
        //	WALL                   
        //-------------------------------------------------

        if (on_wall)
        {
            grounded = true;
            transform.position = wall_contact_position;
            player_rigidbody.velocity = Vector3.zero;

            wall_timer += Time.fixedDeltaTime;

            ResetDashValues();

            if(wall_timer > 2f)
            {
                on_wall = false;
                grounded = false;
            }
            
        }

        //-------------------------------------------------
        //	DASH                      
        //keep momentum off dash
        //-------------------------------------------------

        //Activate dash
        if ((Input.GetButtonDown("Controller_X")) && dash_counter < 1 && !grounded && current_speed > 1f && !grinding && !in_ring)
        {
            current_speed = 40f;
            StartCoroutine(Dash(dash_duration, (int)percentage_of_dash_duration_on_accelerate, (int)percentage_of_dash_duration_on_deaccelerate));
        }

        if (DetectCollision(1.5f, transform.forward))
        {
            ExitDash();
            RestrictVerticalMovement(false);
        }
			

        if (exiting_ring)
        {
            exiting_ring_timer += Time.fixedDeltaTime;
            if (exiting_ring_timer < 1f)
            {
                transform.position += ring_direction * 50f * Time.fixedDeltaTime;
            }
            else
            {
                current_speed = 40f;
                exiting_ring_timer = 0f;
                exiting_ring = false;
            }
        }


    } // <- end of FixedUpdate

    private void LateUpdate()
    {
        //Return Delayed Direction, the higher the value of the parameter, the longer the delay
        delayed_player_direction = GetDelayedDirection(0.1f);
    }

    private bool DetectCollision(float _max_distance, Vector3 _direction)
    {
        return Physics.Raycast(transform.position, _direction, out hit, _max_distance);

    }

    //DASH
    //As the title of the function intends, to reset dashing values for reuse of dash
    private void ResetDashValues()
    {
        if (grounded || on_wall)
        {
            dash_counter = 0;
            dashing = false;
            dash_timer = 0;
            dash_trail_renderer.enabled = false;
        }
    }

    int dash_unary, dash_unary_max_speed;

    void SetCurrentSpeed(int unary, float max_speed)
    {
        if (unary == 1)
        {
            if (current_speed < max_speed)
            {
                current_speed += dash_acceleration * Time.deltaTime;
            }
        } else if (unary == -1)
        {
            if (current_speed > max_speed)
            {
                current_speed -= (current_speed * dash_deacceleration) * Time.deltaTime;
            }
        }

    }

    void RestrictVerticalMovement(bool _enable)
    {

        if (_enable)
        {
            player_rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            player_rigidbody.constraints = RigidbodyConstraints.None;
            player_rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

    }

    IEnumerator Dash(float duration, int percent_duration_accelerate, int percent_duraction_deaccelerate)
    {
		SetTrailRender(true);

        //move_direction = transform.forward;

        dashing = true;

        dash_counter += 1;

        //Restrict y-axis transformation
        RestrictVerticalMovement(true);

        //disable_left_joystick = true;

        dash_unary = 1;
        dash_unary_max_speed = (int)dash_acceleration;

        yield return new WaitForSeconds(dash_duration * ((float)percent_duration_accelerate/100f));

        //Unrestrict y-axis transformation
        RestrictVerticalMovement(false);

        dash_unary = -1;
        dash_unary_max_speed = (int)max_running_speed;

        yield return new WaitForSeconds(dash_duration * ((float)percent_duration_accelerate / 100f));

        //move_direction = transform.forward;

        //disable_left_joystick = false;

        SetTrailRender(false);

        dashing = false;

    }

    void ExitDash()
    {

        move_direction = transform.forward;

        disable_left_joystick = false;

        SetTrailRender(false);

        dashing = false;

        StopAllCoroutines();
    }

	//Public function to disable/enable gamepad controller
	public void SetSmoothedRotation(bool _enable)
	{
		smoothed_rotation = _enable;
	}

    //Public function to disable/enable gamepad controller
    public void SetGamepadEnable(bool _enable)
    {
        gamepad_allowed = _enable ? false : true;
    }

    //Check to see if we are using left joystick
    private bool UsingLeftJoystick()
    {
        return input_joystick_left == Vector3.zero ? false : true;
    }

    //Check to see if we are using left joystick
    private bool UsingRightJoystick()
    {
        return input_joystick_right == Vector3.zero ? false : true;
    }

    //Calculate the direction of the joystick by finding the theta angle through arctangent, given the opposite value from the input_joystick_left.x and adjacent value from the input_joystick_left.right
    private float GetCurrentDirection()
    {
        return Mathf.Atan2(input_direction.x, input_direction.z) * Mathf.Rad2Deg;
    }

    //This will get the player direction later than GetCurrentDirection()
    private float GetDelayedDirection(float _delay_rate)
    {
        timer_direction += Time.fixedDeltaTime;
        if (timer_direction > _delay_rate)
        {
            delayed_player_direction = GetCurrentDirection();
            timer_direction = 0f;
            return delayed_player_direction;
        }

        return delayed_player_direction;
    }

    //Enables or disables trail component of the player
    void SetTrailRender(bool _enable)
    {

        dash_trail_renderer.enabled = _enable;
    }

    //Enables or disables rigidbody.gravity
    public void SetPlayerGravity(bool _enable)
    {
        player_rigidbody.useGravity = _enable;
    }

    void SetPlayerKinematic(bool _enable)
    {
        player_rigidbody.isKinematic = _enable;
    }

    //Used to capture last frames of the left joystick position, to compare it with the current one, this will calculate the delta (rate of change) of the left joystick
    void LastFrameLeftJoystick()
    {
        delayed_player_direction = player_direction;
    }

    //This will move the player for a little bit forward after the player has exited the rings or 
    
    IEnumerator MoveFor(float seconds)
    {
        current_speed = 40f;
        current_speed_multiplier = 20f;
        current_speed = Mathf.SmoothDamp(current_speed, current_speed_multiplier * 50, ref speed_smooth_velocity, 0.5f);
        gamepad_allowed = true;

        yield return new WaitForSeconds(0.5f);
        in_ring = false;
    }

    //Disable gravity (velocity.y), since it affects ring movetowards/lerping
    IEnumerator DisableGravityForRing()
    {
        SetPlayerGravity(false);
        yield return new WaitForSeconds(3.0f);
        SetPlayerGravity(true);
    }

    public void SetPlayerMovement(bool _enable)
    {
        disable_left_joystick = !_enable;
        allow_gamepad_player_movement = _enable;
    }

    //Determines player direction when entering a rail
    private string CheckEnteringDirection(float _difference_of_angles)
    {
        return _difference_of_angles > 90f && _difference_of_angles < 270f ? "forward" : "backward";
    }

    //-----------
    // Added by TJ & Tru
    //-----------
    public bool CheckGrounded()
    {
        return grounded;
    }

    //--------------------------------------------------------------------------
    //	COLLIDERS               
    //--------------------------------------------------------------------------

    void OnCollisionEnter(Collision col)
    {


        if (col.gameObject.tag == "wallJump")
        {
			if (!grounded) {
				wall_contact_position = transform.position;

				on_wall = true;
				wall_timer = 0f;
			}
        }

        //Player candashingagain
        ResetDashValues();

        //If collided with death zone, 
        if (col.gameObject.name == "Death Zone")
        {

            PlayerDied = true;
            transform.position = GameObject.Find("Spawn Point").transform.position;

        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "wallJump")
        {
            on_wall = false;
        }

    }

    void OnTriggerEnter(Collider col)
    {

        
        if (col.gameObject.tag == "Rail")
        {
            player_rigidbody.velocity = Vector3.zero;

            //Start the grinding statement in the FixedUpdate()
            grinding = true;

            //Stop the player from falling when on rail
            SetPlayerKinematic(true);

            ResetDashValues();

            jump_counter = 0;

            grounded = true;

            //Will determine what direction the player will go towards
            if (Mathf.Abs(col.transform.eulerAngles.y - transform.eulerAngles.y) < 90f || Mathf.Abs(col.transform.eulerAngles.y - transform.eulerAngles.y) > 270f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, col.transform.eulerAngles.y, 0));
                grinding_direction = col.transform.forward;
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, col.transform.eulerAngles.y + 180f, 0));
                grinding_direction = -col.transform.forward;
            }

        }

        if (col.gameObject.tag == "Launch Ring")
        {
            in_ring = true;
            //Used to calculate direction
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            //Used to compare directions
            Vector3 to_other = col.transform.up - transform.position;

            ring_manager_script = col.transform.parent.transform.parent.GetComponent<RingManager>();
            ring_manager_script.engage_transit = true;
            //get col.gameObject name
            string col_name = col.transform.parent.name;
            //get last character of col.gameObject.name, its a number
            //the rings are automatically named in RingManager.cs
            string last_character = col_name.Substring(col_name.Length - 1);
            ring_manager_script.counter = Convert.ToInt32(last_character);

            if (ring_manager_script.counter == 0)
                allow_gamepad_player_movement = false;

            if (ring_manager_script.counter < ring_manager_script.child_count - 1)
            {
                ring_manager_script.counter++;
                ring_manager_script.engage_transit = true;
            }
            else
            {
                ring_manager_script.counter++;
                ring_manager_script.engage_transit = false;
            }

            //this is executed when the player reaches the final ring, to push the player our a bit
            if (ring_manager_script.counter == ring_manager_script.child_count)
            {
                //Enables player movement with left joystick
                allow_gamepad_player_movement = true;
                //Aligns the player's rotation with the ring
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, col.transform.eulerAngles.z, transform.rotation.eulerAngles.z), 1.0f);
                ring_direction = col.transform.up;
                input_direction = ring_direction;
                StartCoroutine(MoveFor(4.0f));
                StartCoroutine(DisableGravityForRing());
                exiting_ring = true;
                SetPlayerGravity(true);
                in_ring = false;
            }
        }

        if (col.gameObject.tag == "Launch Pad")
        {
            //simple addforce jumper for launch pad
            GetComponent<Rigidbody>().AddForce(col.gameObject.transform.forward * 500000 * booster_force * Time.deltaTime, ForceMode.Impulse);
            //to NOT allow player to jump after hitting launch pad
            jump_counter++;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "wallJump")
        {
            RestrictVerticalMovement(true);
        }

        if (col.gameObject.tag == "Rail")
            grinding = false;
    }

	public bool isPlayerDead(){
		return PlayerDied;
	}

	public void setPlayerDeath(bool d){
		PlayerDied = d;
	}
}