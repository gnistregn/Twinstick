using UnityEngine;
using System.Collections;

public class UpdateLevelDescription : MonoBehaviour {

	GameMaster gameMaster;

	// Use this for initialization
	void Start () {
		
		GameObject go = GameObject.Find("Game Master");
		
		if (go != null) {
			gameMaster = go.GetComponent<GameMaster>();
			GetComponent<UILabel>().text = "Entering floor " + gameMaster.currentLevel + "\n" + gameMaster.currentCorpName;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
