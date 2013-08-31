using UnityEngine;
using System.Collections;

public class Player {

	
	public int hitPoints = 100;		// current HP
	public int maxHitPoints = 100;	// max HP
	public int expPoints = 0;		// current XP
	public int level = 1;			// current level
	public int kills = 0;
	private int nextLevelPoints = 300;	// how many XP required for next level


	public Player () {
		Debug.Log("New player instantiated");
	}


	public int nextLevel {
		get {
			return nextLevelPoints;
		}
	}


	public void TakeDamage (int amount) {
		
		hitPoints -= amount;
		if (hitPoints < 0) {
			Debug.Log("Player is dead!");
		}
		
	}


}
