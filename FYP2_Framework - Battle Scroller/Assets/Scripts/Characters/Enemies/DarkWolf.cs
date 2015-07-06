using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DarkWolf : Enemy {
	
	Unit MainChr;
	
	enum STATES
	{	
		STATE_RUNAROUND,
		STATE_AROUNDPLAYER,
		STATE_PREPARE,
		STATE_ATTACK,
		STATE_DEATH
	}
	
	public Collider WayPointsBoundary;
	
	STATES current_state;
	float NextPetrol = 0;
	bool FaceLeft = true;
	
	Vector3 LastPosition;
	Vector3 NextPosition; 
	
	Vector3 NextAroundPosition;
	int NumberOfPoints;
	
	
	public void RandomizeStats()
	{
		Debug.Log("Wolf Inited.");
		Stats.Set(1, Random.Range(500, 700),
		          Random.Range(190, 270), Random.Range(150, 220),
		          Random.Range(190, 270), Random.Range(150, 220),
		          Random.Range(1.1f, 1.75f), "Mage", "Tsunayoshi");
	}
	
	// Use this for initialization
	public void Start () {
		//Class has been inherited
		Inherited = true;
		
		//Init from Parent Class
		this.Init();
		
		//Init Unit's Type
		this.UnitType = UType.UNIT_MAGE;
		
		//Init Stats
		this.RandomizeStats();
		
		theModel.SetTrigger("DarkWolfWalk");
		LastPosition = this.transform.position;
	}
	
	// Update is called once per frame
	public void Update () {
		ChangeState();
		StaticUpdate ();
		Action();
	}
	
	Vector3 RandomizeWayPoint()
	{
		return new Vector3(Random.Range(WayPointsBoundary.transform.position.x - WayPointsBoundary.bounds.size.x * 0.5f,
		                                WayPointsBoundary.transform.position.x + WayPointsBoundary.bounds.size.x * 0.5f),
		                   Random.Range(WayPointsBoundary.transform.position.y - WayPointsBoundary.bounds.size.y * 0.5f,
		             WayPointsBoundary.transform.position.y + WayPointsBoundary.bounds.size.y * 0.5f),
		                   0.0f);
	}
	
	Vector3 RandomizePlayerPoint()
	{
		Vector3 TempVector = new Vector3(Random.Range(MainChr.transform.position.x - 1f,MainChr.transform.position.x + 1f),
		                   Random.Range(MainChr.transform.position.y - 1f, MainChr.transform.position.y + 1f),
		                   0.0f);
		                   
		TempVector.y = Mathf.Clamp(TempVector.y, -1.5f, 1.4f);
		
  		return TempVector;
	}
	
	void ChangeState()
	{
		MainChr = Movement.Instance.theUnit;
		switch(current_state)
		{
		case STATES.STATE_RUNAROUND:
			if (Vector3.Distance(this.transform.position, MainChr.transform.position) <= 2)
			{
				current_state = STATES.STATE_AROUNDPLAYER;
				NextAroundPosition = RandomizePlayerPoint();
				NumberOfPoints = Random.Range(3,6);
			}
			break;
		case STATES.STATE_AROUNDPLAYER:
			//if Character Move out of range goes back to Idle
			if (Vector3.Distance(this.transform.position, MainChr.transform.position) > 2)
			{
				current_state = STATES.STATE_RUNAROUND; 
			}
			else if(NumberOfPoints == 0)
			{
				current_state = STATES.STATE_PREPARE; 
			}
			break;
		case STATES.STATE_PREPARE:
			break;
		case STATES.STATE_ATTACK:
			//After Attacking, switch immediatly back to move
			
			
			break;
		}
	}
	
	void Action()
	{
		LastPosition = this.transform.position;
		switch(current_state)
		{
		case STATES.STATE_RUNAROUND:
				//Randomize Way Point
			if(this.transform.position == NextPosition)
				NextPosition = RandomizeWayPoint();
				
			this.transform.position = Vector3.MoveTowards(this.transform.position, NextPosition, 4.5f * Time.deltaTime);
			break;
		case STATES.STATE_AROUNDPLAYER:
			//Move towards main character
			//Debug.Log("Moveing");
			this.transform.position = Vector3.MoveTowards(this.transform.position, NextAroundPosition, 4.5f * Time.deltaTime);
			
			if(this.transform.position ==  NextAroundPosition)
			{	
				NextAroundPosition = RandomizePlayerPoint();
				--NumberOfPoints;
			}	
			break;
		case STATES.STATE_PREPARE:
			
			break;
		case STATES.STATE_ATTACK:
			//Attack Main Character
			//Debug.Log("Attacking");
			break;
		}
		
		FaceLeft = LastPosition.x > this.transform.position.x;
		
		if(FaceLeft)
			this.transform.localScale = new Vector3(-1,1,1);
		else
			this.transform.localScale = new Vector3(1,1,1);
	}
}