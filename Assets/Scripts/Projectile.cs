using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public float speed = 1f;
	public float damage = 20f;
	

	public GameObject graphic;
	public ParticleSystem particleSystem;
	
	private float deathTime = 0;
	
	void Start () {
		
		rigidbody.AddForce (transform.forward * speed);
		
	}
	
	void Update () {
		
		if (deathTime > 0 && Time.time >= deathTime + particleSystem.startLifetime) {
			Destroy(gameObject);
		}
		
	}
	
	
	void OnCollisionEnter (Collision collision) {
		
		deathTime = Time.time;
		Destroy(graphic);
		particleSystem.emissionRate = 0;
		collision.gameObject.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
		
	}
	
}
