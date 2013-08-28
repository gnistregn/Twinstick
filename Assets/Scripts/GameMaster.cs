using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public int currentLevel = 1;
	

	void Awake () {
		DontDestroyOnLoad(transform.gameObject);	
	}
	
	public void GoUpLevel () {
		if (currentLevel < 100) currentLevel++;
		Application.LoadLevel("Newlevel");
	}

	
}
