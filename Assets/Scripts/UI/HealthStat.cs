using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HealthStat{

    [SerializeField]
    private HealthBar bar;

    [SerializeField]
    private float maxVal;

    [SerializeField]
    private float currentVal;

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
        MaxVal = maxVal;
		CurrentVal = currentVal;
    }

}
