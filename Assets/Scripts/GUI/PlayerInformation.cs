using UnityEngine;
using System.Collections;

public class PlayerInformation : MonoBehaviour {

	//public PlayerBrain playerBrain;

	private Player player;
	private UILabel statValues;

	// Use this for initialization
	void Start () 
	{
		statValues = GameObject.Find("Stat Values").GetComponent<UILabel>();
		Refresh();
	}
	
	public void SetPlayer (Player p) {
		player = p;
	}
	
	void FixedUpdate () {
		Refresh();
	}
	
	void Refresh () 
	{
		if (player != null) 
		{
			string output = "";
			output += player.hitPoints + "/" + player.maxHitPoints + "\n"; 	// HP
			output += player.expPoints + "/" + player.nextLevel + "\n";		// XP
			output += "99/99\n";	// AMMO
			output += player.level + "\n";	// LEVEL
			output += player.kills + "\n";
			statValues.text = output;
		}
			
	}
}
