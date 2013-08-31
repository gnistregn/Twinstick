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
	private float springStrength = 0f;
	private float triggerTime = 0f;

	private void Start () 
	{
		hingeJoint = GetComponent<HingeJoint>();
		spr = hingeJoint.spring;
		defaultBounds = collider.bounds;
		defaultBounds.size *= 1.2f;
		
		if (reverseDirection) 
		{
			hingeJoint.anchor = new Vector3(hingeJoint.anchor.x, hingeJoint.anchor.y, -hingeJoint.anchor.z);
			JointLimits limits = hingeJoint.limits;
			limits.min = -160;
			limits.max = 0;
		}
		
	}
	

	public void Open () 
	{	
		Debug.Log("Open door");
		refreshState = true;
		spr.targetPosition = reverseDirection ? -160 : 160;
		open = true;
		triggerTime = Time.time;
		rigidbody.isKinematic = false;
		springStrength = 0f;
	}
	
	public void Close () 
	{
		Debug.Log("Close door");
		refreshState = true;
		open = false;
		springStrength = 0f;
		triggerTime = Time.time;
	}
	
	private void SetSpring (float v) {
		spr.spring = v;
		spr.damper = 0.05f;
		spr.targetPosition = open ? (reverseDirection ? -160 : 160) : 0;
		hingeJoint.spring = spr;
	}
	
	
	public void Update () {
		
		springStrength = Mathf.Lerp(springStrength, 0.5f, (Time.time-triggerTime) / 20);
		SetSpring(springStrength);
		float dist = Mathf.Abs(transform.localEulerAngles.y - spr.targetPosition) * Mathf.Deg2Rad;
		if (dist < .01f) {
			if (refreshState) {

				Debug.Log("Refreshing");
				refreshState = false;
				AstarPath.active.UpdateGraphs (defaultBounds);
				
				
			}
		}
	}
	
	void OnCollisionEnter(Collision collision) 
	{	
		// Smooth out collisions with players by reducing the torque to zero
		if (collision.gameObject.tag == "Player") {
			springStrength = 0;
			triggerTime = Time.time;
		}
		
	}
	
	
}
