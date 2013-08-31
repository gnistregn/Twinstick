using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

	public int currentLevel = 1;
	public int playerCount = 2;

	private List<Player> players;

	void Awake () 
	{
	
		DontDestroyOnLoad(transform.gameObject);

		players = new List<Player>();
		
		for (int i = 0; i < playerCount; i++)
		{
			Player p = new Player();
			players.Add(p);
		}
		
	}
	
	
	public Player GetPlayer (int playerNumber) {
		return players[playerNumber];
	}
	
	
	public void GoUpLevel () {
		if (currentLevel < 100) currentLevel++;
		Application.LoadLevel("Newlevel");
	}
	
	
	
}
