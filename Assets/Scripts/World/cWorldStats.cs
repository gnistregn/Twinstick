using UnityEngine;
using System.Collections;

public class cWorldStats : MonoBehaviour
{

	public UILabel statusLabel;		// element for updating world GUI values
	public UILabel nextFloorLabel;
	private cMap MAP;
	private bool countExploredFloors = false;
	private int tempCount = 0;

	private bool oldDummyCheck = false, dummyCheck = false;
	
	private GameObject[] allPlayers;
	
	private GameMaster gameMaster;
	
	void Start()
	{
		MAP = (cMap)GetComponent("cMap");
		
		GameObject go = GameObject.Find("Game Master");
		if (go != null) gameMaster = go.GetComponent<GameMaster>();
		
	}
	
	void Update()
	{
		if (countExploredFloors)	CountExploredFloors();
		UpdateWorldLabel();
		UpdateNextFloorLabel();
		
		if (MAP)
		{
			if (MAP.level < 100)
			{
				oldDummyCheck = dummyCheck;
				dummyCheck = Input.GetKeyDown(KeyCode.Space);
				if ((!dummyCheck) && (oldDummyCheck))
					if (AreAllPlayersInEndArea())
						DummyNextLevel();
			}
		}
	}
	
	public bool AreAllPlayersInEndArea()
	{
		// Loop through all players, see if they are in the down staircase
		allPlayers = GameObject.FindGameObjectsWithTag("Player");
		if (allPlayers != null)
		{
			foreach (GameObject thing in allPlayers)
			{
				int px = (int)(thing.transform.position.x+.5f);
				int py = (int)(thing.transform.position.z+.5f);
				if ((px >= 0) && (px < MAP.levelWidth) && (py >= 0) && (py < MAP.levelHeight))
					if (MAP.floorMap[px,py] != 253)	// 253 = FLOOR_END
						return false;
			}
		}
		else
			return false;
		
		return true;
	}
	
	void DummyNextLevel()
	{
		Debug.Log("Next level!");
		gameMaster.GoUpLevel();
	//	Application.LoadLevel("Newlevel");
	//	MAP.NextLevel();
	}
	
	void UpdateNextFloorLabel()
	{
		nextFloorLabel.enabled = AreAllPlayersInEndArea();
	}
	
	void UpdateWorldLabel()
	{
		if (!MAP)	return;
		string perc = "";
		if (MAP.statsFloorCount > 0)	perc = "("+((MAP.statsFloorExploredCount*100)/MAP.statsFloorCount).ToString()+"%) ";
		if (statusLabel != null) statusLabel.text = MAP.level.ToString()+"/100"+"\n"+perc+MAP.statsFloorExploredCount.ToString()+"/"+MAP.statsFloorCount.ToString()+"\n"+MAP.statsSecretExploredCount.ToString()+"/"+MAP.statsSecretCount.ToString();
	}
	
	public void CountExploredFloors()
	{
		tempCount = 0;
		for(int i=0; i<MAP.levelWidth; i++)
			for(int j=0; j<MAP.levelHeight; j++)
				if (MAP.exploredMap[i,j] == 2)
					tempCount++;
		countExploredFloors = false;
		MAP.statsFloorExploredCount = tempCount;
	}
	
	public void ExploreFloor(int in_x, int in_y)
	{
		if ((in_x >= 0) && (in_x < MAP.levelWidth) && (in_y >= 0) && (in_y < MAP.levelHeight))
			if (MAP.exploredMap[in_x,in_y] == 1)
			{
				MAP.exploredMap[in_x,in_y] = 2;
				countExploredFloors = true;
			}
				
	}

	// public void IncreaseExploredSecrets(int in_x, int in_y)
	// {
		// //MAP.statsSecretExploredCount++;
	// }

}
