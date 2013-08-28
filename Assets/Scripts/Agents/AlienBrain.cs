using UnityEngine;
using System.Collections;
using Pathfinding;
using System.Collections.Generic;

public class AlienBrain : MonoBehaviour {
	
	public Transform head;		// The head transform, so we can rotate it in the direction it's looking
	public GameObject graphics; // The graphics for this alien
	
	// PERSONAL
	public int hitPoints = 50;
	public float walkSpeed = 0.5f;
	public float runSpeed = 1.2f;
	
	
	// MELEE
	public int meleeDamage = 10;
	public float meleeRange = 1;
	public float meleeInterval = 1;
	private float lastMelee = 0;
	
	
	// EYES
	public float viewDistance = 5; 	// How far can it see?
	public float totalFOV = 90;		// The total Field of View
	private float halfFOV;			// Half of the FOV
	
	
	// ORIENTATION
	private Vector3 lookDirection;		// Where is it looking?
	private Vector3 lookTargetDirection;	// Where does it want to look?
	
	
	// QUARRY MANAGEMENT
	private GameObject[] players;	// Array of all the players
	private int quarryPlayer;		// Array offset for which of players[] is our target
	public bool quarryVisible = false;	// Can it see the quarry?
	private float quarryTime = 0; // When did we last see our quarry?
	public float quarryTimeout = 3f; // How many seconds after we can't see our quarry do we stop tracking it?
	private Vector3 quarryDirection;	// The direction of the quarry
	
	
	// AI
	const byte IDLE = 0;
	const byte PATROL = 1;
	const byte CHASE = 2;
	const byte LOOKING = 3;
	
	
	private int state = IDLE;
	private float lookingTimeout = 3f;
	private float lookingTime = 0f;
	private float idleTimeout = 3f;
	private float idleTime = 0;
	
	
	// PATHFINDING
	public Path path;
	private Seeker seeker;
	private AIPath aiPath;
	private CharacterController controller;
	private Vector3 targetPosition;
	private CMap map;
	



	// Use this for initialization
	void Start () {
		
		Messenger.AddListener("NewLevel", SelfDestruct);

		// Gain knowledge of the world
		GameObject go = GameObject.Find("World");
		map = go.GetComponent<CMap>() as CMap;

		// Assemble list of players
		players = GameObject.FindGameObjectsWithTag("Player");
			
		foreach (GameObject player in players) {
//			Debug.Log("Player found: " + player.name);
		}
		



		halfFOV = totalFOV / 2;

		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		aiPath = GetComponent<AIPath>();

		lookDirection = transform.forward;
		lookTargetDirection = lookDirection;
		
		
	}
	
	
	private void SelfDestruct () {
//	//	Debug.Log("Self-destructing " + gameObject.name);
//		Destroy(gameObject);
	}
	
	
	public void TakeDamage (int amount) {
		Debug.Log("Took damage " + amount);
		hitPoints -= amount;
		if (hitPoints < 0) {
			Destroy(gameObject);
		}
	}
	
	
	
	public void TargetReached () {
		Debug.Log("Enemy reached target");
		StateIdle();
	}
	
	
	private void StateIdle () {
		idleTime = Time.time;
		state = IDLE;
		aiPath.SetSpeed(walkSpeed);
	}
	
	private void StatePatrol () {
		state = PATROL;
		aiPath.SetSpeed(walkSpeed);
		Vector3 p = map.GetRandomSquareOfType(1);
		Debug.Log("Test: " + p);
		aiPath.SetTarget(p);
	}
	
	private void StateChase () {
		// If we haven't set quarryVisible to true yet...
		aiPath.SetSpeed(runSpeed);
		Debug.Log("Spotted an enemy");
		state = CHASE;
	}
	
	public void FixedUpdate () {
		
		
		// AI
		if (state == IDLE && Time.time > idleTime + idleTimeout) 
		{
			StatePatrol();
		}
		
		
		
		// List of players currently visible to the alien
		List<GameObject> visiblePlayers = CanSeePlayers();
		
		
		
		// If enough time has passed to have considered lost the quarry
		if (quarryVisible && Time.time > quarryTime + quarryTimeout) 
		{
			quarryVisible = false;
			Debug.Log("Lost track of quarry");
			// Remove current target from the pathfinder
			aiPath.ClearQuarry();
			state = LOOKING;
			lookingTime = Time.time;
		}
		
		
		// If we see any players...
		if (visiblePlayers.Count > 0) {
			
			// Get the closest player
			GameObject closestPlayer = visiblePlayers[0];
			float lastDistance = 9999;
			foreach (GameObject player in visiblePlayers) {
				float dist = Vector3.Distance(transform.position, player.transform.position);
				if (dist < lastDistance) {
					lastDistance = dist;
					closestPlayer = player;
				}	
			}

			// When was the last time we saw our quarry?
			quarryTime = Time.time;
			
			if (!quarryVisible) {
				quarryVisible = true;
				StateChase();
			}
			
			// Update the pathfinder
			aiPath.SetQuarry(closestPlayer);
			

			// Establish direction of closest player 
			lookTargetDirection = closestPlayer.transform.position - transform.position;
		
		
			

			// If player is in range of doing a melee attack...
			if (Vector3.Distance(transform.position, closestPlayer.transform.position) < meleeRange) 
			{
				// If enough time has passed since meleeing last time
				if (Time.time > lastMelee + meleeInterval) 
				{
					// Update melee time
					lastMelee = Time.time;
					// Tell player it took damage
					closestPlayer.SendMessage("TakeDamage", meleeDamage);
				}
			}
		
		
		
		
		// If we don't see any players...
		} else {
		
			// Look straight forward
			lookTargetDirection = transform.forward;
		
			
		}
		
	
		// Rotate head towards desired target direction
		lookDirection = Vector3.RotateTowards(lookDirection, lookTargetDirection, 5f * Time.deltaTime, 1000);
		
		// Rotate graphics
		if (lookDirection != Vector3.zero) head.rotation = Quaternion.LookRotation(lookDirection);
		
		
		
	
		
		
		
		
	
	}
	
	
	
	
	
	
	
	List<GameObject> CanSeePlayers () {
		
		
		List<GameObject> visiblePlayers = new List<GameObject>();
		


		foreach (GameObject player in players) {
			
			if (player != null) {

		
		
				RaycastHit hit;
				Vector3 rayDirection = new Vector3(player.transform.position.x, .5f, player.transform.position.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z);
		
				//Debug.DrawRay(head.transform.position, rayDirection, Color.yellow);
		
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
		
		if (quarryVisible && quarryPlayer != null) {
			Vector3 rayDirection = players[quarryPlayer].transform.position - head.transform.position;
			Gizmos.DrawRay( head.transform.position, quarryDirection);
		}
		
	}
	
	
	
	
	
}
