using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
	public enum BGM
	{
		BGM_NONE,
		BGM_MAIN,
		BGM_INGAME
	}	
	
	public enum Effects
	{
		EFFECT_NONE,
		EFFECT_CLICK,
		EFFECT_PUNCH,
		EFFECT_KICK
	}
	
	public List<AudioFile> AudioList = new List<AudioFile>();
	public Dictionary<BGM, AudioFile> BGMDict = new Dictionary<BGM, AudioFile>();
	public Dictionary<Effects, AudioFile> EffectsDict = new Dictionary<Effects, AudioFile>();
	
	//Current playing BGM
	AudioFile CurrentBGM;
	
	//Here is a private reference only this class can access
	private static SoundManager mInstance;
	
	//This is the public reference that other classes will use
	public static SoundManager Instance
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
	
	//After Awake, the codes enter Start(). All gameobject should be initialise by now. 
	//Loading of the sounds 
	void Start()
	{
		foreach(AudioFile Audio in AudioList)
		{
			switch(Audio.AudioType)
			{
				case AudioFile.A_TYPE.AUDIO_BGM:
					BGMDict.Add(Audio.ThisBGM, Audio);
					break;
				
				case AudioFile.A_TYPE.AUDIO_EFFECTS:
					EffectsDict.Add(Audio.ThisEffect, Audio);
					break;
			}
		}
	}
		
	public void PlayBGM(BGM BGMType)
	{
		if(CurrentBGM != null)
		{
			//Allow only one BGM to play at a time
			CurrentBGM.Stop();
		}
		BGMDict[BGMType].Play();
		CurrentBGM = BGMDict[BGMType];
	}
	
	public void PlayEffect(Effects EffectType)
	{
		EffectsDict[EffectType].Play();
	}
	
	//Stop Playing All Music
	public void StopAllSound()
	{
		foreach (AudioFile Audio in AudioList)
		{
			Audio.Stop();
		}
	}
	
	public void ChangeVolume()
	{	
		foreach (AudioFile Audio in AudioList)
		{
			Audio.SetVolume(VolumeBar.f_Vol);
		}
	}
	
	public void MuteBGM()
	{
		foreach (AudioFile Audio in BGMDict.Values)
		{
			Audio.SetVolume(0);
		}
	}
	
	public void StopBGM()
	{
		if(CurrentBGM != null)
		{
			CurrentBGM.Stop();
		}
	}
}

