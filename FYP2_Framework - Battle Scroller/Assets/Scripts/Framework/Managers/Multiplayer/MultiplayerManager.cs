using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GooglePlayGames;
using UnityEngine.SocialPlatforms;

using System.Runtime.Serialization.Formatters.Binary;
using System;
using GooglePlayGames.BasicApi.Multiplayer;

public class MultiplayerManager : GooglePlayGames.BasicApi.Multiplayer.RealTimeMultiplayerListener {

	protected MultiplayerManager()
	{
		CurrentOnlineStatus = SignInStatus.SIGN_IN_NONE;
	}
	//Used to check current state of signing in
	public enum SignInStatus
	{
		SIGN_IN_NONE,
		SIGN_IN_FAILED,
		SIGN_IN_PASS,
	}
	
	private static MultiplayerManager mInstance;
	
	//This is the public reference that other classes will use
	public static MultiplayerManager Instance
	{
		get
		{
			//If _instance hasn't been set yet, we grab it from the scene!
			//This will only happen the first time this reference is used.
			if(mInstance == null)
				mInstance =  new MultiplayerManager();
			return mInstance;
		}
	}
	
	public SignInStatus CurrentOnlineStatus{ get; private set; }
	public String MyPID{ get; private set; }
	String HostID;
	
	//Used to Sign in
	public void StartUp()
	{
		Social.localUser.Authenticate((bool success) => 
		{
			// handle success or failure
			if(success)
				CurrentOnlineStatus = SignInStatus.SIGN_IN_PASS;
			else
				CurrentOnlineStatus = SignInStatus.SIGN_IN_FAILED;
		});
	
	}
	
	const int MinOpponents = 1, MaxOpponents = 3;
	const int GameVariant = 0;
	
	public void AutoMatch()
	{
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MinOpponents, MaxOpponents, GameVariant, this);
	}
	
	public void OnRoomSetupProgress(float progress) 
	{
		// update progress bar
		// (progress goes from 0.0 to 100.0)
		// Progress of Joining Room
	}
	
	public void OnRoomConnected(bool success)
	{
		if (success) 
		{
			MyPID = GetSelf().ParticipantId;
			SetUpHost();
		} 
		else 
		{	
			
		}
	}
	
	public void OnLeftRoom() 
	{
		// display error message and go back to the menu screen
		// (do NOT call PlayGamesPlatform.Instance.RealTime.LeaveRoom() here --
		// you have already left the room!)
	}
	
	public void OnPeersConnected(string[] participantIds)
	 {
		// react appropriately (e.g. add new avatars to the game)
	}
	
	public void OnPeersDisconnected(string[] participantIds)
	{
		// react appropriately (e.g. remove avatars from the game)
	}
	
	public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
	{
		MessageType Type = (MessageType)data[0];
		RemoveMessageType(ref data);
		
		switch(Type)
		{
			case MessageType.MESSAGE_STARTGAME:
				String result = System.Text.Encoding.UTF8.GetString(data);
				Debug.Log (result);
				break;
		}
	}
	
	private Participant GetSelf()
	{
		return PlayGamesPlatform.Instance.RealTime.GetSelf();
		PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
	}
	
	private List<Participant> GetAllP()
	{
		return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
	}
	
	private void SetUpHost()
	{
		List<Participant> Players = GetAllP();
		if(Players[0].ParticipantId == MyPID)
		{
			//You are chosen to randomise the host
			int ChosenHost = UnityEngine.Random.Range(0,4);
			
			String HostID = Players[ChosenHost].ParticipantId;
			
			byte[] Type = {(byte)MessageType.MESSAGE_STARTGAME};
			byte[] Message = System.Text.Encoding.UTF8.GetBytes(HostID);
			
			byte[] FinalMessage = CombineMessages(Type,Message);
			
			PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, FinalMessage);
		}
	}
	
	private byte[] CombineMessages(byte[] Array1, byte[] Array2)
	{
		byte[] Array3 = new byte[Array1.Length + Array2.Length];
		Array1.CopyTo(Array3, 0);
		Array2.CopyTo(Array3, Array1.Length);
		
		return Array3;
	}
	
	private void RemoveMessageType(ref byte[] Message)
	{
		List<byte> list = new List<byte>(Message);
		list.RemoveAt(0);
		Message = list.ToArray();
	}
	
	enum MessageType
	{
		MESSAGE_STARTGAME
	}
	
	
	[Serializable]
	public struct GameStart
	{
		String HostID;
	}
	
}
	
	
	