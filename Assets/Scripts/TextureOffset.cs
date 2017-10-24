using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureOffset : MonoBehaviour {

	public Renderer render;
	public float speedX;
	public float speedY;
	private float curX;
	private float curY;

	// Use this for initialization
	void Awake () {
		render = GetComponent<Renderer> ();
		curX = render.material.mainTextureOffset.x;
		curY = render.material.mainTextureOffset.y;
	}
	
	// Update is called once per frame
	void Update () {
		curX += Time.deltaTime * speedX;
		curY += Time.deltaTime * speedY;
		render.material.SetTextureOffset ("_MainTex", new Vector2 (curX, curY));
	}
}
