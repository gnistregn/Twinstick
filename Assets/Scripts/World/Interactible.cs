using UnityEngine;
using System.Collections;

public class Interactible : MonoBehaviour {

	public bool toggle = false;
	private bool toggled = true;
	public string ActivateMessage = "";
	public string DeactivateMessage = "";

	private  void Start () {
		//StartCoroutine("Test");
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
		if (toggled) gameObject.SendMessage(ActivateMessage);
		else gameObject.SendMessage(DeactivateMessage);
		if (toggle) toggled = !toggled;		
	}
	
}
