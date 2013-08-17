using UnityEngine;
using System.Collections;
using Pathfinding;

public class RigidbodyUpdater : MonoBehaviour {

	private bool fellAsleep = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (rigidbody.IsSleeping()) {
			if (!fellAsleep) {
				fellAsleep = true;
				print("Sleeping " + collider.bounds);
				AstarPath.active.UpdateGraphs (collider.bounds);

			}
		} else {
			fellAsleep = false;
		}
		 
	}
}
