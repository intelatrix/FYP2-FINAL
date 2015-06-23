using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{

	protected SoundManager()
	{
		//Load Sound Here	
	}
	
	
	public enum Sounds
	{
		MAIN_BGM
	}	
	//Here is a private reference only this class can access
	private static SoundManager mInstance;
	
	//This is the public reference that other classes will use
	public static SoundManager Instance
	{
		get
		{
			//If _instance hasn't been set yet, we grab it from the scene!
			//This will only happen the first time this reference is used.
			if(mInstance == null)
				mInstance =  new SoundManager();
			return mInstance;
		}
	}
		
	public void PlaySound(Sounds SoundType)
	{
		switch (SoundType)
		{
			case Sounds.MAIN_BGM:
			break;
		}
	
	}
}

