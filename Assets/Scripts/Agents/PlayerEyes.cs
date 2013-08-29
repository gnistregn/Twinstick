using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEyes : MonoBehaviour {

	public Transform head;

	public float viewDistance = 5; 	// How far can I see?
	public float totalFOV = 180;		// The total Field of View
	private float halfFOV;			// Half of the FOV
	public float enemyAutoDetectProximity = .7f;
	public bool debug = true; // Draw gizmos!

	private int worldDiscoveryRays = 20; // Number of rays that discover the world
	private int worldDiscoveryDistance = 20; // How far do these rays go?
	
	
	// Save if something is interactive in our vicinity
	GameObject closestInteractibleThing;
	float closestInteractibleAngle;
	
	public GameObject closestInteractible {
		get {
			return closestInteractibleThing;
		}
	}
	
	
	// Player specific variables
	private List<GameObject> discoverables;
	private float discoverableRefreshTimeout = 1f;


	void Start () {
	
		halfFOV = totalFOV / 2;

		StartCoroutine("RefreshDiscoverables");
		
	}

	IEnumerator RefreshDiscoverables () 
	{
		discoverables = new List<GameObject>();
		Discoverable[] discoverableComponents = FindObjectsOfType<Discoverable>();
		
		foreach (Discoverable d in discoverableComponents) 
		{
			discoverables.Add(d.gameObject);
		}
		
		yield return new WaitForSeconds(discoverableRefreshTimeout);
		StartCoroutine("RefreshDiscoverables");
		
	}



	
	void FixedUpdate () {
		
		closestInteractibleThing = null;
		closestInteractibleAngle = 999;
		
		// What items can these eyes see?
		if (discoverables != null) {
			
			
			// Iterate through our list of discoverables in the world
			foreach (GameObject thing in discoverables) {

				// If this thing actually exists...
				if (thing != null) {
					
					// So far we haven't seen what we're looking for
					bool thingSeen = false;
					
					// In what direction does the thing lie?
					Vector3 rayDirection = thing.transform.position - head.position;

					// Special stuff for enemies as discoverables
					// Sidestepping character controller raycasting bug...
					if (thing.tag == "Enemy") 
					{	
						rayDirection += new Vector3(0,0.5f,0);
					}



					// Show debug info
					if (debug) {
						Debug.DrawRay(head.position, rayDirection, Color.blue);
						Debug.DrawRay(head.position, head.forward, Color.red);
					}
					
					
					
					// Throw a ray towards the thing
					RaycastHit hit;
					float thingAngle = Vector3.Angle(Vector3.Scale(rayDirection, new Vector3(1,0,1)), Vector3.Scale(head.forward, new Vector3(1,0,1)));
					
					if (thingAngle < halfFOV) {
						Physics.Raycast(head.position, rayDirection, out hit);
						if (hit.transform.gameObject == thing) {
							Debug.DrawRay(head.position, hit.point - head.position, Color.cyan);
							thingSeen = true;
							
							// Is it something we can interact with?
							if (thing.GetComponent<Interactible>() != null && thingAngle < closestInteractibleAngle) {
								closestInteractibleAngle = thingAngle;
								closestInteractibleThing = thing;
							}
							
						} else {
							thingSeen = false;
						}
					} else {
						thingSeen = false;
					}
					
					
					// If the thing we're looking for is tagged as an enemy
					if (thing.tag == "Enemy") 
					{
						// If the enemy is superclose...
						Vector3 enemyPos = new Vector3(thing.transform.position.x,0,thing.transform.position.z);
						Vector3 headPos = new Vector3(head.position.x, 0, head.position.z);
						if (Vector3.Distance(enemyPos, headPos) < enemyAutoDetectProximity)
						{
							// We see it!
							thingSeen = true;
						}
					}
					
					
					
					
					
					// Tell the thing that we're either seeing it or not
					if (thingSeen) thing.SendMessage("Seen", SendMessageOptions.DontRequireReceiver);
					else thing.SendMessage("Unseen", SendMessageOptions.DontRequireReceiver);

				}
				
	
				

			}
			
			
			
			
		
		}
		
					
	
		
		
		// World Discovery
		
		Vector3 worldRay;
		Vector3 worldRayEnd;
		
		
		for (int i = 0; i <= worldDiscoveryRays; i++) 
		{	
			// Eye level world discovery. For walls, door, etc.
			Quaternion dir = Quaternion.AngleAxis(-halfFOV + totalFOV * i / worldDiscoveryRays, Vector3.up);
			worldRay = dir * head.forward;
			worldRay = worldRay.normalized;
			worldRayEnd = head.position + worldRay * worldDiscoveryDistance;
			//if (debug) Debug.DrawRay(head.position, worldRay * worldDiscoveryDistance, new Color(1,1,0,0.55f));
			RaycastHit hit;
			Physics.Raycast(head.position, worldRay, out hit, worldDiscoveryDistance);
			
			if (hit.transform != null) 
			{
				if (hit.transform.gameObject.tag == "Hider") 
				{
					hit.transform.gameObject.SendMessage("Seen", SendMessageOptions.DontRequireReceiver);
					//if (debug) Debug.DrawRay(head.position, hit.point - head.position, new Color(1,1,0,0.5f));
					worldRayEnd = hit.point;
				}			
			}
			
		}
		
		
		
		
		
		
		
		
		if (debug) 
		{
			Quaternion leftRayRotation = Quaternion.AngleAxis( -halfFOV, Vector3.up );
			Quaternion rightRayRotation = Quaternion.AngleAxis( halfFOV, Vector3.up );
			Vector3 leftRayDirection = leftRayRotation * head.forward;
			Vector3 rightRayDirection = rightRayRotation * head.forward;
			Debug.DrawRay(head.position, leftRayDirection, Color.white);
			Debug.DrawRay(head.position, rightRayDirection, Color.white);
		}
		
		
		
		
		
		
		
		
		
		
	}
	
	
	public static T[] FindObjectsOfType<T>()
	{
	   T[] objects = UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[];
	   return objects;
	}
	
	
	
}
