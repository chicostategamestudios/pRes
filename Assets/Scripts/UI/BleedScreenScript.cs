using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BleedScreenScript : MonoBehaviour {



    public bool screenBleed = false;
    public Image blood;
    public PlayerHealth playerB;


    // Use this for initialization
    void Start () {
        blood = GameObject.FindGameObjectWithTag("BleedScreen").GetComponent<Image>();
        blood.color = new Color(0,0,0,0);
        playerB = GameObject.Find("Player").GetComponentInChildren<PlayerHealth>();
    }
	
	// Update is called once per frame
	void Update () {
        if (screenBleed == true) {
            if (playerB.health <= 20) { 
            blood.color = new Color(.8f, 0, 0, .25f);
            }else if (playerB.health <= 15)
            {
                blood.color = new Color(.8f, 0, 0, .3f);
            }else if (playerB.health <= 10)
            {
                blood.color = new Color(.9f, 0, 0, .4f);
			}else  if (playerB.health <= 5)
            {
                blood.color = new Color(1, 0, 0, .5f);
            }
        }
        if (screenBleed == false)
        {
            blood.color = new Color(0, 0, 0, 0);
        }
    }
    void OnGUI()
    {
        
       

            /*for (int x = 0; x <= mCamera.rect.width; x++)
            {
                for (int y = 0; y <= mCamera.rect.height; y++)
                {

                    var bleedTexture = new Texture2D(100, 100);

                    float redFade = 255;
                    Color blood = new Color(redFade, 0, 0); ;
                    bleedTexture.SetPixel(x, y, blood);
                    bleedTexture.Apply();
                    GUI.Box(new Rect(0, 0, mCamera.rect.width, mCamera.rect.height), "lose");
                }
            }
            */
    }
}
