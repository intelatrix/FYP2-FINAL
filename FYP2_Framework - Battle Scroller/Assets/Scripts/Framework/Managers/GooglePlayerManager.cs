using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayerManager : MonoBehaviour {

	System.Action<bool> mAuthCallback;
	private static GooglePlayerManager mInstance;
	//This is the public reference that other classes will use
	public static GooglePlayerManager Instance
	{
		get
		{	
			//if mInstance is null, there's something wrong. Thus, there's should be no error checking
			return mInstance;
		}
	}
	
	//Setting Up the Singleton
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
	
	void Start()
	{
			mAuthCallback = (bool success) => 
		{
			if (success)
			{
				Debug.Log("Authenticated");
			}
			else 
			{
				Debug.Log("Authentication failed");
			}
		};
	
		PlayGamesPlatform.Activate();
		PlayGamesPlatform.Instance.Authenticate(mAuthCallback);
		
		Social.localUser.Authenticate((bool success) =>{
			// handle success or failure
			if(success)
			{
//				//Load MainMenuScene
//				Social.ReportProgress("CgkIkbPhxrsDEAIQAA", 100.0f, (bool Achievement) => {
//					// handle success or failure
//				});
			}
			else
			{
				//CLOSE GAME(MAYBE)
			}
		});
	}
	
	public void PostSurvivalScore(float TimeLasted)
	{
		long Score = (long)TimeLasted*1000;
		Social.ReportScore(Score, "CgkIkbPhxrsDEAIQBg", (bool success) => {
			// handle success or failure
		});
	}
	
	public void ShowLeaderBoard()
	{
		Social.ShowLeaderboardUI();
	}
	
	public void ShowAchievement ()
	{
		Social.ShowAchievementsUI();
	}	
}
