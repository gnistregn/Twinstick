using UnityEngine;
using System.Collections;

public class LoadNewLevel : MonoBehaviour {
	
	public float waitForSeconds = 2f;

	// Use this for initialization
	void Start () {
		StartCoroutine("LoadGame");
	}
	
	IEnumerator LoadGame () {
		yield return new WaitForSeconds(waitForSeconds);
		Application.LoadLevel("Game");
	}
}
