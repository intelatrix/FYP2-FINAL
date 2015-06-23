using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour 
{
    //Singleton Structure
    protected static AudioManager mInstance;
    public static AudioManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject tempObj = new GameObject();
                mInstance = tempObj.AddComponent<AudioManager>();
                Destroy(tempObj);
            }
            return mInstance;
        }
    }

    //List of Audio Sources
    public List<AudioFile> AudioList = new List<AudioFile>(); //Populated via Editor
    Dictionary<AudioFile.A_TYPE, AudioFile> AudioDict = new Dictionary<AudioFile.A_TYPE, AudioFile>();
    List<AudioFile.A_TYPE> BGMList = new List<AudioFile.A_TYPE>();

    //Initialisation
    void Start()
    {
        //Set Instance
        if (mInstance != null) 
        {
            Destroy(this.gameObject);
            return;
        }
        mInstance = this;

        //Determine which enum is a BGM
        BGMList.Add(AudioFile.A_TYPE.AUDIO_BGM);
        BGMList.Add(AudioFile.A_TYPE.AUDIO_INGAME);

        //Populate Dictionary
        for (short i = 0; i < AudioList.Count; ++i)
            AudioDict.Add(AudioList[i].Type, AudioList[i]);

        //Make Sure List is Filled
        for (short i = 0; i < (short)AudioFile.A_TYPE.AUDIO_TOTAL; ++i)
        {
            if (!AudioDict.ContainsKey((AudioFile.A_TYPE)i))
                AudioDict.Add((AudioFile.A_TYPE)i, new AudioFile((AudioFile.A_TYPE)i));
        }
    }

    //Play Music in List
    public void Play(AudioFile.A_TYPE Type)
    {
        AudioDict[Type].Play();
    }

    //Stop Music in List
    public void Stop(AudioFile.A_TYPE Type)
    {
        AudioDict[Type].Stop();
    }

    //Stop Playing All Music
    public void Mute()
    {
        for (short i = 0; i < (short)AudioFile.A_TYPE.AUDIO_TOTAL; ++i)
            AudioDict[(AudioFile.A_TYPE)i].Stop();
    }

    //Stop Playing All BGMs
    public void MuteBGM()
    {
        //Check for BGM
        for (short i = 0; i < BGMList.Count; ++i)
            AudioDict[BGMList[i]].Stop();
    }

    //Awake Func
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
