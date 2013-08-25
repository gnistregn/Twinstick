using UnityEngine;
using System.Collections;

public class Eyes : MonoBehaviour {

	public Transform head;

	public float viewDistance = 5; 	// How far can I see?
	public float totalFOV = 180;		// The total Field of View
	private float halfFOV;			// Half of the FOV
	
	public bool debug = true; // Draw gizmos!

	private int worldDiscoveryRays = 20; // Number of rays that discover the world
	private int worldDiscoveryDistance = 20; // How far do these rays go?
	private GameObject[] worldThings; // Internal list of things in the world


	void Start () {
	
		halfFOV = totalFOV / 2;
		worldThings = GameObject.FindGameObjectsWithTag("Discoverable_Item");
		
	}


	

	
	void FixedUpdate () {
		
		// What items can these eyes see?
		if (worldThings != null) {
			
			foreach (GameObject thing in worldThings) {

				if (thing != null) {
					Vector3 rayDirection = thing.transform.position - head.position;
					
					if (debug) {
						Debug.DrawRay(head.position, rayDirection, Color.blue);
						Debug.DrawRay(head.position, head.forward, Color.red);
					}
					
					RaycastHit hit;
					if (Vector3.Angle(rayDirection, head.forward) < halfFOV) {
						Physics.Raycast(head.position, rayDirection, out hit);
						if (hit.transform.gameObject == thing) {
							Debug.DrawRay(head.position, hit.point - head.position, Color.cyan);
							hit.transform.gameObject.SendMessage("Seen", SendMessageOptions.DontRequireReceiver);
						} else {
							hit.transform.gameObject.SendMessage("Unseen", SendMessageOptions.DontRequireReceiver);
						}
					} else {
						thing.SendMessage("Unseen", SendMessageOptions.DontRequireReceiver);
					}

				}

			}
		
		}
		
		
		// If these eyes are on a player, let's discover world elements too
		if (gameObject.tag == "Player") {
			Vector3 worldRay;
			Vector3 worldRayEnd;
			
			if (debug) {
			
				Quaternion leftRayRotation = Quaternion.AngleAxis( -halfFOV, Vector3.up );
				Quaternion rightRayRotation = Quaternion.AngleAxis( halfFOV, Vector3.up );
				Vector3 leftRayDirection = leftRayRotation * head.forward;
				Vector3 rightRayDirection = rightRayRotation * head.forward;
				Debug.DrawRay(head.position, leftRayDirection, Color.white);
				Debug.DrawRay(head.position, rightRayDirection, Color.white);
			}
			
			
			for (int i = 0; i <= worldDiscoveryRays; i++) {
				
				// Eye level world discovery. For walls, door, etc.
				Quaternion dir = Quaternion.AngleAxis(-halfFOV + totalFOV * i / worldDiscoveryRays, Vector3.up);
				worldRay = dir * head.forward;
				worldRay = worldRay.normalized;
				worldRayEnd = head.position + worldRay * worldDiscoveryDistance;
				if (debug) Debug.DrawRay(head.position, worldRay * worldDiscoveryDistance, new Color(1,1,0,0.55f));
				RaycastHit hit;
				Physics.Raycast(head.position, worldRay, out hit, worldDiscoveryDistance);
				if (hit.transform != null) {
					if (hit.transform.gameObject.tag == "Hider") {
						hit.transform.gameObject.SendMessage("Seen", SendMessageOptions.DontRequireReceiver);
						if (debug) Debug.DrawRay(head.position, hit.point - head.position, new Color(1,1,0,0.5f));
						worldRayEnd = hit.point;
					}			
				}
				
		
			}
			
			
			
			
		}
		
		
		
	}
}
