using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


	public int hitPoints = 100;
	
	CharacterController controller;

	public float viewDistance = 5; 	// How far can I see?
	public float totalFOV = 90;		// The total Field of View
	private float halfFOV;			// Half of the FOV

	private GameObject[] worldThings;


	public Transform torso;
	public Transform legs;
	public Transform gun;
	
	public Transform eyes;
	
	public GameObject hitGraphic;
	
	private Vector3 bodyDirection;
	private Vector3 bodyTargetDirection;
	private Vector3 aimDirection;
	private Vector3 aimTargetDirection;
	
	private bool firedGun;
	
	
	// Use this for initialization
	void Start () {
		
		controller = GetComponent<CharacterController>();
	
		
		
		halfFOV = totalFOV / 2;
		
		bodyDirection = new Vector3(1,0,0);
		bodyTargetDirection = bodyDirection;
		aimDirection = new Vector3(1,0,0);
		aimTargetDirection = aimDirection;
		
		firedGun = false;
		
		worldThings = GameObject.FindGameObjectsWithTag("Enemy");
		
		
	}
	
	
	
	
	void FixedUpdate () {
		
	
			// What can I see?
			if (worldThings != null) {
				
					foreach (GameObject thing in worldThings) {

						if (thing != null) {

							Vector3 rayDirection = thing.transform.position - eyes.position;
							//Debug.DrawRay(eyes.position, rayDirection, Color.blue);
							RaycastHit hit;
							if (Vector3.Angle(rayDirection, aimDirection) < halfFOV) {
								Physics.Raycast(eyes.position, rayDirection, out hit);
								if (hit.transform.gameObject == thing) {
								//	Debug.DrawRay(eyes.position, hit.point - eyes.position, Color.cyan);
									hit.transform.gameObject.SendMessage("Seen", SendMessageOptions.DontRequireReceiver);
								} else {
									thing.SendMessage("Unseen", SendMessageOptions.DontRequireReceiver);
								}
							} else {
								thing.SendMessage("Unseen", SendMessageOptions.DontRequireReceiver);
							}

						}


					}
				
			}
		
			
			


		float hA = Input.GetAxis("Horizontal A");
		float vA = Input.GetAxis("Vertical A");
		float hB = Input.GetAxis("Horizontal B");
		float vB = Input.GetAxis("Vertical B");
 		


 		if (Input.GetAxis("Fire") > 0 && !firedGun) {
			Debug.Log("Fire!");
			firedGun = true;
			gun.SendMessage("Fire");
			var fwd = transform.TransformDirection (Vector3.forward);
			RaycastHit hit;
			Debug.Log(aimDirection);
			if (Physics.Raycast (gun.position, aimDirection, out hit)) {
				Debug.Log("hit! " + hit.normal);
				hit.transform.gameObject.SendMessage("TakeDamage", 25, SendMessageOptions.DontRequireReceiver);
				Instantiate(hitGraphic, hit.point, Quaternion.FromToRotation(transform.up,  Vector3.Reflect (aimDirection, hit.normal)));
			}

		}
		
		if (Input.GetAxis("Fire") < 0) {
			firedGun = false;
		}
		
		
		
		/* --- MOVE --- */
		
		
		Vector3 targetDirection = hA * Vector3.right + vA * Vector3.forward;
		targetDirection = targetDirection.normalized;
		
		if (targetDirection.magnitude > .3f) {		
			bodyTargetDirection = targetDirection;
		}
		
		
		
		// Rotate towards aiminig direction
		bodyDirection = Vector3.RotateTowards(bodyDirection, bodyTargetDirection, 10f * Time.deltaTime, 1000);

		// Move speed
		float moveSpeed = targetDirection.magnitude * 3;

		// Move slower when aiming backwards
		if (Vector3.Dot(bodyDirection, aimDirection) < 0) {
			if (-bodyDirection != Vector3.zero) legs.rotation = Quaternion.LookRotation(-bodyDirection);
			moveSpeed *= 0.5f;
		} else {
			if (bodyDirection != Vector3.zero) legs.rotation = Quaternion.LookRotation(bodyDirection);
		}

		

		
		// Move character
		//transform.Translate(bodyDirection * Time.deltaTime * moveSpeed);
		controller.Move(bodyDirection * Time.deltaTime * moveSpeed);
		// Draw debug ray
		Debug.DrawRay(transform.position, bodyTargetDirection, Color.green);


		
		
		/* --- AIM --- */
		
		// Assemble target direction vector
		targetDirection = hB * Vector3.right + vB * Vector3.forward;
		// Normalize
		targetDirection = targetDirection.normalized;
		
		// Ignore dead zone
		if (targetDirection.magnitude > .3f) {
			aimTargetDirection = targetDirection;
		} else {
			aimTargetDirection = bodyDirection;
		}
		
		// Rotate towards aiming direction
		aimDirection = Vector3.RotateTowards(aimDirection, aimTargetDirection, 10f * Time.deltaTime, 1000);
		// Rotate graphics
		if (aimDirection != Vector3.zero) torso.rotation = Quaternion.LookRotation(aimDirection);
		// Draw debug ray
		Debug.DrawRay(gun.position, aimTargetDirection * 3, Color.red);

	
		
		
	}
	
	
	public void TakeDamage (int amount) {
		Debug.Log("Took damage " + amount);
		hitPoints -= amount;
		if (hitPoints < 0) {
			Destroy(gameObject);
		}
	}
	
	
	
	
	
	void OnDrawGizmos () {
		
		// FOV Gizmo
		Gizmos.color = Color.green;
		Quaternion leftRayRotation = Quaternion.AngleAxis( -halfFOV, Vector3.up );
		Quaternion rightRayRotation = Quaternion.AngleAxis( halfFOV, Vector3.up );
		Vector3 leftRayDirection = leftRayRotation * aimDirection;
		Vector3 rightRayDirection = rightRayRotation * aimDirection;
		Gizmos.DrawRay( transform.position, leftRayDirection * viewDistance );
		Gizmos.DrawRay( transform.position, rightRayDirection * viewDistance);
		
	}
	
	
	
}
