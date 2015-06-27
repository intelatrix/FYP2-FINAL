using UnityEngine;
using System.Collections;

public class Button_Transition : Button
{
    // *** Inherited Virtual Functions *** //
    public override void ExecuteFunction()
    {
        SoundManager.Instance.PlayEffect(SoundManager.Effects.EFFECT_CLICK);
        Fading.Instance.ResetFade(false);
        Selected = true;
    }
    bool Selected = false;

    //Enum for Switchin' Music (#ShawnMichael)
	public SoundManager.BGM SWAP_UPON_TRANSITION = SoundManager.BGM.BGM_NONE;

    //Scene's Name
    public string SceneName;

    //Use this for initialization
    void Start()
    {
        //Set Type
        this.ButtonType = BType.BUTTON_TRANSITION;
    }

    //Update is called once per frame
    void Update()
    {
        //Update from Parent
        this.StaticUpdate();

        //Transit Level
        if (Fading.Instance.isFaded && Selected)
        {
            //Swap BGM before Transiting
			if (SWAP_UPON_TRANSITION != SoundManager.BGM.BGM_NONE)
            {
               	SoundManager.Instance.PlayBGM(SWAP_UPON_TRANSITION);
            }

            //Load the Level
            Application.LoadLevel(SceneName);
        }
    }
}
