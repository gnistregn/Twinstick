using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyEyes : MonoBehaviour {

	public Transform head;
	
	private GameMaster gameMaster;
	
	private GameObject[] players;	// Array of all the players
	private float playerListRefreshTime = 0;
	private float playerListRefreshTimeout = 1;
	
	
	public float viewDistance = 5; 	// How far can it see?
	public float totalFOV = 90;		// The total Field of View
	private float halfFOV;			// Half of the FOV
	
	
	
	public void Start () 
	{
		
		// Set this FOV variable
		halfFOV = totalFOV / 2;
		
		
		GameObject go = GameObject.Find("Game Master");
		if (go != null) gameMaster = go.GetComponent<GameMaster>();	
		
		players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	
	public void Update () 
	{
		// List of players currently visible
		List<GameObject> visiblePlayers = GetVisiblePlayers();
		
		// If any of the players are visible...
		if (visiblePlayers.Count > 0)
		{
			// Tell the rest of the game object!
			SendMessage("Seeing", visiblePlayers, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	
	
	// Function for fetching a list of all visible players
	List<GameObject> GetVisiblePlayers () 
	{
		
		
		// Create list for visible players
		List<GameObject> visiblePlayers = new List<GameObject>();
		
		
		
		// Step through all the players
		foreach (GameObject player in players) {
			
			// If the player actually exists
			if (player != null) 
			{

				RaycastHit hit;
				Vector3 rayDirection = new Vector3(player.transform.position.x, .5f, player.transform.position.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z);
		
				if (Vector3.Angle(rayDirection, head.transform.forward) < halfFOV) 
				{
					if (Physics.Raycast(head.transform.position, rayDirection, out hit)) 
					{
						if (hit.transform.tag == "Player") 
						{
							visiblePlayers.Add(player);
						} 
					}
				}

			}
		
		}
		
		
		/* 
		
			Add distance sorting here!
		
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
		
		
		*/
		
		
		return visiblePlayers;
		
		
	}
	
	
	void OnDrawGizmos () 
	{
		
		Gizmos.color = Color.blue;
		
		// Draw FOV lines
		Quaternion leftRayRotation = Quaternion.AngleAxis( -halfFOV, Vector3.up );
		Quaternion rightRayRotation = Quaternion.AngleAxis( halfFOV, Vector3.up );
		Vector3 leftRayDirection = leftRayRotation * head.transform.forward;
		Vector3 rightRayDirection = rightRayRotation * head.transform.forward;
		Gizmos.DrawRay( head.transform.position, leftRayDirection);
		Gizmos.DrawRay( head.transform.position, rightRayDirection);
		
	}
	
	
}
