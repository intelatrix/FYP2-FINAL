using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Attack_Kick_Combo : AttackScript 
{
	//Use this for initialization
	void Start () 
    {
        //Set Combo Flag
        this.isCombo = true;

        //Init from Parent
        this.Init();

        //Set Type
        this.AttackType = AType.ATTACK_KICK_COMBO;

        //Set Keys (Combo Chains)
        ListOfKeys.Add(KeyCode.X);
        ListOfKeys.Add(KeyCode.X);
        ListOfKeys.Add(KeyCode.Z);

        //Set Damage
        this.Damage = 50.0f;

        //Set Anim Index
        this.AnimationIndex = 2; //6

        //Set Anim Time
        this.AnimationTimer.Time = 0.4f;
	}
	
	//Update is called once per frame
	void Update () 
    {
	    //Update from Parent
        this.StaticUpdate();

        // -- BBA Executed;
        if (this.executeCombo && Tutorial.isTut() && CombatManager.AttackCount >= 4)
            CombatManager.BBAExecuted = true;
	}
}
