using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

	public Transform target;
	public float height;
	public float tilt;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(target.position.x, target.position.y+15, target.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (target) {
			
			transform.RotateAround(target.position, Vector3.right, 20 * Time.deltaTime);
		}
	}
}
