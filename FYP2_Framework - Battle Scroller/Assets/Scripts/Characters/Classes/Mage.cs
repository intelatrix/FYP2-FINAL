using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mage : Unit 
{
    // *** Inherited Virtual Functions *** //
    public void RandomizeStats()
    {
        Debug.Log("Mage Stats Inited.");
        Stats.Set(1, Random.Range(500, 700),
                  Random.Range(1200, 1500), Random.Range(150, 220),
                  Random.Range(190, 270), Random.Range(150, 220),
                  Random.Range(1.1f, 1.75f), "Mage", "Tsunayoshi");
    }

    //Enemy FSM
    public enum E_State
    {
        STATE_IDLE,
        STATE_MOVE,
        STATE_ATTACK,
        STATE_FALL,
        STATE_TOTAL
    } public E_State theState = E_State.STATE_MOVE;

    //Idle Timer
    Timer.TimeBundle IdleTimer;

    //Damage Timer
    Timer.TimeBundle DamageTimer;
    bool isLeft_Attack = false;

    public Collider WayPointsBoundary;
    short CurIndex = 0;
    public List<Vector3> WayPoints_List = new List<Vector3>();
    bool isLeft_Movement = false;
    void Move()
    {
        //Random Way Points System
        float BufferZone = 5.0f, MovementSpeed = 4.0f;
        if ((this.transform.position - WayPoints_List[CurIndex]).sqrMagnitude > BufferZone)
            this.transform.position = Vector2.MoveTowards(this.transform.position, WayPoints_List[CurIndex], MovementSpeed * Time.deltaTime);
        else
        {
            WayPoints_List.Add(RandomizeWayPoint());
            ++CurIndex;
        }        
    }

    Vector3 RandomizeWayPoint()
    {
        return new Vector3(Random.Range(WayPointsBoundary.transform.position.x - WayPointsBoundary.bounds.size.x * 0.5f,
                                        WayPointsBoundary.transform.position.x + WayPointsBoundary.bounds.size.x * 0.5f),
                           Random.Range(WayPointsBoundary.transform.position.y - WayPointsBoundary.bounds.size.y * 0.5f,
                                        WayPointsBoundary.transform.position.y + WayPointsBoundary.bounds.size.y * 0.5f),
                           0.0f);
    }

    void ExecuteState()
    {
        switch (this.theState)
        {
            case E_State.STATE_IDLE:

                if (this.State == EState.FALL)
                {
                    this.theState = E_State.STATE_FALL;
                    break;
                }

                theModel.SetAnimation(0);
                if (Timer.ExecuteTime(IdleTimer.Time, IdleTimer.Index))
                    theState = E_State.STATE_MOVE;
                break;
            case E_State.STATE_FALL:
                if (this.State != EState.FALL)
                    theState = E_State.STATE_IDLE;
                break;
            case E_State.STATE_MOVE:

                if (this.State == EState.FALL)
                {
                    this.theState = E_State.STATE_FALL;
                    break;
                }

                Move();
                this.theModel.SetAnimation(1);

                //Set Face Dir
                isLeft_Movement = (this.transform.position.x - WayPoints_List[CurIndex].x < 0);

                //Flip
                if (!isLeft_Movement && theModel.transform.localScale.x < 0)
                {
                    Vector3 tempScale = theModel.transform.localScale;
                    tempScale.x *= -1;
                    theModel.transform.localScale = tempScale; //Flip Player's Model
                }
                if (isLeft_Movement && theModel.transform.localScale.x > 0)
                {
                    Vector3 tempScale = theModel.transform.localScale;
                    tempScale.x *= -1;
                    theModel.transform.localScale = tempScale; //Flip Player's Model
                }

                if ((this.transform.position - Movement.Instance.theUnit.transform.position).sqrMagnitude < 5.0f)
                    theState = E_State.STATE_ATTACK;
                break;
            case E_State.STATE_ATTACK:

                if (this.State == EState.FALL)
                {
                    this.theState = E_State.STATE_FALL;
                    break;
                }

                if ((this.transform.position - Movement.Instance.theUnit.transform.position).sqrMagnitude >= 5.0f)
                    theState = E_State.STATE_MOVE;

                this.transform.position = Vector2.MoveTowards(this.transform.position,
                                                              Movement.Instance.theUnit.transform.position,
                                                              3.0f * Time.deltaTime);
                theModel.SetAnimation(2);

                //Set Face Dir
                isLeft_Attack = (this.transform.position.x - Movement.Instance.theUnit.transform.position.x < 0);

                //Flip
                if (!isLeft_Attack && theModel.transform.localScale.x < 0)
                {
                    Vector3 tempScale = theModel.transform.localScale;
                    tempScale.x *= -1;
                    theModel.transform.localScale = tempScale; //Flip Player's Model
                }
                if (isLeft_Attack && theModel.transform.localScale.x > 0)
                {
                    Vector3 tempScale = theModel.transform.localScale;
                    tempScale.x *= -1;
                    theModel.transform.localScale = tempScale; //Flip Player's Model
                }

                if ((this.transform.position - Movement.Instance.theUnit.transform.position).sqrMagnitude <= 1.0f)
                {
                    if (Timer.ExecuteTime(DamageTimer.Time, DamageTimer.Index))
                        Movement.Instance.theUnit.Stats.TakePhysicalDamage(this.Stats, 1.0f);
                }
                break;
            default:
                break;
        }
    }

    //Use this for initialization
    void Start()
    {
        //Class has been inherited
        Inherited = true;

        //Init from Parent Class
        this.Init();

        //Init Unit's Type
        this.UnitType = UType.UNIT_MAGE;

        //Init Stats
        this.RandomizeStats();

        //Create 1st Random Way Point
        WayPoints_List.Add(RandomizeWayPoint());

        //Set Timer
        IdleTimer.Time = 0.3f;
        IdleTimer.Index = Timer.GetExecuteID(IdleTimer.Time);
        DamageTimer.Time = 0.9f;
        DamageTimer.Index = Timer.GetExecuteID(DamageTimer.Time);
    }

    //Update is called once per frame
    void Update()
    {
        //Update from Parent Class
        this.StaticUpdate();

        //Execute FSM
        ExecuteState();
    }
}
