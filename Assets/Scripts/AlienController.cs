using UnityEngine;
using System.Collections;
using Pathfinding;
using System.Collections.Generic;

public class AlienController : MonoBehaviour {
	
	public Transform head;
	public GameObject graphics;
	
	// PERSONAL
	public int hitPoints = 50;
	
	
	
	// MELEE
	public int meleeDamage = 10;
	public float meleeRange = 1;
	public float meleeInterval = 1;
	private float lastMelee = 0;
	
	
	
	
	public float viewDistance = 5; 	// How far can it see?
	public float totalFOV = 90;		// The total Field of View
	private float halfFOV;			// Half of the FOV
	
	

	private Vector3 lookDirection;		// Where is it looking?
	private Vector3 lookTargetDirection;	// Where does it want to look?
	
	private GameObject[] players;
	private int quarryPlayer;
	//public GameObject quarry;			// Current quarry
	public bool quarryVisible = false;	// Can it see the quarry?
	private float quarryTime = 0; // When did we last see our quarry?
	private float quarryTimeout = 1f; // How many seconds after we can't see our quarry do we stop tracking it?
	
	
	private Vector3 quarryDirection;	// The direction of the quarry
	
	
	
	
	
	
	public Path path;
	private Seeker seeker;
	private AIPath aiPath;
	private CharacterController controller;
	private Vector3 targetPosition;
	

	// Use this for initialization
	void Start () {



		// Assemble list of players
		players = GameObject.FindGameObjectsWithTag("Player");
			
		foreach (GameObject player in players) {
			Debug.Log("Player found: " + player.name);
		}




		halfFOV = totalFOV / 2;

		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		aiPath = GetComponent<AIPath>();

		lookDirection = transform.forward;
		lookTargetDirection = lookDirection;
		
		
	}
	
	
	
	
	
	public void TakeDamage (int amount) {
		Debug.Log("Took damage " + amount);
		hitPoints -= amount;
		if (hitPoints < 0) {
			Destroy(gameObject);
		}
	}
	
	
	
	
	
	public void FixedUpdate () {
		
			
		List<GameObject> visiblePlayers = CanSeePlayers();
		
		
		
		

	
			
		if (quarryVisible && Time.time > quarryTime + quarryTimeout) {
			quarryVisible = false;
			Debug.Log("Lost track of quarry");
			aiPath.ClearQuarry();
		}
		
		
		if (visiblePlayers.Count > 0) {
			

		
			GameObject closestPlayer = visiblePlayers[0];
			float lastDistance = 9999;
			foreach (GameObject player in visiblePlayers) {
				float dist = Vector3.Distance(transform.position, player.transform.position);
				if (dist < lastDistance) {
					lastDistance = dist;
					closestPlayer = player;
				}	
			}

			quarryTime = Time.time;

			if (!quarryVisible) {
				quarryVisible = true;
				Debug.Log("Spotted an enemy");
				aiPath.SetQuarry(closestPlayer);
			}


			

			// Establish direction of closest player
			lookTargetDirection = closestPlayer.transform.position - transform.position;
		
		
			
			// MELEE
			// If player is in range
			if (Vector3.Distance(transform.position, closestPlayer.transform.position) < meleeRange) {
				// If enough time has passed since whooping ass last time
				if (Time.time > lastMelee + meleeInterval) {
					lastMelee = Time.time;
					// Tell player it took damage
					closestPlayer.SendMessage("TakeDamage", meleeDamage);
				}
			}
		
		
		
		
		
		} else {
		
			
			lookTargetDirection = transform.forward;
		
			
		}
		
	
		
		// Rotate head towards quarry direction
		lookDirection = Vector3.RotateTowards(lookDirection, lookTargetDirection, 5f * Time.deltaTime, 1000);
		
		// Rotate graphics
		if (lookDirection != Vector3.zero) head.rotation = Quaternion.LookRotation(lookDirection);
		
		
		
	
		
		
		
		
	
	}
	
	
	
	
	
	
	
	List<GameObject> CanSeePlayers () {
		
		
		List<GameObject> visiblePlayers = new List<GameObject>();
		


		foreach (GameObject player in players) {
			
			if (player != null) {
				

		
		
				RaycastHit hit;
				Vector3 rayDirection = new Vector3(player.transform.position.x, .8f, player.transform.position.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z);
		
				Debug.DrawRay(head.transform.position, rayDirection, Color.yellow);
		
				if (Vector3.Angle(rayDirection, head.transform.forward) < halfFOV) {
					if (Physics.Raycast(head.transform.position, rayDirection, out hit)) {
						if (hit.transform.tag == "Player") {
							quarryDirection = rayDirection;
							visiblePlayers.Add(player);
						} 
					}
				}
			
			}
		
		
		}
		
		return visiblePlayers;
		
		
	}
	
	
	
	
	
	
	
	void OnDrawGizmos () {
		
		// FOV Gizmo
		if (quarryVisible) Gizmos.color = Color.red;
		else Gizmos.color = Color.green;
		
		
		Quaternion leftRayRotation = Quaternion.AngleAxis( -halfFOV, Vector3.up );
		Quaternion rightRayRotation = Quaternion.AngleAxis( halfFOV, Vector3.up );
		Vector3 leftRayDirection = leftRayRotation * head.transform.forward;
		Vector3 rightRayDirection = rightRayRotation * head.transform.forward;
		Gizmos.DrawRay( head.transform.position, leftRayDirection * viewDistance );
		Gizmos.DrawRay( head.transform.position, rightRayDirection * viewDistance);
		//Gizmos.DrawRay( head.transform.position, lookTargetDirection * viewDistance);
		
	//	if (quarryVisible) {
		//	Vector3 rayDirection = quarry.transform.position - head.transform.position;
	//		Gizmos.DrawRay( head.transform.position, quarryDirection);
	//	}
		
	}
	
	
	
	
	
}
