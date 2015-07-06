using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HpBar : MonoBehaviour 
{
    public Unit theUnit;
    float InitialScale;
    public Text HpText;

	// Use this for initialization
	void Start () 
    {
        //Store Initial Scale
        InitialScale = this.transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Set Cur Scale
        float CurScaleX = this.transform.localScale.x;

        //Set New Scale
        CurScaleX = InitialScale * (theUnit.Stats.HP / theUnit.Stats.MAX_HP);

        //Modify Scale
        this.transform.localScale = new Vector3(CurScaleX, this.transform.localScale.y, this.transform.localScale.z);

        //Set Text
        if (HpText != null)
        {
            float HpNo = (int)(100.0f * (theUnit.Stats.HP / theUnit.Stats.MAX_HP));
            HpText.text = HpNo + "%";
        }
	}
}
