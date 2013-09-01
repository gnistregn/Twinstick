using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour {

	
	// GENERICS
	public float range = 10f;		// Range
	public float timeout = .5f;		// Attack frequency
	private float attackTime = 0f;	// Last attack time
	
	// SPECIFICS
	public GameObject projectile;
	public Transform barrel;
	
	
	void Start () {}
	
	
	
	public void Attack (GameObject target) {
		
		// Distance to target
		float targetDistance = Vector3.Distance(transform.position, target.transform.position);
		
		// If in range and not timed out...
		if (targetDistance <= range && Time.time > attackTime + timeout) {
			
			attackTime = Time.time;

			// Shoot straight
			Vector3 shootDirection = new Vector3(target.transform.position.x, barrel.position.y, target.transform.position.z) - barrel.position;

			// Create projectile
			Instantiate(projectile, barrel.position, Quaternion.LookRotation(shootDirection));
			
		}
		
	}

}
