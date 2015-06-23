﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeBar : MonoBehaviour 
{
    //Variables
    public Slider theSlider;
    public Text theText;
    static int Volume = -1;
    public static float f_Vol = 1.0f;

    //Initialisation
    void Start()
    {
        //Set Volume
        if (Volume == -1)
            Volume = (int)(theSlider.value * 100);
        else
            theSlider.value = (float)Volume*0.01f;
    }
	
	//Update is called once per frame
	void Update () 
    {
        //Set Volume
        Volume = (int)Mathf.Ceil(theSlider.value * 100);
        if (theText != null)
            theText.text = "VOLUME (" + Volume + "%)";
        f_Vol = theSlider.value;
	}
}
