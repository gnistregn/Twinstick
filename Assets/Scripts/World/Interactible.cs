using UnityEngine;
using System.Collections;

public class Interactible : MonoBehaviour {

	public bool toggle = false;
	private bool toggled = true;
	public GameObject target = null;
	public string ActivateMessage = "";
	public string DeactivateMessage = "";

	private  void Start () {
		//StartCoroutine("Test");
		if (target == null) target = gameObject;
	}
	
	IEnumerator Test () {
		yield return new WaitForSeconds(1f);
		Interact();
		yield return new WaitForSeconds(2f);
		Interact();
		yield return new WaitForSeconds(2f);
		Interact();
		yield return new WaitForSeconds(2f);
		Interact();
		yield return new WaitForSeconds(2f);
		Interact();
		yield return new WaitForSeconds(2f);
		Interact();
		yield return new WaitForSeconds(2f);
		Interact();

	}

	public void Interact () {
		
		print("Interacting");
		
		if (toggled) target.SendMessage(ActivateMessage);
		else target.SendMessage(DeactivateMessage);
		if (toggle) toggled = !toggled;		
	}
	
}
