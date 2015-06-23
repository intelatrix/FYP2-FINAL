using UnityEngine;
using System.Collections;

public class AudioFile : MonoBehaviour 
{
    //Audio's Type
    public enum A_TYPE
    {
        AUDIO_BGM,
        AUDIO_INGAME,
        AUDIO_CLICK,
        AUDIO_FIRE,
        AUDIO_TOTAL
    } public A_TYPE Type;

    //Play Flag
    bool m_bPlaying;

    //Initialisation
    void Start()
    {
        //Set Play Flag
        if (this.GetComponent<AudioSource>() != null)
            m_bPlaying = this.GetComponent<AudioSource>().isPlaying;
    }

    //Constructor (Overloaded)
    public AudioFile(A_TYPE Type)
    {
        Start();
        this.Type = Type;
    }

    //Play
    public void Play(bool PlayOnce = false)
    {
        if (PlayOnce && m_bPlaying) return;

        if (this.GetComponent<AudioSource>() != null)
        {
            this.GetComponent<AudioSource>().Play();
            m_bPlaying = true;
        }
    }

    //Stop
    public void Stop()
    {
        if (this.GetComponent<AudioSource>() != null)
        {
            this.GetComponent<AudioSource>().Stop();
            m_bPlaying = false;
        }
    }

    //Set Volume
    public void SetVolume(float f_Vol)
    {
        if (this.GetComponent<AudioSource>() != null &&
            this.GetComponent<AudioSource>().isPlaying)
            this.GetComponent<AudioSource>().volume = f_Vol;
    }

    //Update Func
    void Update()
    {
        this.SetVolume(VolumeBar.f_Vol);
    }
}
