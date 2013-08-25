using UnityEngine;
using System.Collections;


/*

	Manages and administrates everyting regarding the player that's not animation, movement, input, viewing etc.

*/

public class PlayerBrain : MonoBehaviour {


	
	public int hitPoints = 100;		// current HP
	public int maxHitPoints = 100;	// max HP
	public int expPoints = 0;		// current XP
	public int level = 0;			// current level
	private int nextLevel = 100;	// how many XP required for next level
	
	
	public UILabel statusLabel;		// element for updating player GUI values
	
	
	public void Start () {
		
		UpdateStatusLabel();
		
	}
	
	
	
	
	// Det här ska inte skötas från spelaren. Snarare ska grafiken sköta sig själv.
	public void UpdateStatusLabel () {
		
		if (statusLabel != null) statusLabel.text = hitPoints + "/" + maxHitPoints + "\n" + expPoints + "/" + nextLevel + "\n" + 99;
		
	}
	
	
	
	
	
	public void TakeDamage (int amount) {
		
		hitPoints -= amount;
		if (hitPoints < 0) {
			Debug.Log("Player is dead!");
		}
		
	}
	
	


}
