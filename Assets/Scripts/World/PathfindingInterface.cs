using UnityEngine;
using System.Collections;
using Pathfinding;


public class PathfindingInterface : MonoBehaviour {


	public void UpdatePathfinding (Bounds b) {
		print("Updating graph");
		AstarPath.active.UpdateGraphs (b);
	}

}
