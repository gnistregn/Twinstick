using UnityEngine;
using System.Collections;

public class EnemyMelee : MonoBehaviour {

	
	// GENERICS
	public float range = 0.4f;		// Range
	public int damage = 10;			// Damage
	public float timeout = 1f;		// Attack frequency
	private float attackTime = 0f;	// Last attack time
	
	
	
	public void Attack (GameObject target) {

		// Distance to target
		float targetDistance = Vector3.Distance(transform.position, target.transform.position);

		// If in range and not timed out...
		if (targetDistance <= range && Time.time > attackTime + timeout) {
			
			attackTime = Time.time;
			
			target.SendMessage("TakeDamage", damage);
			
		}
		
	}

}
