using UnityEngine;
using System.Collections;

public class Pistol : MonoBehaviour {
	
	public GameObject shell;
	public AudioSource gunshot;

	public void Fire () {
		gunshot.Play();
		//GameObject spawnedShell = Instantiate(shell, transform.position + new Vector3(0,0.3f,0), transform.rotation) as GameObject;
		//spawnedShell.rigidbody.AddForce (Vector3.up * .1f, ForceMode.Impulse);
	}
	
}
