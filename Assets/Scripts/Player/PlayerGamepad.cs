//Original Author: Alexander Stamatis || Last Edited: Alexander Stamatis | Modified on May 9, 2017
//This script deals with player movement, camera, collisions and trigger interactions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerGamepad : MonoBehaviour
{



    //MOVEMENT
    [Tooltip("How fast the player moves")]
    public float speed;
    [Tooltip("How fast the player gets to max speed. Value between 0 and 1.")]
    public float acceleration;
    [Tooltip("How fast the player slows down. Value between 0 and 1.")]
    public float deacceleration;
    public float current_speed, speed_smooth_velocity, current_speed_multiplier;  // Neil: Making these public to use with animator.
    private bool disable_left_joystick, disable_right_joystick; //left stick is movement, right stick is camera
    Vector3 move_direction;

    //PLAYER
    private float player_direction;
    private float player_rotation_speed;
    private Rigidbody player_rigidbody;
    public bool grounded;  // Neil: I made this public so my animation script can detect if player is grounded. 

    //RAIL
    public bool grinding;   // Neil: Also made public so my animation script can detect grinding.
    private Vector3 grinding_direction;

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

    //CHECKPOINT SYSTEM
    public Transform[] checkpoints;
    private int last_checkpoint_used;

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
    public float dash_duration;
    private float dash_timer;
    [Tooltip("The speed of the dash. Enter value between 10 - 150")]
    public float dash_speed;
    public bool dashing;
    private float last_captured_y_pos; //Used to cancel out y movement
    private Vector3 last_captured_player_direction;//to commit a player to a direction, can't change directions when dashing
    private TrailRenderer dash_trail_renderer;
    private int dash_counter;

    //BOOSTER
    public float booster_force;

    void Awake()
    {

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
            booster_force = 10f;

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

        //Dash
        if (dash_speed == 0)
            dash_speed = 80f;

        if (dash_duration == 0)
            dash_duration = 0.3f;

        if (dash_trail_renderer == false)
            dash_trail_renderer = GetComponent<TrailRenderer>();

        //Create array in heap with 4 memory spaces of transform type
        checkpoints = new Transform[4];

        //For each checkpoint index check if there is something in it, else remind in the console that all of those indexes are not assigned
        for (int i = 1; i < 5; i++)
        {
            if (checkpoints[i - 1] == null)
            {
                checkpoints[i - 1] = GameObject.Find("checkpoint_" + i).transform;
            }
            else
            {
                Debug.LogError("please assign gameobject to " + checkpoints[i - 1]);
            }
        }
    }

    void Start()
    {

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

        if (speed == 0)
            speed = 0.5f;

        //Change this to control player desired speed
        if (current_speed_multiplier == 0)
            current_speed_multiplier = 48;

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

    }

    void Update()
    {

        if (in_ring)
        {
            SetPlayerGravity(false);
            SetTrailRender(false);
            //Make the rigidbody of the player y set to zero, in order for the velocity.y to not affect player height transformation
            player_rigidbody.velocity = new Vector3(player_rigidbody.velocity.x, 0, player_rigidbody.velocity.z);
        }

        //---------------------------------------------------------------------------
        //	PAUSE                         
        //---------------------------------------------------------------------------

        //Toggle time
        if (Input.GetButtonDown("Controller_Start"))
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        }

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
        grounded = Physics.Raycast(transform.position, -transform.up, out hit_down, 1.5f);

        //Slow down rotation speed of player while on air
        player_rotation_speed = grounded ? 10f : 18f;

        //Sometimes touching the ground doesnt reset the counter, raycast is more reliable
        if (grounded)
            dash_counter = 0;

        //---------------------------------------------------------------------------
        //	JUMP                         
        //---------------------------------------------------------------------------


        //Check if you can jump
        if ((Input.GetButton("Controller_A")) && jump_counter < jump_limit)
        {
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

            if (!grinding )
            {
                //Slowly rotate from the initial rotation to the player rotation, adding camera_anchor.eulerAngles to make it so the axis is based of the camera rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, player_direction + camera_anchor.transform.eulerAngles.y, 0), player_rotation_speed * Time.deltaTime);
            }

            //Getting sensitivity of the left joystic, squaring it will make negatives into positives
            if (current_speed < 40f)
                current_speed += input_joystick_left.sqrMagnitude;
        }
        else
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

        //The higher the difference of GetCurrentDirection from GetDelayedDirection, the slower the player
        current_speed -= difference_in_degrees / 50f;

        //Slow down the player while on air
        current_speed_multiplier = grounded ? 33.0f : 48.0f;

        if (grinding)
        {
            move_direction = grinding_direction;
        }
        else
            move_direction = transform.forward;

        move_direction *= current_speed * Time.deltaTime;

        if (!allow_gamepad_player_movement)
            move_direction = Vector3.zero;

        //Speed limit when running
        if (current_speed > 48f && !dashing && !grinding)
            current_speed = 48f;

        //Speed limit when dashing
        if (current_speed > 300f && dashing)
            current_speed = 300f;


        //Prevents player from drifting backwards
        //Checking for collision, prevents taleporting through objects when dashing 
        if (DetectCollision(2f, transform.forward))
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
            current_speed = 200f;
        else
            SetPlayerKinematic(false);

        //-------------------------------------------------
        //	DASH                      
        //keep momentum off dash
        //-------------------------------------------------

        //Activate dash
        if ((Input.GetButtonDown("Controller_X")) && dash_counter < 1 && !grounded)
        {
            StartCoroutine(Dash());
        }

        //Slows down the player, to prevent taleporting through objects
        if (current_speed > 48f && dash_counter > 0)
        {

            if (DetectCollision(4f, transform.forward))
            {
                current_speed -= dash_speed * 200f * Time.fixedDeltaTime;
            }

            if (DetectCollision(8f, transform.forward))
            {
                current_speed -= dash_speed * 100f * Time.fixedDeltaTime;
            }

        }

        //print(dash_counter);

        //---------------------------------------------------------------------------
        //	CHECKPOINTS                        
        //---------------------------------------------------------------------------

        //D-PADS
        float d_pad_vertical = Input.GetAxis("DPadVertical");
        float d_pad_horizontal = Input.GetAxis("DPadHorizontal");

        //make camera rotation the same as player rotation
        if (d_pad_horizontal != 0 || d_pad_vertical != 0)
        {
            camera_anchor.transform.rotation = transform.rotation;
        }

        //D-pad right is d_pad_horizontal == 1
        //D-pad left is d_pad_horizontal == -1
        //D-pad up is d_pad_vertical == 1
        //D-pad down is d_pad_vertical == -1

        if (d_pad_horizontal == 1)
        {
            transform.position = checkpoints[2].position;
            transform.rotation = checkpoints[2].rotation;
            last_checkpoint_used = 3;
        }
        else if (d_pad_horizontal == -1)
        {
            transform.position = checkpoints[0].position;
            transform.rotation = checkpoints[0].rotation;
            last_checkpoint_used = 1;
        }

        if (d_pad_vertical == 1)
        {
            transform.position = checkpoints[1].position;
            transform.rotation = checkpoints[1].rotation;
            last_checkpoint_used = 2;

        }
        else if (d_pad_vertical == -1)
        {
            transform.position = checkpoints[3].position;
            transform.rotation = checkpoints[3].rotation;
            last_checkpoint_used = 4;

        }

        //if you press the back controller button, then transform the players position to a gameobject named "Spawn Point"
        if (Input.GetButtonDown("Controller_Back"))
        {
            transform.position = GameObject.Find("Spawn Point").transform.position;
            transform.rotation = GameObject.Find("Spawn Point").transform.rotation;
            last_checkpoint_used = 0;
            //make camera rotation the same as player rotation
            camera_anchor.transform.rotation = transform.rotation;
        }

        //RESTART SCENE - TEMPORARY!
        /*
         if (Input.GetButtonDown("Controller_B"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        */

		if (Input.GetButtonDown("Controller_LB")){
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        if (grounded)
        {
            dash_counter = 0;
            dashing = false;
            dash_timer = 0;
            dash_trail_renderer.enabled = false;
        }
    }

    IEnumerator Dash()
    {

        SetTrailRender(true);

        if (current_speed < 1000f)
            current_speed += dash_speed * 200f * Time.fixedDeltaTime;

        dash_counter += 1;
        dashing = true;

        yield return new WaitForSeconds(.10f);

        SetTrailRender(false);

        current_speed = 40f;

        dashing = false;
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
    void SetPlayerGravity(bool _enable)
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

    //This will move the player for a little bit forward after the player has exited the rings or rails
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
	public bool CheckGrounded(){
		return grounded;
	}

    //--------------------------------------------------------------------------
    //	COLLIDERS               
    //--------------------------------------------------------------------------

    void OnCollisionEnter(Collision col)
    {

        //JUMPING
        jump_counter = 0;

        //Player candashingagain
        ResetDashValues();

        //If collided with death zone, 
        if (col.gameObject.name == "Death Zone")
        {
            if (last_checkpoint_used != 0)
            {
                transform.position = checkpoints[last_checkpoint_used - 1].position;
                transform.rotation = checkpoints[last_checkpoint_used - 1].rotation;
            }
            else
                transform.position = GameObject.Find("Spawn Point").transform.position;
        }
    }

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Rail")
        {
            player_rigidbody.velocity = Vector3.zero;

            jump_counter = 0;

            //Start the grinding statement in the FixedUpdate()
            grinding = true;

            //Stop the player from falling when on rail
            SetPlayerKinematic(true);

            ResetDashValues();

            //Will determine what direction the player will go towards
            if (Mathf.Abs(col.transform.eulerAngles.y - transform.eulerAngles.y) < 90f)
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
        if (col.gameObject.tag == "Rail")
            grinding = false;
    }

}