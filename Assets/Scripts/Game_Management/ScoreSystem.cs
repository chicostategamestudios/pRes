using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////
// SCORE SYSTEM //
//////////////////
// These functions help calculate points from time completions and combos, as well as points taken from getting hit or killed.

public class ScoreSystem : MonoBehaviour
{
    // Declare variables here!
    public int score_totalScore;        // This is the total score that the player has throughout a level.
	public float[] completionTime;
	public int score_totalComboScore;   // This shows the score earn from all the combos in the current game.
	public int hitNumber;
	public int deathNumber;


    int comboScore_hits;         // Counts the hits landed during a combo.  This also displays on the game UI.\
    int comboScore_hitsTracker;  // This helps count if we reached increments of 10 hits to increase the combo multiplier.
    public int comboScore_multiplier;   // Keeps track of the combo multiplier.  This displays on the game UI too.
    int comboScore_subtotal;     // This variable is where everything in the combo is added together before multiplied by the multiplier.                         
    public int comboScore_total;        // This is the total points earned by a combo that will then be added to both score_totalScore and totalComboScore.
                                        // It allows us to display the score of the current combo.


    static public ScoreSystem Singleton_ScoreSystem;

    void Awake()
    {
        Singleton_ScoreSystem = this;
    }

    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(transform.gameObject);
        score_totalScore = 0;              
        score_totalComboScore = 0;

