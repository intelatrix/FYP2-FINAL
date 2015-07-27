using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {

	// Use this for initialization
	
	//Here is a private reference only this class can access
	private static AdManager mInstance;
	BannerView bannerView;
	
	//This is the public reference that other classes will use
	public static AdManager Instance
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
	
	
	void Start () {
		bannerView = new BannerView("UA-61893481-3", AdSize.Banner, AdPosition.Bottom);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		bannerView.LoadAd(request);
	
	}
	
	void ShowAd()
	{
		bannerView.Show();
	}
	
	void HideAd()
	{
		bannerView.Hide();
	}
}
