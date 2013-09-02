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
	
	
	
	
	// ORIENTATION
	private Vector3 lookDirection;		// Where is it looking?
	private Vector3 lookTargetDirection;	// Where does it want to look?
	
	
	// TARGET MANAGEMENT
	private GameObject target = null;
	private float targetTime = 0;
	private float targetTimeout = 1f;
	
	
	// PATHFINDING
	public Path path;
	private Seeker seeker;
	private AIPath aiPath;
	private CharacterController controller;
	private Vector3 targetPosition;
	private cMap map;
	



	// Use this for initialization
	void Start () {
		
		// Gain knowledge of the world
		GameObject go = GameObject.Find("World");
		if (go != null) map = go.GetComponent<cMap>() as cMap;

		// We'll need to call these pathfinding and movement-related components
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		aiPath = GetComponent<AIPath>();

		// Where we are looking
		lookDirection = transform.forward;
		
		// Where we should look
		lookTargetDirection = lookDirection;
		
		
	}
	
	
	
	
	
	
	// Called whenever the enemy reaches the end of a path
	public void TargetReached () 
	{
		
		Debug.Log("Enemy reached target");
				
	}
	


	/*---------------------------------------
               	   SENSOR INPUT 
	-----------------------------------------*/

	// EYES
	public void Seeing (List<GameObject> visiblePlayers) 
	{
		// Save the last time we a player
		targetTime = Time.time;
		
		// Target the closest one
		target = visiblePlayers[0];
	}
	

	// EARS
	public void Hearing (List<GameObject> hearableThings) 
	{
		
	}


	// SMELL
	public void Smelling (List<Vector3> smellPath)
	{
		
	}
	










	
	public void FixedUpdate () {
		
		
		
		if (Time.time > targetTime + targetTimeout) {
			
			target = null;
			
			// Remove current target from the pathfinder, stopping it in its tracks
			aiPath.ClearQuarry();
			
		} else {
			
			// Update the pathfinder
			if (target != null && aiPath.target == null) aiPath.SetQuarry(target);
			
		}
		
		
		
		
		if (target) {
			
			// Trigger attack scripts
			gameObject.SendMessage("Attack", target, SendMessageOptions.DontRequireReceiver);
			
			// In what direction is the target?
			lookTargetDirection = target.transform.position - transform.position;

			// Rotate head towards desired target direction
			lookDirection = Vector3.RotateTowards(lookDirection, lookTargetDirection, 5f * Time.deltaTime, 1000);

			// Rotate graphics
			//if (lookDirection != Vector3.zero) head.rotation = Quaternion.LookRotation(lookDirection);
		}
		
		
		
	
	}
	
	
	
	
	
	
	
	
	
	// Deal with damage taken
	public void Damage (int amount) 
	{
		
		hitPoints -= amount;
		
		if (hitPoints < 0) {
			// Outright destroy this gameobject if HP < 0
			Destroy(gameObject);
		}
		
	}
	
	
	
	
	
	
	
	
	
}
