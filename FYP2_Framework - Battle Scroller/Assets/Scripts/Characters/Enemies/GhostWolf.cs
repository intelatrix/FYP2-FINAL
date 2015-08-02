using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostWolf : Enemy {
	
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
	public CollisionRegion AttackRegion;
	public SpriteRenderer GhostWolfRenderer;
	public SpriteRenderer HealthBarRenderer;
	
	public Collider SurvivalPlayArea = null;
	
	STATES current_state;
	float NextPetrol = 0;
	bool FaceLeft = true;
	
	Vector3 LastPosition;
	Vector3 NextPosition; 
	
	Vector3 NextAroundPosition;
	int NumberOfPoints;
	public GameObject WolfAnimator;
	bool DealDamage = false;
	float HealthLast;
	float ShowUpCountDown = 0;
	
	public GameObject DestroyWhenKill;
	
	public void RandomizeStats()
	{
		Debug.Log("GhostWolf");
		Stats.Set(1, 50,
		          7, 0,
		          0,0,
		          0, "GhostWolf", "GhostWolf");
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
		
		theModel.SetTrigger("GhostWolfWalk");
		LastPosition = this.transform.position;
		
		current_state = STATES.STATE_RUNAROUND;
		NextPosition = RandomizeWayPoint();
		GhostWolfRenderer.color = new Color(1f,1f,1f,0.4f);
		HealthBarRenderer.color = new Color(1f,1f,1f,0.1f);
		
		HealthLast = Stats.HP;
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
		Vector3 TempVector = new Vector3(Random.Range(MainChr.transform.position.x - 1.2f,MainChr.transform.position.x + 1.2f),
		                                 Random.Range(MainChr.transform.position.y - 1.5f, MainChr.transform.position.y + 1.5f),
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
			if (Vector3.Distance(this.transform.position, MainChr.transform.position) <= 2f)
			{
				current_state = STATES.STATE_AROUNDPLAYER;
				NextAroundPosition = RandomizePlayerPoint();
				NumberOfPoints = Random.Range(1,3);
			}
			break;
		case STATES.STATE_AROUNDPLAYER:
			//if Character Move out of range goes back to Idle
			if (Vector3.Distance(this.transform.position, MainChr.transform.position) > 2f)
			{
				current_state = STATES.STATE_RUNAROUND; 
			}
			else if(NumberOfPoints == 0)
			{
				current_state = STATES.STATE_PREPARE; 
				bool LeftSide = Random.Range(0,2) == 1;
				if(LeftSide)
					NextAroundPosition = new Vector3(-1.5f,0,0);
				else
					NextAroundPosition = new Vector3(1.5f,0,0);
			}
			break;
		case STATES.STATE_PREPARE:
			if (Vector3.Distance(this.transform.position, MainChr.transform.position) > 2f)
			{
				current_state = STATES.STATE_RUNAROUND; 
			}	
			else if(Vector3.Distance(this.transform.position,(MainChr.transform.position+NextAroundPosition)) <= 0.5f)
			{
				current_state = STATES.STATE_ATTACK;
				if( this.transform.position.x  > MainChr.transform.position.x)
				{
					//Left
					this.transform.localScale = new Vector3(-1,1,1);
					
				}
				else
				{
					//Right 
					this.transform.localScale = new Vector3(1,1,1);
				}		
				DealDamage = false;
				theModel.SetTrigger("GhostWolfAttack");
			}
			break;
		case STATES.STATE_ATTACK:
			//After Attacking, switch immediatly back to move
			if(theModel.GetAnimation().GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
			{
				if (Vector3.Distance(this.transform.position, MainChr.transform.position) <= 2f)
				{
					current_state = STATES.STATE_AROUNDPLAYER;
					NextAroundPosition = RandomizePlayerPoint();
					NumberOfPoints = Random.Range(1,3);
				}
				else
				{
					current_state = STATES.STATE_RUNAROUND;
				}
				if(this.transform.localScale ==  new Vector3(-1,1,1))
					this.transform.position -= WolfAnimator.transform.localPosition;
				else 
					this.transform.position += WolfAnimator.transform.localPosition;
				
				WolfAnimator.transform.localPosition = Vector3.zero;
				theModel.SetTrigger("GhostWolfWalk");
			}
			
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
			
			this.transform.position = Vector3.MoveTowards(this.transform.position, NextPosition, 5f * Time.deltaTime);
			break;
		case STATES.STATE_AROUNDPLAYER:
			//Move towards main character
			//Debug.Log("Moveing");
			this.transform.position = Vector3.MoveTowards(this.transform.position, NextAroundPosition, 7.25f * Time.deltaTime);
			
			if(this.transform.position ==  NextAroundPosition)
			{	
				NextAroundPosition = RandomizePlayerPoint();
				--NumberOfPoints;
			}	
			break;
		case STATES.STATE_PREPARE:
			this.transform.position = Vector3.MoveTowards(this.transform.position, MainChr.transform.position+NextAroundPosition, 7.25f * Time.deltaTime);
			break;
		case STATES.STATE_ATTACK:
			//Attack Main Character
			//Debug.Log("Attacking");
			if(AttackRegion.inRegion && !DealDamage)
			{
				Movement.Instance.theUnit.Stats.TakePhysicalDamage(this.Stats, 5.0f);
				DealDamage = true;
			}	
			break;
		}
		
		if(LastPosition.x > this.transform.position.x)
			this.transform.localScale = new Vector3(-1,1,1);
		else if(LastPosition.x < this.transform.position.x)
			this.transform.localScale = new Vector3(1,1,1);
		
		SetInvisibility();
	}
	
	public override void KillUnit()
	{
		if(Application.loadedLevelName != "1_00_SurvivalGameScene")
			Destroy(DestroyWhenKill);
	}
	
	void OutOfBound()
	{
		if(SurvivalPlayArea != null)
		{
			if(!SurvivalPlayArea.bounds.Contains(transform.position))
				transform.position = LastPosition;
		}
	}
	
	public void SetSurvival(GameObject MonsterParent)
	{
		this.transform.parent = MonsterParent.transform;
		WayPointsBoundary = SurvivalPlayArea;
		Destroy(DestroyWhenKill);
	}
	
	public void SetInvisibility()
	{
		if(ShowUpCountDown > 0)
		{
			ShowUpCountDown -= Time.deltaTime;
		}
		
		if(!Mathf.Approximately(HealthLast, Stats.HP))
		{
			ShowUpCountDown = 1.75f;
			GhostWolfRenderer.color = new Color(1f,1f,1f,1f);
			HealthBarRenderer.color = new Color(1f,1f,1f,1f);
		}	
		
		if(ShowUpCountDown <= 0)
		{
			GhostWolfRenderer.color = new Color(1f,1f,1f,Mathf.MoveTowards(GhostWolfRenderer.color.a, 0.4f ,Time.deltaTime));
			HealthBarRenderer.color = new Color(1f,1f,1f,Mathf.MoveTowards(HealthBarRenderer.color.a, 0.1f ,Time.deltaTime));
		}
		
		HealthLast = Stats.HP;
	}
}