using UnityEngine;
using System.Collections;

public class Populator : MonoBehaviour {

	// This script is responsible for populating the level with enemies and stuff
	
	public bool generateEnemies = true;
	public bool generateLoot = true;
	public bool generateFurniture = true;
	
	
	public GameObject pfbPlayer;
	public GameObject pfbEnemy;
	
	private CMap map;
	private GameMaster gameMaster;
	
	void Start () {
		
		GameObject go = GameObject.Find("Game Master");
		if (go != null) gameMaster = go.GetComponent<GameMaster>();
		
		map = GetComponent<CMap>();
		
		if (generateEnemies) GenerateEnemies(); // Make some enemies!
		
		GeneratePlayers();
		
		
	}
	
	
	private void GeneratePlayers () 
	{
		
		int playerCount = gameMaster.playerCount;
		
		Vector3 startPosition = map.GetStartPosition();
		
		for (int i = 0; i < playerCount; i++) {
			
			
			
			// Create player object
			GameObject p = Instantiate(pfbPlayer, startPosition, Quaternion.identity) as GameObject;
			
			// Name it properly
			p.name = "Player " + (i + 1);
			
			// Tell this gameObject which class instance contains all data
			p.SendMessage("SetPlayer", gameMaster.GetPlayer(i));
						
			// Find the camera
			GameObject cameraRig = GameObject.Find("Camera Rig");
			
			// Tell the camera to watch this player too
			if (cameraRig != null) cameraRig.SendMessage("AddTarget", p.transform);
			
		}
		
	}
	
	
	
	private void GenerateEnemies () 
	{
		for (int i = 0; i < 10; i++) 
		{
			Vector3 enemyStartPoint = map.GetRandomSquareOfType(CMap.FLOOR_CORRIDOR); // Enemy starts in a random piece of corridor
			GameObject e = Instantiate(pfbEnemy, enemyStartPoint, Quaternion.AngleAxis(Random.Range(0,360), Vector3.up)) as GameObject;
			e.name = "Enemy " + i;
		}
	}
	
	
	
	
	

}
