using UnityEngine;
using System.Collections;

public class PlayerInformation : MonoBehaviour {

	public PlayerBrain playerBrain;

	private UILabel statValues;

	// Use this for initialization
	void Start () 
	{
		statValues = GameObject.Find("Stat Values").GetComponent<UILabel>();
		Refresh();
	}
	
	void FixedUpdate () {
		Refresh();
	}
	
	void Refresh () 
	{
		string output = "";
		output += playerBrain.hitPoints + "/" + playerBrain.maxHitPoints + "\n"; 	// HP
		output += playerBrain.expPoints + "/" + playerBrain.nextLevel + "\n";		// XP
		output += "99/99\n";	// AMMO
		output += playerBrain.level + "\n";	// LEVEL
		output += playerBrain.kills + "\n";
		statValues.text = output;	
	}
}