        // These batch of variables are all used to help calculate the scores from combos.
        comboScore_hits = 0;            
        comboScore_hitsTracker = 0;
        comboScore_multiplier = 1;
        comboScore_subtotal = 0;    
        comboScore_total = 0;       // This displays on the screen as the player builds their combo.  When the combo ends, this is added to both total combo score and main score.
    }

    // Update is called once per frame
    void Update ()
    {
		if (Input.GetKeyDown ("p")) {
			saveScores ();
		}
	}

	public void getCompletionTime(float[] time)
	{
		completionTime = time;
	}

    /////////////////////
    // TIME COMPLETION //   NOTE: ALl commented out and waiting for function that passes on time completion variable
    /////////////////////
    // This function awards points based on time completion.  It accepts 5 parameters to know which timings reward which points,
    // going from the quickest time (Absolute Perfection, which rewards 100,000 pts) to the slowest (Imperfect).  There is no
    // parameter for 'Imperfect' since any time beyond the 'Apprentice' threshold is pretty much 'Imperfect' which rewards no points.
    //
    // Note: The timing thresholds are all entered in seconds, so milliseconds should be entered as thousandths of a second.
    // Examples:  - 2 minutes and 15 seconds will instead be entered as "135" 
    //            - 55 seconds and 2 milliseconds would be entered as "55.002" 
    //            - 4 minutes, 35 seconds, and 50 milliseconds would be entered as "275.05"
    
    void score_addTimeCompletion(float timing_absolutePerfection, float timing_divine, float timing_godlike, float timing_devoted, float timing_apprentice)
    {
        float completionTime = levelClear();  // THIS IS A FUNCTION PLACEHOLDER!!  Just sayin'.

        if (completionTime <= timing_absolutePerfection)
        {
            score_totalScore += 100000;   // Absolute Perfection!!
        }
        else if (completionTime > timing_absolutePerfection && completionTime <= timing_divine)
        {
            score_totalScore += 75000;    // Divine!!
        }
        else if (completionTime > timing_divine && completionTime <= timing_godlike)
        {
            score_totalScore += 50000;     // Godlike!
        }
        else if (completionTime > timing_godlike && completionTime <= timing_devoted)
        {
            score_totalScore += 25000;     // Devoted!
        }
        else if (completionTime > timing_devoted && completionTime <= timing_apprentice)
        {
            score_totalScore += 10000;     // Apprentice.
        }
        else
        {
            // Imperfect.  So 0 points awarded.
        }
    }
    
    float levelClear()
    {
        return 2;   //This is a placeholder. change later!. compelte din 2 secs.
    }

    /////////////////
    // Combo Score //
    /////////////////
    // These functions help calculate points earned from combos.
    // This formula is also used:
    // Total Combo =  (Fast Attacks + Strong Attacks + Counters + Kills) x Combo Multiplier
    //
    // For anyone using the combo functions, you'll most likely just need the following functions:
    // combo_addFastAttack()
    // combo_addStrongAttack()
    // combo_addCounter()
    // combo_addKill()
    // combo_resetCombo()
    // combo_getHits()
    // combo_getMultiplier()
    // combo_getCurrentComboScore()
    //
    // You'll most likely not need the following since they're used by the other functions here: 
    // combo_addHit()
    // combo_calculateCombo()

    public void combo_addFastAttack()
    {
        comboScore_subtotal += 10;
        combo_addHit();
    }

    public void combo_addStrongAttack()
    {
        comboScore_subtotal += 20;
        combo_addHit();
    }

    public void combo_addCounter()
    {
        comboScore_subtotal += 150;
        combo_addHit();     // Counters has the player character perform a quick swipe at his attacker.
    }

    public void combo_addKill()
    {
        comboScore_subtotal += 200;
        combo_calculateCombo();     // Most likely one of the other attacks already landed, so we can directly call to calculate the combo score
    }                               // and not worry about calling 'combo_addHit()'.

    public void combo_addHit()      // This counts hits that the player landed, and increases the multiplier by 1 (max 5) for every 10 hits.
    {                               // combo_calculateCombo() is also conveniently left at the end.
        comboScore_hits += 1;
        if(comboScore_multiplier < 5)  // This next batch of code keeps track of when to increment the multiplier.  The multiplier is also capped at a max of 5.
        {
            comboScore_hitsTracker += 1;
            if (comboScore_hitsTracker == 10)   // Once hitsTracker reaches 10, it is reset and we increase the combo multiplier by 1.
            {
                comboScore_multiplier += 1;
                comboScore_hitsTracker = 0;
            }
        }          
        combo_calculateCombo();     // In the end, we calculate the score.
    }

    public void combo_calculateCombo()  // This function is constantly called to update the total combo score earned.
    {
        comboScore_total = comboScore_subtotal * comboScore_multiplier;
    }

    public void combo_resetCombo()      // Once a combo is finished or interrupted, the total score earned from the combo 
    {                                   // is added to both Total Combo Score and the main Total Score.  Hits and multipliers are also reset.
        score_totalComboScore += comboScore_total;
        score_totalScore += comboScore_total;

        comboScore_hits = 0;
        comboScore_hitsTracker = 0;
        comboScore_multiplier = 1;
        comboScore_subtotal = 0;
        comboScore_total = 0;
    }

    public int combo_getHits()    // This returns the number of hits landed in a combo. 
    {
        return comboScore_hits;
    }

    public int combo_getMultiplier()    // This returns the multiplier of the current combo. 
    {
        return comboScore_multiplier;
    }

    public int combo_getCurrentComboScore()    // This returns the score of the current combo.
    {
        return comboScore_total;
    }

    /////////////////////
    //  score_hitTaken //
    /////////////////////
    // This function subtracts 150 points from the total score whenever the player takes a hit.
    void score_hitTaken()
    {
        combo_resetCombo();         // This resets the combo since the player already got hit anyways.
        if (score_totalScore < 150)
            score_totalScore = 0;   // Here we make sure the player doens't go into a negative score.
        else
            score_totalScore -= 150;
    }

    ////////////////////////
    //  score_playerDeath //
    ////////////////////////
    // This function subtracts 1000 points from the total score upon player death.
    void score_playerDeath()
    {
        combo_resetCombo();             // Well the player did die... and it's here just in case score_hitTaken() wasn't called.
        if (score_totalScore < 1000)
            score_totalScore = 0;       // Here we make sure the player doens't go into a negative score.
        else
            score_totalScore -= 1000;
    }


    //////////////////////////
    //  score_getFinalScore //
    //////////////////////////
    // Simply returns the final score. Most likely called upon ending a level to compare the score of the current run to the stored highest score.
    int score_getFinalScore()
    {
        return score_totalScore;
    }

	void saveScores(){
		SaveLoad.S.SaveDataOfObject (this);
	}
}
