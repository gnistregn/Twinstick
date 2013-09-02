using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {

	public int currentLevel = 1;
	public string currentCorpName = "";
	public int playerCount = 1;

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
		
		currentCorpName = NameGenerator.GenerateCorpName();
		
	}
	
	
	public Player GetPlayer (int playerNumber) {
		return players[playerNumber];
	}
	
	
	public void GoUpLevel () {
		if (currentLevel < 100) currentLevel++;
		currentCorpName = NameGenerator.GenerateCorpName();
		Application.LoadLevel("Newlevel");
	}
	
	
	
}
