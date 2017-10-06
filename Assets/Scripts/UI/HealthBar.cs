using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public float fillAmount;

    [SerializeField]
    private float lerpspeed;

    [SerializeField]
    private Image content;

    [SerializeField]
    private Color Fullcolor;

    [SerializeField]
    private Color Lowcolor;

    [SerializeField]
    private bool lerpColors;

    public float maxValue { get; set; }

    public float Value
    {
        set
        {
            fillAmount = Map(value, 0, maxValue, 0, 1);
        }
    }

	// Use this for initialization
	void Start () {
        if(lerpColors)
        {
            content.color = Fullcolor;
        }
	}
	
	// Update is called once per frame
	void Update () {
        HandleBar();
	}

    private void HandleBar()
    {

        if (fillAmount != content.fillAmount)
        {
			//Debug.Log ("content.fillAmount " + content.fillAmount);
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, Time.deltaTime * lerpspeed);
        }
        if (lerpColors)
        {
            content.color = Color.Lerp(Lowcolor, Fullcolor, fillAmount);
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

    }
}
