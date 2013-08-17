using UnityEngine;
using System.Collections;
using Pathfinding;

public class AITest : MonoBehaviour {
	
	public Transform target;
	private Vector3 targetPosition;
	public Vector3 newTargetPosition;
	public float speed = 100;
	public float nextWaypointDistance = 3;
	private int currentWaypoint = 0;
	
	public Path path;
	
	private Seeker seeker;
	private CharacterController controller;
	
	// Use this for initialization
	void Start () {
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();
		targetPosition = target.position;
		seeker.StartPath(transform.position, targetPosition, OnPathComplete);
	}
	
	public void OnPathComplete (Path p) {
		Debug.Log("Yay! " + p.error);
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}
	
	public void FixedUpdate () {
		
		if (Input.GetKeyDown ("space")) {
			Debug.Log("new target");
			targetPosition = newTargetPosition;
			seeker.StartPath(transform.position, targetPosition, OnPathComplete);
		}
		
		if (path == null) {
			return;
		}
		
		if (currentWaypoint >= path.vectorPath.Length) {
			Debug.Log("End reached");
			return;
		}
		
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;
		controller.SimpleMove(dir);
		
		if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
		
	}
	
}
