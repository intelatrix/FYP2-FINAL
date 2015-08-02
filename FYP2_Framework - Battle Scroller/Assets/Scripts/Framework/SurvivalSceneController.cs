using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SurvivalSceneController : MonoBehaviour {

	public SurvivalSystem SurvivalGamePlayManager;
	public GameObject GameOverOverlay;
	public Text CountDownText;
	public Text Score;
	private float CountDownTime;
	bool GameStart = false;
	
	// Use this for initialization
	void Start () 
	{
		Global.SurvivalCountDown = true;
		CountDownTime = 3;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if(GameStart == false)
		{
			CountDownTime -= Time.deltaTime;
			CountDownText.text = CountDownTime.ToString("0");
			if(CountDownTime <= 0)
			{
				Global.SurvivalCountDown = false;
				GameStart = true;
				Destroy(CountDownText.gameObject);
				
				SurvivalGamePlayManager.enabled = true;
			}
		}
		else
		{
		}
		
		if(Global.SurivalGameLost)
		{
			GameOverOverlay.SetActive(true);
			float TimeSoFar = SurvivalGamePlayManager.GetScore();
			int seconds = (int)TimeSoFar%60;
			int minute = (int)TimeSoFar/60;
			Score.text = minute.ToString("00") + ":" + seconds.ToString("00");
		}
	}
}
