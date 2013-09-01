using UnityEngine;
using System.Collections;
using Pathfinding;

public class SlidingDoorControl : MonoBehaviour {

	
	public bool reverseDirection = false;
	public float speed = 1f;
	private Bounds defaultBounds;
	private bool refreshState = false;
	private float triggerTime = 0f;
	private float slideDistance = .55f;
	private float slideTarget = 0f;
	private float slideStart = 0f;
	private float slidePos = 0f;
	private float startPos;
	
	private void Start () 
	{
		
		startPos = transform.localPosition.x;


		defaultBounds = collider.bounds;
		defaultBounds.size *= 1.2f;
		
		if (reverseDirection) 
		{
	
		}
		
	}
	

	public void Open () 
	{	
		Debug.Log("Open door");
		refreshState = true;
		slideTarget = reverseDirection ? -slideDistance : slideDistance;
		triggerTime = Time.time;
	}
	
	public void Close () 
	{
		Debug.Log("Close door");
		refreshState = true;
		slideTarget = 0;
		triggerTime = Time.time;
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
