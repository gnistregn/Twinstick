using UnityEngine;
using System.Collections;

public class DestroyOnSeen : MonoBehaviour {

	/*private Component[] renderers;

	public void Start () {
		
		renderers = GetComponentsInChildren<Renderer>();
		if (renderers != null) {
			foreach (Renderer r in renderers) {
				r.material.color = new Color(0,0,0,.1f);
			}
		}
	}*/

	public void Seen () {
		Destroy(gameObject);
	}
}
