using UnityEngine;
using System.Collections;

public class SanityCheck : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		if (GameObject.Find("Game Master") == null) Application.LoadLevel("Start");
	}
	
}
