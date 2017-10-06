/* Code made by Kyle Partlow | Last edited 9/4/17

	The point of this code is to activate a 3,2,1, GO countdown once the player enters a level.
	This code is also used to indicate when the level is over and the end level screen will pop up
	THIS NEEDS TO BE PUT ON THE OBJECT THAT SIGNIFIES THAT THE LEVEL IS OVER ONCE TOUCHED BY THE PLAYER
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Start_Level_Timer : MonoBehaviour {

	private static Start_Level_Timer _start_level;

	public static Start_Level_Timer Instance {
		get {
            if (_start_level == null)
            {
                if (FindObjectOfType<Start_Level_Timer>() == null)
                {
                    _start_level = FindObjectOfType<Start_Level_Timer>();
                    return _start_level;
                }
                else
                {
                    Debug.Log("we cant find it fam");
                    return null;
                }
            }
            else
            {
                return _start_level;
            }
			
		}
	}

	public GameObject Start_Countdown; 			//Empty object holding all of the countdown objects
	public GameObject One_Countdown;			//image indicating "1" in countdown
	public GameObject Two_Countdown;			//image indicating "2" in countdown
	public GameObject Three_Countdown;			//image indicating "3" in countdown
	public GameObject GO_Countdown;				//image indicating "GO" in countdown

	public GameObject Score_and_Time;			//Empty object holding all of the score and timer objects
	public Text Show_Time; 						//Text to show time

    private GameObject player;
	public float minutes_timer = 0;
	public float seconds_timer = 0;
	public float millisec_timer = 0;
	public bool count_end = false;

    public float startTime;

	//once brought into scene
	void Awake()
	{
	    if (_start_level == null) {
            _start_level = this;
		} else {
			Destroy (gameObject);
		}

		player = GameObject.Find ("Player");
		player.GetComponent<PlayerGamepad> ().enabled = false;
		StartCoroutine("Countdown");
	
	}

	public IEnumerator Countdown()
	{
		yield return new WaitForSeconds (0f);
		Start_Countdown.SetActive (true);	//Makes Start_Countdown visbile
		Two_Countdown.SetActive (false);	//Makes countdown 2 invisible
		Three_Countdown.SetActive (false);	//makes countdown 3 invisible
		GO_Countdown.SetActive (false);		//makes countdown GO invisible

		yield return new WaitForSeconds (1f);
		One_Countdown.SetActive (false);	//Makes countdown 1 invisible
		Two_Countdown.SetActive (true);		//Makes countdown 2 visible

		yield return new WaitForSeconds (1f);
		Two_Countdown.SetActive (false);	//Makes countdown 2 invisible
		Three_Countdown.SetActive (true);	//Makes countdown 3 visible

		yield return new WaitForSeconds (1f);
		Three_Countdown.SetActive (false);	//Makes countdown 3 invisible
		GO_Countdown.SetActive (true);		//Makes countdown GO visible

		yield return new WaitForSeconds (1f);
		Start_Countdown.SetActive (false);	//Makes all of Start_Countdown invisible
		player.GetComponent<PlayerGamepad> ().enabled = true;
        //count_end = true;

		player.GetComponent<PlayerGamepad> ().enabled = true;
		startTime = 4;
        count_end = true;
        yield return Timer();
        

}

    IEnumerator Timer()
    {
        float newTime = 0f;
        while (true)
        {
            newTime = Time.timeSinceLevelLoad - startTime;
			//Debug.Log (Time.timeSinceLevelLoad);
            yield return new WaitForEndOfFrame();
            millisec_timer = (int)((newTime * 100f) % 100);
            seconds_timer = (int)(newTime % 60f);
            minutes_timer = (int)((newTime / 60f) % 60);
        }
    }

	void Update()
	{
		if (count_end == true) {
			DisplayTimer ();
		}
		
	}

	void DisplayTimer()
	{
        //counts minutes and seconds
		Score_and_Time.SetActive (true);	//Makes Score and Timer visible
		

        if (millisec_timer < 10 && seconds_timer < 1 && minutes_timer < 1)
        {
			Show_Time.text = "Time: 00:00:0" + millisec_timer.ToString("g2");
        }
        else if (millisec_timer >= 10 && millisec_timer < 100 && seconds_timer < 1 && minutes_timer < 1)
        {
			Show_Time.text = "Time: 00:00:" + millisec_timer.ToString("g2");
        }


        else if (millisec_timer < 10  && seconds_timer >= 0 && seconds_timer < 10 && minutes_timer < 1)
        {
			Show_Time.text = "Time: 00:0" + seconds_timer.ToString("g2") + ":0" + millisec_timer.ToString("g2");
        }
        else if (millisec_timer >= 10 && millisec_timer < 100 && seconds_timer >= 0 && seconds_timer < 10 && minutes_timer < 1)
        {
			Show_Time.text = "Time: 00:0" + seconds_timer.ToString("g2") + ":" + millisec_timer.ToString("g2");
        }
        else if (millisec_timer < 10 && seconds_timer >= 10 && seconds_timer < 60 && minutes_timer < 1)
        {
			Show_Time.text = "Time: 00:" + seconds_timer.ToString("g2") + ":0" + millisec_timer.ToString("g2");
        }
        else if (millisec_timer >= 10 && millisec_timer < 100 && seconds_timer >= 10 && seconds_timer < 60 && minutes_timer < 1)
        {
			Show_Time.text = "Time: 00:" + seconds_timer.ToString("g2") + ":" + millisec_timer.ToString("g2");
        }


        else if (millisec_timer < 10 && seconds_timer >= 0 && seconds_timer < 10 && minutes_timer >= 1)
        {
			Show_Time.text = "Time: 0" + minutes_timer.ToString("g2") + ":0" + seconds_timer.ToString("g2") + ":0" + millisec_timer.ToString("g2");
        }
        else if (millisec_timer >= 10 && millisec_timer < 100 && seconds_timer >= 0 && seconds_timer < 10 && minutes_timer >= 1)
        {
			Show_Time.text = "Time: 0" + minutes_timer.ToString("g2") + ":0" + seconds_timer.ToString("g2") + ":" + millisec_timer.ToString("g2");
        }
		else if (millisec_timer < 10 && seconds_timer >= 10 && seconds_timer < 60 && minutes_timer >= 1)
        {
			Show_Time.text = "Time: 0" + minutes_timer.ToString("g2") + ":" + seconds_timer.ToString("g2") + ":0" + millisec_timer.ToString("g2");
        }
        else if (millisec_timer >= 10 && millisec_timer < 100 && seconds_timer >= 10 && seconds_timer < 60 && minutes_timer >= 1)
        {
			Show_Time.text = "Time: 0" + minutes_timer.ToString("g2") + ":" + seconds_timer.ToString("g2") + ":" + millisec_timer.ToString("g2");
        }

        else if (millisec_timer < 10 && seconds_timer >= 0 && seconds_timer < 10 && minutes_timer >= 10)
        {
			Show_Time.text = "Time: " + minutes_timer.ToString("g2") + ":0" + seconds_timer.ToString("g2") + ":0" + millisec_timer.ToString("g2");
        }
        else if (millisec_timer >= 10 && millisec_timer < 100 && seconds_timer >= 0 && seconds_timer < 10 && minutes_timer >= 10)
        {
			Show_Time.text = "Time: " + minutes_timer.ToString("g2") + ":0" + seconds_timer.ToString("g2") + ":" + millisec_timer.ToString("g2");
        }
        else if (millisec_timer < 10 && seconds_timer >= 10 && seconds_timer < 60 && minutes_timer >= 10)
        {
			Show_Time.text = "Time: " + minutes_timer.ToString("g2") + ":" + seconds_timer.ToString("g2") + ":0" + millisec_timer.ToString("g2");
        }
        else if (millisec_timer >= 10 && millisec_timer < 100 && seconds_timer >= 10 && seconds_timer < 60 && minutes_timer >= 10)
        {
			Show_Time.text = "Time: " + minutes_timer.ToString("g2") + ":" + seconds_timer.ToString("g2") + ":" + millisec_timer.ToString("g2");
        }


    }
		
}