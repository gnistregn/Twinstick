using UnityEngine;
using System.Collections;


/*

	Manages and administrates everyting regarding the player that's not animation, movement, input, viewing etc.

*/

public class PlayerBrain : MonoBehaviour {


	private Player playerStructure;

	public void SetPlayer (Player p) {
		playerStructure = p;
	}
	
	public Player player {
		get {
			return playerStructure;
		}
	}
	
	public int hitPoints = 100;		// current HP
	public int maxHitPoints = 100;	// max HP
	public int expPoints = 0;		// current XP
	public int level = 1;			// current level
	public int kills = 0;
	private int nextLevelPoints = 300;	// how many XP required for next level
	
	
	
	public int nextLevel {
		get {
			return nextLevelPoints;
		}
	}
	
	
	public void Start () {
		
	}
	
	
	
	
	
	public void Damage (int amount) {
		
		hitPoints -= amount;
		if (hitPoints < 0) {
			Debug.Log("Player is dead!");
		}
		
	}
	
	


}
