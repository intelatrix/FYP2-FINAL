using UnityEngine;
using System.Collections;
using GooglePlayGames.BasicApi.Multiplayer;

public class Listener : RealTimeMultiplayerListener
{
	MultiplayerManager MultiManager;
	
	public void Init(MultiplayerManager TempM)
	{
		MultiManager = TempM;
	}
	
	public void OnRoomSetupProgress(float progress) 
	{
		MultiManager.OnRoomSetupProgress(progress);
	}
	
	public void OnRoomConnected(bool success)
	{
		MultiManager.OnRoomConnected(success);
	}
	
	public void OnLeftRoom() 
	{
		// display error message and go back to the menu screen
		// (do NOT call PlayGamesPlatform.Instance.RealTime.LeaveRoom() here --
		// you have already left the room!)
		MultiManager.OnLeftRoom();
	}
	
	public void OnPeersConnected(string[] participantIds)
	{
		// react appropriately (e.g. add new avatars to the game)
		MultiManager.OnPeersConnected(participantIds);
	}
	
	public void OnPeersDisconnected(string[] participantIds)
	{
		// react appropriately (e.g. remove avatars from the game)
		MultiManager.OnPeersDisconnected(participantIds);
	}
	
	public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
	{
		MultiManager.OnRealTimeMessageReceived(isReliable, senderId, data);
	}
	
	public void OnParticipantLeft(Participant participant)
	{}
}
