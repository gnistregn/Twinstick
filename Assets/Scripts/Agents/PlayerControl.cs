using UnityEngine;
using System.Collections;


/*
	
	TODO: Separate FOV/viewdistance from this script and move into separate reusable script
	
*/

public class PlayerControl : MonoBehaviour {



	public const float interactionReach = 1f; // How far can we reach to interact with something?

	
	CharacterController controller;


	private GameObject[] worldThings;
	
	public bool debug = false;

	


	public Transform torso;
	public Transform legs;
	public Transform gun;
		
	private Vector3 bodyDirection;
	private Vector3 bodyTargetDirection;
	private Vector3 aimDirection;
	private Vector3 aimTargetDirection;
	

	
	
	// Use this for initialization
	void Start () {
		
		controller = GetComponent<CharacterController>();
	
		//Messenger.AddListener("Awake", DropItem);
		
		bodyDirection = new Vector3(1,0,0);
		bodyTargetDirection = bodyDirection;
		aimDirection = new Vector3(1,0,0);
		aimTargetDirection = aimDirection;
		
	
		
	}
	
	
	
	
	void FixedUpdate () {
		

		// Input axii (?)
		float hA = 0;
		float vA = 0;
		float hB = 0;
		float vB = 0;

		if (debug) {
			
			// Use keyboard controls to move
			hA = Input.GetAxis("Keyboard Horizontal");
			vA = Input.GetAxis("Keyboard Vertical");

			if (Input.GetKeyDown (KeyCode.Space)) {
				GameObject n = GetComponent<PlayerEyes>().closestInteractible;
				float dist = GetComponent<PlayerEyes>().closestInteractibleDistance;
				if (n != null) {
					if (dist < interactionReach) {
						n.SendMessage("Interact");
					} else {
						print ("Too far away from " + n.name);
					}
				}
				
			}


		} else {

			// OSX + Xbox controls
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) 
			{

				hA = Input.GetAxis("Horizontal A");
				vA = Input.GetAxis("Vertical A");
				hB = Input.GetAxis("Horizontal B");
				vB = Input.GetAxis("Vertical B");

				if (Input.GetAxis("Fire") > 0) {
					gun.SendMessage("Trigger");
				}

				if (Input.GetAxis("Fire") < 0) {
					gun.SendMessage("Release");
				}
				
			// Windows + Xbox controls
			// Invert inputs to your liking
			} else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
			{

				hA = Input.GetAxis("WinHorizontal A");
				vA = Input.GetAxis("WinVertical A");
				hB = Input.GetAxis("WinHorizontal B");
				vB = Input.GetAxis("WinVertical B");

				if (Input.GetAxis("Fire") > 0) {
					gun.SendMessage("Trigger");
				}

				if (Input.GetAxis("Fire") < 0) {
					gun.SendMessage("Release");
				}

				if (Input.GetKey(KeyCode.JoystickButton0)) {
					GameObject n = GetComponent<PlayerEyes>().closestInteractible;
					float dist = GetComponent<PlayerEyes>().closestInteractibleDistance;
					if (n != null) {
						if (dist < interactionReach) {
							n.SendMessage("Interact");
						} else {
							print ("Too far away from " + n.name);
						}
					}
					
				}
				
			}


			
		
			
			
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
		float moveSpeed = targetDirection.magnitude * 2f;

		// Move slower when aiming backwards
		if (Vector3.Dot(bodyDirection, aimDirection) < 0) {
			if (-bodyDirection != Vector3.zero) legs.rotation = Quaternion.LookRotation(-bodyDirection);
			moveSpeed *= 0.5f;
		} else {
			if (bodyDirection != Vector3.zero) legs.rotation = Quaternion.LookRotation(bodyDirection);
		}

		

		
		// Move character
		Vector3 moveDirection = bodyDirection * moveSpeed; 
		moveDirection.y -= -Physics.gravity.y; // Apply gravity
		controller.Move(moveDirection * Time.deltaTime);
		
		
		
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
	
	

	
	
	
	
	
	
}
