using UnityEngine;
using System.Collections;

public class Boink : MonoBehaviour {

	public void Jump () {
		Debug.Log("Jump");
		rigidbody.AddForce (Vector3.up / 5, ForceMode.Impulse);
	}

}
