using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;

using System.Runtime.Serialization.Formatters.Binary;
using System;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Runtime.InteropServices;

using UnityEngine.UI;

public class MultiplayerManager : MonoBehaviour  {

	int NoReadied = 0;
	bool Ready = false;
	public GameObject TextTesting;

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
		SIGN_IN_GAME
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
	
	Listener GameListener = new Listener();
	
	System.Action<bool> mAuthCallback;
	
	//To Test Decoding and Encoding
	void Start()
	{	
		GameListener.Init(this);
		
		mAuthCallback = (bool success) => 
		{
			if (success)
			{
				TextTesting.GetComponent<Text>().text = "Authenticated";
			}
			else 
			{
				TextTesting.GetComponent<Text>().text = "Authentication Failed";
			}
		};
		
	
		M_GameStart Test = new M_GameStart();
		Test.HostID = "Allson";
		Debug.Log (Test.HostID );
		byte[] MiddleMan =  getBytes(Test);
		
		M_GameStart Test2 = new M_GameStart();
		fromBytes(MiddleMan, ref Test2);
		Debug.Log (Test2.HostID );
		
		M_GameStart Test3 = new M_GameStart();
		byte[] Type = {(byte)MessageType.MESSAGE_STARTGAME};
		byte[] Message = MiddleMan;
		byte[] FinalMessage = CombineMessages(Type,Message);
		fromBytes(FinalMessage, ref Test3);
		Debug.Log (Test3.HostID );
		
		M_GameStart Test4 = new M_GameStart();
		MessageType TypeOfMessage = (MessageType)FinalMessage[0];
		RemoveMessageType(ref FinalMessage);
		fromBytes(FinalMessage, ref Test4);
		Debug.Log (Test4.HostID );
		
		switch(TypeOfMessage)
		{
		case MessageType.MESSAGE_STARTGAME:
			Debug.Log ("Loving it");
			break;
		default: 
			Debug.Log ("You fucked up");
			break;
		}
		
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();

		PlayGamesPlatform.Instance.Authenticate(mAuthCallback);
		
		StartUp();
		

	}
	
	
	void Update()
	{
		if(CurrentOnlineStatus == SignInStatus.SIGN_IN_PASS)
		{
			CurrentOnlineStatus = SignInStatus.SIGN_IN_GAME;
		}
	}
	//Used to Sign in
	public void StartUp()
	{
		TextTesting.GetComponent<Text>().text = "Starting Up";
		Social.localUser.Authenticate((bool success) => 
		{
			// handle success or failure
			if(success)
			{
				CurrentOnlineStatus = SignInStatus.SIGN_IN_PASS;
				TextTesting.GetComponent<Text>().text = "Connected";
			}
			else
				CurrentOnlineStatus = SignInStatus.SIGN_IN_FAILED;
		});
	
	}
	
	const int MinOpponents = 2, MaxOpponents = 4;
	const int GameVariant = 1;
	
	public void AutoMatch()
	{
		TextTesting.GetComponent<Text>().text = "Automatching";
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MinOpponents, MaxOpponents, GameVariant, GameListener);
	}
	
	public void OnRoomSetupProgress(float progress) 
	{
		// update progress bar
		// (progress goes from 0.0 to 100.0)
		// Progress of Joining Room
		TextTesting.GetComponent<Text>().text = progress.ToString();
	}
	
	public void OnRoomConnected(bool success)
	{
		if (success) 
		{
			TextTesting.GetComponent<Text>().text = "Room Connected";
			//MyPID = GetSelf().ParticipantId;
			//MD_SetUpHost();
		} 
		else 
		{	
			TextTesting.GetComponent<Text>().text = "Room Connection Failed";
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
			//Received what the ID of the host is
			case MessageType.MESSAGE_SETUPHOST:
			
				M_GameStart Message = new M_GameStart();
				fromBytes(data, ref Message);
				Debug.Log (Message.HostID);
				HostID = Message.HostID;
				TextTesting.GetComponent<Text>().text = "Host Connected";
				//Set Up Game Here
				
				MD_ReadyCall();
				break;
			//*Only Host will received* Receive that someone has setup the host and game
			case MessageType.MESSAGE_HOSTCONFIRMED:
				++NoReadied;
				
				if(NoReadied >= GetAllP().Count && Ready)
					MD_StartGame();
				break;
			//*Everyone but host should receive* 
			case MessageType.MESSAGE_STARTGAME:
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
	
	private void MD_SetUpHost()
	{
		List<Participant> Players = GetAllP();
		if(Players[0].ParticipantId == MyPID)
		{
			//You are chosen to randomise the host
			int ChosenHost = UnityEngine.Random.Range(0,4);
			
			String HostID = Players[ChosenHost].ParticipantId;
			
			byte[] Type = {(byte)MessageType.MESSAGE_SETUPHOST};
			M_GameStart Message = new M_GameStart();
			Message.HostID = HostID;
			byte[] ByteMessage = getBytes(Message);
			
			byte[] FinalMessage = CombineMessages(Type,ByteMessage);
			
			TextTesting.GetComponent<Text>().text = "I am Host";
			
			PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, FinalMessage);
		}
	}
	
	private void MD_ReadyCall()
	{
		byte[] FinalMessage = {(byte)MessageType.MESSAGE_SETUPHOST};
		PlayGamesPlatform.Instance.RealTime.SendMessage(true,HostID,FinalMessage);
	}
	
	private void MD_StartGame()
	{
	
	
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
	
	//All the Messages struct and enum will be after this
	enum MessageType
	{
		MESSAGE_SETUPHOST,
		MESSAGE_HOSTCONFIRMED,
		MESSAGE_STARTGAME
	}
	
	public struct M_GameStart
	{
		//Must be placed before every string in the struct
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
		public String HostID;
			
	}
	
	//Tp stands for template
	byte[] getBytes<TP>(TP str) {
		int size = Marshal.SizeOf(str);
		byte[] arr = new byte[size];
		
		IntPtr ptr = Marshal.AllocHGlobal(size);
		Marshal.StructureToPtr(str, ptr, true);
		Marshal.Copy(ptr, arr, 0, size);
		Marshal.FreeHGlobal(ptr);
		return arr;
	}
	
	void fromBytes<TP>(byte[] arr, ref TP outStr) where TP : new(){
		TP str = new TP();
		
		int size = Marshal.SizeOf(str);
		IntPtr ptr = Marshal.AllocHGlobal(size);
		
		Marshal.Copy(arr, 0, ptr, size);
		
		str = (TP)Marshal.PtrToStructure(ptr, str.GetType());
		Marshal.FreeHGlobal(ptr);
		
		outStr = str;
	}
}
	
	
	