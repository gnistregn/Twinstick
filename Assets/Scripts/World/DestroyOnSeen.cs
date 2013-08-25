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

	private cWorldStats WS;
	
	private void Start()
	{
		WS = (cWorldStats)GameObject.Find("World").GetComponent("cWorldStats");
	}
	
	public void Seen () {
		if (WS)	WS.ExploreFloor((int)transform.position.x, (int)transform.position.z);
		Destroy(gameObject);
	}
}
