using UnityEngine;
using System.Collections;
using Pathfinding;

public class DoorControl : MonoBehaviour {

	
	public bool reverseDirection = false;
	private HingeJoint hingeJoint;
	private JointSpring spr;
	private bool refreshState = false;
	private Bounds defaultBounds;
	private bool open = false;

	private void Start () 
	{
		hingeJoint = GetComponent<HingeJoint>();
		spr = hingeJoint.spring;
		defaultBounds = collider.bounds;
		defaultBounds.size *= 1.2f;
	}
	

	public void Open () 
	{	
		Debug.Log("Open");
		refreshState = true;
		spr.spring = 0.5f;
		spr.damper = 0.05f;
		spr.targetPosition = reverseDirection ? -160 : 160;
		hingeJoint.spring = spr;
		rigidbody.isKinematic = false;
	}
	
	public void Close () 
	{
		Debug.Log("Close");
		refreshState = true;
		spr.spring = 0.5f;
		spr.damper = 0.05f;
		spr.targetPosition = 0;
		hingeJoint.spring = spr;
	}
	
	public void Update () {
		float dist = Mathf.Abs(spr.targetPosition - transform.localEulerAngles.y);
		if (dist < 1) {
			if (refreshState) {
				if (spr.targetPosition == 0) open = false;
				else open = true;
				Debug.Log("Refreshing");
				refreshState = false;
				AstarPath.active.UpdateGraphs (defaultBounds);
				
				if (!open) rigidbody.isKinematic = true;

				
				
			}
		}
	}
	
	
}
