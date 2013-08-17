using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

	private Color defaultColor;
	
	// Use this for initialization
	void Start () {
		defaultColor = renderer.material.color;
		renderer.material.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
