using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelScript : MonoBehaviour {


    public Image bleedImage;
    public float FadeRate;
    private float targetAlpha = 2f;

    // Use this for initialization
    void Start () {
        bleedImage = GameObject.Find("BleedPanel").GetComponent<Image>();
        bleedImage.color = Color.red;

    }
	
	// Update is called once per frame
	void Update () {
        //var tempColor = bleedImage.color;
        //tempColor.a++;
        //bleedImage.color = tempColor;

    }
}
