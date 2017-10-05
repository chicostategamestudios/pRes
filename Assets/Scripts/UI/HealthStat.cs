using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HealthStat : MonoBehaviour{

    [SerializeField]
    private HealthBar bar;

    [SerializeField]
    private float maxVal;

    [SerializeField]
    private float currentVal;

	public static HealthStat S;

	void Awake()
	{
		S = this;
	}

    public float CurrentVal
    {
        get
        {
			return currentVal;
        }

        set
        { 
            this.currentVal = Mathf.Clamp(value,0,MaxVal);
            bar.Value = currentVal;
        }
    }

    public float MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
            maxVal = value;
            bar.maxValue = value;
        }
    }

    public void Initialize()
    {
        MaxVal = 100;
        CurrentVal = 100;
    }

}
