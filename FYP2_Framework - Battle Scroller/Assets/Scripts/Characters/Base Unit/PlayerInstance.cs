using UnityEngine;
using System.Collections;

public class PlayerInstance : Unit 
{
    //Singleton Structure
    protected static PlayerInstance mInstance;
    public static PlayerInstance Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject tempObj = new GameObject();
                mInstance = tempObj.AddComponent<PlayerInstance>();
                Destroy(tempObj);
            }
            return mInstance;
        }
    }

    // *** Inherited Virtual Functions *** //
    public void RandomizeStats()
    {
        Debug.Log("Player Stats Inited.");
        Stats.Set(1, Random.Range(500, 700),
                  Random.Range(1200, 1500), Random.Range(150, 220),
                  Random.Range(190, 270), Random.Range(150, 220),
                  Random.Range(1.1f, 1.75f), "Player", "Slife");
    }

    void Start()
    {
        //Set Instance
        if (mInstance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        mInstance = this;

        //Class has been inherited
        Inherited = true;

        //Init from Parent Class
        this.Init();

        //Init Unit's Type
        this.UnitType = UType.UNIT_PLAYER;

        //Init Stats
        this.RandomizeStats();
    }

    void Update()
    {
        StaticUpdate();
    }
}
