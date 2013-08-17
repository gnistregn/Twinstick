using UnityEngine;
using System.Collections;
using Pathfinding;

public class SceneUpdater : MonoBehaviour {

	public GameObject obstaclePrefab;

	// Use this for initialization
	void Start () {
		GameObject obstacle = GameObject.Instantiate (obstaclePrefab,new Vector3(-2,10,3),Quaternion.identity) as GameObject;
		GameObject.Instantiate (obstaclePrefab,new Vector3(-2,14,3.6f),Quaternion.identity);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}
}
