using UnityEngine;
using System.Collections;

public class AudioFile : MonoBehaviour 
{
    //Audio's Type
    public enum A_TYPE
    {
        AUDIO_BGM,
        AUDIO_EFFECTS
    }
	
    AudioSource ThisSound;
    
    public A_TYPE AudioType;
	public SoundManager.BGM ThisBGM;
	public SoundManager.Effects ThisEffect;

    //Initialisation
    void Start()
    {
		ThisSound = GetComponent<AudioSource>();
    }
    
    //Play
    public void Play()
    {
		if (ThisSound != null)
		{
			ThisSound.Play();
		}
    }

    //Stop
    public void Stop()
    {
		if (ThisSound != null)
        {
			ThisSound.Stop();
        }
    }

    //Set Volume
    public void SetVolume(float f_Vol)
    {
		if ( ThisSound != null)
			ThisSound.volume = f_Vol;
    }

    //Update Func
    void Update()
    {
    }
}
