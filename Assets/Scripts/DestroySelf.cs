using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour {

	public float delay = 0.0f;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, delay);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
