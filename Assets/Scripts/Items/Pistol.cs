using UnityEngine;
using System.Collections;

public class Pistol : MonoBehaviour {
	
	public GameObject shell;
	public AudioSource gunshot;
	public GameObject hitGraphic;

	private bool firedGun = false;

	public void Trigger () {
		
		if (!firedGun) {
			firedGun = true;
			gunshot.Play();

			var fwd = transform.TransformDirection (Vector3.forward);
			RaycastHit hit;

			if (Physics.Raycast (transform.position, transform.forward, out hit)) {
				Debug.Log("hit! " + hit.normal);
				hit.transform.gameObject.SendMessage("TakeDamage", 25, SendMessageOptions.DontRequireReceiver);
				Instantiate(hitGraphic, hit.point, Quaternion.FromToRotation(transform.up,  Vector3.Reflect (transform.forward, hit.normal)));
			}
			
		}
		
	}
	
	public void Release () {
		firedGun = false;
	}
	
	
	
}
