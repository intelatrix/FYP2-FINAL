using UnityEngine;
using System.Collections;

using GooglePlayGames;
using UnityEngine.SocialPlatforms;

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
	
	//Used to Sign in
	public void StartUp()
	{
		Social.localUser.Authenticate((bool success) => {
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
			
		} 
		else 
		{
			
		}
	}
	
	public void OnLeftRoom() {
		// display error message and go back to the menu screen
		
		// (do NOT call PlayGamesPlatform.Instance.RealTime.LeaveRoom() here --
		// you have already left the room!)
	}
	
	public void OnPeersConnected(string[] participantIds) {
		// react appropriately (e.g. add new avatars to the game)
	}
	
	public void OnPeersDisconnected(string[] participantIds) {
		// react appropriately (e.g. remove avatars from the game)
	}
	
	public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data) {
		// handle message! (e.g. update player's avatar)
	}
	
	enum MessageType
	{

	}
}
	
	
	