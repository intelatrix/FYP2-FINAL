using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// *** Class to Manage Achievements *** //
public class AchievementManager : MonoBehaviour 
{
    //Singleton Structure
    protected static AchievementManager mInstance;
    public static AchievementManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject tempObj = new GameObject();
                mInstance = tempObj.AddComponent<AchievementManager>();
                Destroy(tempObj);
            }
            return mInstance;
        }

    }
    //List to keep track of Number of Achievements for each Type
	static Dictionary<LAchievement.A_Type, int> AchievementCountTracker = new Dictionary<LAchievement.A_Type, int>();

    //List of Achievements
	static Dictionary<LAchievement.A_Type, LAchievement> AchievementsList = new Dictionary<LAchievement.A_Type, LAchievement>();

    //Returns Achievement List
	public static Dictionary<LAchievement.A_Type, LAchievement> GetList()
    {
        return AchievementsList;
    }

    //Initialisation
	public AchievementManager(List<LAchievement> AchvList)
    {
        //Set Instance
        if (mInstance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        mInstance = this;

        //Populate Tracker Dictionary
		for (short i = 0; i < (short)LAchievement.A_Type.ACHV_COUNT; ++i)
			AchievementCountTracker.Add((LAchievement.A_Type)i, 0);

        //Populate Achievement Dictionary
        for (short i = 0; i < AchvList.Count; ++i)
            AchievementsList.Add(AchvList[i].Type, AchvList[i]);
    }

    //Awake Func
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    //Register an Achievement 
	public void RegisterAchievement(LAchievement.A_Type Type)
    {
        //Check if Type exists in List
        if (!AchievementCountTracker.ContainsKey(Type) ||
            !AchievementsList.ContainsKey(Type))
            return;

        //Increment Count
        ++AchievementCountTracker[Type];
    }
}
