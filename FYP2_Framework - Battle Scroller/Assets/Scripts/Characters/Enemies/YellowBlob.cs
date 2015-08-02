using UnityEngine;
using System.Collections;

public class YellowBlob : Enemy {
	
	Unit MainChr;
	public Collider SurvivalPlayArea = null;
	
	enum AnimationType
	{
		ANI_IDLE,
		ANI_MOVE,
		ANI_ATTACK,
		ANI_TOTAL
	}
	
	enum STATES
	{	
		STATE_IDLE = 0,
		STATE_PETROL,
		STATE_MOVE,
		STATE_ATTACKING,
		STATE_ATTACK,
		STATE_DEATH,
		STATE_TOTAL
	}
	
	enum TYPE 
	{
		SLIME_NORMAL,
		SLIME_FIRE
	}
	
	STATES current_state;
	float NextPetrol = 0;
	bool PetrolLeft = false;
	public CollisionRegion AttackRegion;
	bool DealDamage = false;
	
	bool FaceLeft = true;
	Vector3 LastPosition;
	
	public void RandomizeStats()
	{
		Debug.Log("RedBlob");
		Stats.Set(1, 30,
		          10, 0,
		          0,0,
		          0, "YellowBlob", "YellowSlime");
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
		
		current_state = STATES.STATE_IDLE;
		MainChr = Movement.Instance.theUnit;
		
		theModel.SetTrigger("YellowBlobIdle");
		LastPosition = this.transform.position;
	}
	
	// Update is called once per frame
	public void Update () {
		ChangeState();
		StaticUpdate ();
		Action();
	}
	
	void ChangeState()
	{
		MainChr = Movement.Instance.theUnit;
		switch(current_state)
		{
		case STATES.STATE_IDLE:
			//if Character is close enough, chase Character
			//else if Timer is down, goes into petrol mode
			//Debug.Log(Vector3.Distance(this.transform.position, MainChr.transform.position).ToString());
			if (Vector3.Distance(this.transform.position, MainChr.transform.position) <= 2)
			{
				current_state = STATES.STATE_MOVE;
				theModel.SetTrigger("YellowBlobMove");
			}
			else if (NextPetrol <= 0)
			{
				theModel.SetTrigger("YellowBlobMove");
				current_state = STATES.STATE_PETROL;
				NextPetrol = Random.Range(1f, 2.5f);
				PetrolLeft = !PetrolLeft;
			}
			break;
		case STATES.STATE_PETROL:
			//if Character is close enough, chase Character
			//else if Timer is down, goes into idle mode
			if (Vector3.Distance(this.transform.position, MainChr.transform.position) <= 3)
			{
				current_state = STATES.STATE_MOVE;
				theModel.SetTrigger("YellowBlobMove");
			}
			else if(NextPetrol <= 0)
			{
				theModel.SetTrigger("YellowBlobIdle");
				current_state = STATES.STATE_IDLE;
				NextPetrol = Random.Range(1f, 2f);
			}
			break;
		case STATES.STATE_MOVE:
			//if Character Move out of range goes back to Idle
			if (Vector3.Distance(this.transform.position, MainChr.transform.position) > 3)
			{
				theModel.SetTrigger("YellowBlobIdle");
				current_state = STATES.STATE_IDLE; 
			}
			else if (Vector3.Distance(this.transform.position, MainChr.transform.position) <= 0.25)
			{
				theModel.SetTrigger("YellowBlobAttacking");
				current_state = STATES.STATE_ATTACKING;
			}
			break;
		case STATES.STATE_ATTACKING:
			//After Attacking, switch to attack to calculate damage
			if(theModel.GetAnimation().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
			{
				current_state = STATES.STATE_ATTACK;
				theModel.SetTrigger("YellowBlobAttack");
				DealDamage = false;
			}
			break;
		case STATES.STATE_ATTACK:
			//After Attacking, switch to attack to calculate damage
			if(theModel.GetAnimation().GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
			{
				current_state = STATES.STATE_IDLE;
				theModel.SetTrigger("YellowBlobIdle");
			}
			break;
		}
	}
	
	void Action()
	{
		LastPosition = this.transform.position;
		switch(current_state)
		{
		case STATES.STATE_IDLE:
			//NOTHING. IDLING FUCK
			NextPetrol -= Time.deltaTime;
			//Debug.Log("Idling");
			break;
		case STATES.STATE_PETROL:
			if(PetrolLeft)
			{
				//MoveLeft
				this.transform.Translate(new Vector3(-3.0f,0,0)*Time.deltaTime);
			}
			else
			{
				//MoveRight
				this.transform.Translate(new Vector3(3.0f,0,0)*Time.deltaTime);
			}
			NextPetrol -= Time.deltaTime;
			//Debug.Log("Petrolling");
			break;
		case STATES.STATE_MOVE:
			//Move towards main character
			this.transform.Translate((MainChr.gameObject.transform.position - this.transform.position).normalized * Time.deltaTime*4);
			//Debug.Log("Moveing");
			break;
		case STATES.STATE_ATTACKING:
			//Attack Main Character
			//Debug.Log("Attacking");
			break;
		case STATES.STATE_ATTACK:
			//Attack Main Character
			//Debug.Log("Attacking");
			if(AttackRegion.inRegion && !DealDamage)
			{
				Movement.Instance.theUnit.Stats.TakePhysicalDamage(this.Stats, 4.0f);
				DealDamage = true;
			}	
			break;
		}
		
		OutOfBound();
		FaceLeft = LastPosition.x > this.transform.position.x;
		
		if(FaceLeft)
			this.transform.localScale = new Vector3(-1,1,1);
		else
			this.transform.localScale = new Vector3(1,1,1);
	}
	
	void OutOfBound()
	{
		if(SurvivalPlayArea != null)
		{
			if(!SurvivalPlayArea.bounds.Contains(transform.position))
				transform.position = LastPosition;
		}
	}
}
