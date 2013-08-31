using UnityEngine;
using System.Collections;

public class PopulateUI : MonoBehaviour {

	public GameObject pfbPlayerUI;

	private GameMaster gameMaster;

	// Use this for initialization
	void Start () {
		
		GameObject go = GameObject.Find("Game Master");
		
		if (go != null) 
		{
			gameMaster = go.GetComponent<GameMaster>();
			
			// Create an instance of the player UI for each player in gameMaster
			for (int i = 0; i < gameMaster.playerCount; i++)
			{
				// Instantiate UI
				GameObject ui = Instantiate(pfbPlayerUI, new Vector3(0,0,0), Quaternion.identity) as GameObject;
				// Set as parent of this panel
				ui.transform.parent = transform;
				// Reset scale
				ui.transform.localScale = new Vector3(1,1,1);
				// Place
				ui.transform.localPosition = new Vector3(i * 300,0,0);
				// Name
				ui.name = "Info for Player " + (i + 1);
			}
		}
		
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
