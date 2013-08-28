using UnityEngine;
using System.Collections;

public class Discoverable : MonoBehaviour {

	public bool inverted = false;
	public bool LockAfterDiscovery = false;
	private bool discovered = false;
	private Component[] renderers;
	private float hideDelay = .2f;
	
//	private float alpha = 0;
	private float visibility = 0;
////
//	private bool useAlpha = false;
	

	public void Start () {
		renderers = GetComponentsInChildren<Renderer>();
//		alphaTarget = 0;
	}

	public void Seen () {
		discovered = true;
		visibility = 1;
	}
	
	public void Unseen () {
		StartCoroutine("Unsee");
	}
	
	IEnumerator Unsee () {
		yield return new WaitForSeconds(hideDelay);
		if (!LockAfterDiscovery && discovered) visibility = 0;
	}
	
	public void FixedUpdate () {
//		alpha = Mathf.Lerp(alpha, alphaTarget, Time.deltaTime * 15);
		if (renderers != null) {
			foreach (Renderer r in renderers) {
				
/*				if (useAlpha) {

					Color defaultColor = r.material.color;
					r.material.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, alpha);
					if (alpha == 0)	r.enabled = false;
					else r.enabled = true;
				
				} else {*/
					
					if (visibility == 0) {
						r.enabled = false;
						//gameObject.SetActive(inverted);
					}
					else {
						r.enabled = true;
						//gameObject.SetActive(!inverted);
					}
					
				//}
			}
		}

	}
	
	


}
