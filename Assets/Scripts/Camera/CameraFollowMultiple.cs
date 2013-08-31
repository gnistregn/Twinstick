using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollowMultiple : MonoBehaviour {
	
	//public GameObject[] targets;
	public List<Transform> targets;
	Bounds bounds;
	
	
	public void AddTarget (Transform t) {
		targets.Add(t);
	}
	
	
	
	// Update is called once per frame
	void LateUpdate () {
		
	
		if (targets.Count > 0) {
			bounds = new Bounds(targets[0].transform.position, new Vector3(1,1,1));
			for (int i = 1; i < targets.Count; i++) {
				bounds.Encapsulate(targets[i].transform.position);
			}	
			iTween.MoveUpdate(gameObject,bounds.center,1.6f);
			float biggestSize = Mathf.Max(bounds.extents.x, bounds.extents.z);
		
		
		}
		
		
	}
}
