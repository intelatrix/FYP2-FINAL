using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GooglePlayGames.BasicApi.Multiplayer;

public class MultiplayerGameplayManager : MonoBehaviour {

	GameObject PlayerPrefab;
	public GameObject MultplayerTest;
	
	List<GameObject> ListOfPlayers;
	public List<Transform> ListOfPosition;
	
	GameObject ThisPlayer;
	string ThisPlayerID; 

	private static MultiplayerGameplayManager mInstance;
	
	//This is the public reference that other classes will use
	public static MultiplayerGameplayManager Instance
	{
		get
		{
			//If _instance hasn't been set yet, we grab it from the scene!
			//This will only happen the first time this reference is used.
			if(mInstance == null)
				mInstance =  new MultiplayerGameplayManager();
			return mInstance;
		}
	}
	
	
	void Awake() 
	{
		if(mInstance == null)
		{
			//If I am the first instance, make me the Singleton
			mInstance = this;
			DontDestroyOnLoad(this);
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != mInstance)
				Destroy(this.gameObject);
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetUpGame(Participant[] Players, string ThisPlayer)
	{
		int Counter = 0;
		foreach(Participant Player in Players)
		{
			//Spawn the Player to how many there are
			GameObject SpawnedPlayer = Instantiate(PlayerPrefab, ListOfPosition[Counter].position, Quaternion.identity) as GameObject;
			++Counter;
			
			if(Player.ParticipantId == ThisPlayer)
			{
			//Map Controls to this Player
			}
			
			
		}
	}
}
