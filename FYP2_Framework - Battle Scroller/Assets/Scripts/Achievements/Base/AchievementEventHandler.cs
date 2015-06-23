using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// *** Class to Handle User Events & Broadcast *** //
public class AchievementEventHandler : MonoBehaviour 
{
    //Delegate Achievement Functions
    public delegate void AchievementPopUp();
    public delegate void AchievementBonus();
    public delegate void AchievementReset();

    //Events 
    public static event AchievementPopUp ShowAchievement;
    public static event AchievementBonus ExecuteAchievement;
    public static event AchievementReset ResetAchievement;

    //Manager Class
    AchievementManager Manager;

    //Achievements (Populate via Editor)
	public List<LAchievement> Achievements = new List<LAchievement>();

    //Initialisation
    void Start()
    {
        Manager = new AchievementManager(Achievements);
    }

    //Update Func
    void Update()
    {
        //Loop through Manager's Achievement List
        for (short i = 0; i < (int)AchievementManager.GetList().Count; ++i)
        {
            //Check if key exists
			if (AchievementManager.GetList().ContainsKey((LAchievement.A_Type)i))
            {
                //Check if Achievement is unlocked
				if (AchievementManager.GetList()[(LAchievement.A_Type)i].isUnlocked())
                {
                    //Register the Achievement
					AchievementManager.Instance.RegisterAchievement((LAchievement.A_Type)i);

                    //Display Achievement Messsage
                    if (ShowAchievement != null)
                        ShowAchievement();

                    //Execute the Achievement
                    if (ExecuteAchievement != null)
                        ExecuteAchievement();

                    //Reset the Achievement
                    if (ResetAchievement != null)
                        ResetAchievement();
                }
            }
        }
    }
}