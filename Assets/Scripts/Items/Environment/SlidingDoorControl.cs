using UnityEngine;using UnityEngine;
using System.Collections;
using Pathfinding;

public class SlidingDoorControl : MonoBehaviour {

	
	// Settings
	public bool locked = false;
	public bool reverseDirection = false;
	public float speed = 1f;
	
	// Animation
	private float triggerTime = 0f;
	private float slideDistance = .65f;
	private float slideTarget = 0f;
	private float slideStart = 0f;
	private float slidePos = 0f;
	private float startPos;

	// Pathfinding
	public GameObject doorblock; // Dummy item to block pathfinders
	private Bounds defaultBounds;
	private bool refreshState = false;
	
	private void Start () 
	{
		
		
		startPos = transform.localPosition.x;

		defaultBounds = collider.bounds;
		defaultBounds.size *= 2f;
		
		if (reverseDirection) 
		{
	
		}
		
		refreshState = true;
		
	}
	

	public void Open () 
	{	
		Debug.Log("Open door");
		refreshState = true;
		slideTarget = reverseDirection ? -slideDistance : slideDistance;
		triggerTime = Time.time;
		ClearLockState();
	}
	
	public void Close () 
	{
		Debug.Log("Close door");
		refreshState = true;
		slideTarget = 0;
		triggerTime = Time.time;
	}
	
	public void EnemyUsable (bool n) {

		doorblock.SetActive(!n);
		AstarPath.active.UpdateGraphs (defaultBounds);
		
	}
	
	public void CheckLockState () {
		
		if (locked) {
			EnemyUsable(false);
		} else {
			Open();
		}
		
	}
	
	public void ClearLockState () {
		Debug.Log("Clearing lock state");
		EnemyUsable(true);
		
	}
	
	
	public void Update () {
		
		slidePos = Mathf.Lerp(slidePos, slideTarget, (Time.time-triggerTime) * speed);
		transform.localPosition = new Vector3(startPos + slidePos, 0, 0);
	
		float dist = Mathf.Abs(slideTarget - slidePos);

		if (dist == 0) {
			if (refreshState) {
				Debug.Log("Refreshing");
				refreshState = false;
				AstarPath.active.UpdateGraphs (defaultBounds);
			
			}
		}
	}
	

	
}
