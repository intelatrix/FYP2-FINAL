using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SurvivalSystem : MonoBehaviour {

	public List<GameObject> ListOfAliveUnit = new List<GameObject> ();
	public Collider PlayArea;
	
	public GameObject NormalSlimePrefab;
	public GameObject RedSlimePrefab;
	public GameObject YellowSlimePrefab;
	public GameObject DarkWolfPfrefab;
	public GameObject GhostWolfPrefab;
	
	public GameObject MonstersParent;
	
	public Text TimePassed;
	public Text MonsterNo;
	public Text TimeTill;
	
	enum MonsterType
	{
		MONSTER_NSLIME,
		MONSTER_RSLIME,
		MONSTER_YSLIME,
		MONSTER_DWOLF,
		MONSTER_GWOLF
	
	}
	
	Dictionary<MonsterType, bool> ToSpawnOrNot= new Dictionary<MonsterType, bool >()
	{
		{MonsterType.MONSTER_NSLIME, false},
		{MonsterType.MONSTER_RSLIME, false},
		{MonsterType.MONSTER_YSLIME, false},
		{MonsterType.MONSTER_DWOLF, false},
		{MonsterType.MONSTER_GWOLF, false}
		
	};
	
	float TillNextMonster = 0;
	float TimeSoFar = 0;
	float LastTime = 0;
	// Use this for initialization
	void Start () 
	{
		ToSpawnOrNot[MonsterType.MONSTER_NSLIME] = true;
		SpawnFirstUnit();
		
		TillNextMonster = 3f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		RemoveDeadMonstersFromList();
		if(ListOfAliveUnit.Count >= 25)
			Global.SurivalGameLost = true;
			
		if(!Global.SurivalGameLost)
		{
			if(ListOfAliveUnit.Count > 0)
			{
				TimeSoFar += Time.deltaTime;
				TillNextMonster -= Time.deltaTime;
			}
			else
			{ 
				TimeSoFar += Time.deltaTime * 2f;
				TillNextMonster -= Time.deltaTime * 1.5f;
			}	
			MonsterTypeUpgrade();
			if(TillNextMonster <= 0)
			{
				TillNextMonster = 3f - TimeSoFar * 0.05f; 
				TillNextMonster = Mathf.Clamp(TimeSoFar, 0.5f, 2f);
				SpawnNewUnit();
			}
		}
		int seconds = (int)TimeSoFar%60;
		int minute = (int)TimeSoFar/60;
		TimePassed.text = "Time: " + minute.ToString("00") + ":" + seconds.ToString("00");
		TimeTill.text = "Time Left: " + TillNextMonster.ToString("N2");
		MonsterNo.text = "Monsters: " + ListOfAliveUnit.Count.ToString();
	}
	
	//Change Range of Monsters to be spawned
	void MonsterTypeUpgrade()
	{
		if(LastTime < 5 && 5 <= TimeSoFar)
		{
			ToSpawnOrNot[MonsterType.MONSTER_DWOLF] = true;
		}
		if(LastTime < 12 && 12 <= TimeSoFar)
		{
			ToSpawnOrNot[MonsterType.MONSTER_NSLIME] = false;
			ToSpawnOrNot[MonsterType.MONSTER_RSLIME] = true;
		}
		if(LastTime < 18 && 18 <= TimeSoFar)
		{
			ToSpawnOrNot[MonsterType.MONSTER_YSLIME] = true;
		}		
		if(LastTime < 30 && 30 <= TimeSoFar)
		{
			ToSpawnOrNot[MonsterType.MONSTER_DWOLF] = false;
			ToSpawnOrNot[MonsterType.MONSTER_GWOLF] = true;
		}
	}
	
	void RemoveDeadMonstersFromList()
	{
		for(int i = ListOfAliveUnit.Count-1; i >= 0; i--) 
		{
			if(ListOfAliveUnit[i] == null)
				ListOfAliveUnit.RemoveAt(i);
		}
	}
	
	void SpawnNewUnit()
	{
		MonsterType TypeToSpawn = GetRandomUnit();
		Vector3 RandomPosition = RandomSpawnSpotWithinBoundary();
		
		GameObject WhichMonsterPrefab = null;
		switch(TypeToSpawn)
		{
		case MonsterType.MONSTER_NSLIME:
			WhichMonsterPrefab = NormalSlimePrefab;
			break;
			
		case MonsterType.MONSTER_RSLIME:
			WhichMonsterPrefab = RedSlimePrefab;
			break;
		
		case MonsterType.MONSTER_YSLIME:
			WhichMonsterPrefab = YellowSlimePrefab;
			break;
				
		case MonsterType.MONSTER_DWOLF:
			WhichMonsterPrefab = DarkWolfPfrefab;
			break;
			
		case MonsterType.MONSTER_GWOLF:
			WhichMonsterPrefab = GhostWolfPrefab;
			break;			
		}
		GameObject NewMonster = Instantiate(WhichMonsterPrefab, RandomPosition, Quaternion.identity) as GameObject;
		NewMonster.transform.parent = MonstersParent.transform;
		
		switch(TypeToSpawn)
		{
		case MonsterType.MONSTER_NSLIME:
			ListOfAliveUnit.Add(NewMonster);
			NewMonster.GetComponent<Blob>().SurvivalPlayArea = PlayArea;
			break;
			
		case MonsterType.MONSTER_RSLIME:
			ListOfAliveUnit.Add(NewMonster);
			NewMonster.GetComponent<RedBlob>().SurvivalPlayArea = PlayArea;
			break;
			
		case MonsterType.MONSTER_YSLIME:
			ListOfAliveUnit.Add(NewMonster);
			NewMonster.GetComponent<YellowBlob>().SurvivalPlayArea = PlayArea;
			break;
			
		case MonsterType.MONSTER_DWOLF:
			ListOfAliveUnit.Add(NewMonster.transform.FindChild("DarkWolf").gameObject);
			NewMonster.GetComponentInChildren<DarkWolf>().SurvivalPlayArea = PlayArea;
			NewMonster.GetComponentInChildren<DarkWolf>().SetSurvival(MonstersParent);
			break;
			
		case MonsterType.MONSTER_GWOLF:
			ListOfAliveUnit.Add(NewMonster.transform.FindChild("GhostWolf").gameObject);
			NewMonster.GetComponentInChildren<GhostWolf>().SurvivalPlayArea = PlayArea;
			NewMonster.GetComponentInChildren<GhostWolf>().SetSurvival(MonstersParent);
			break;			
		}
	}
	
	
	void SpawnFirstUnit()
	{
		Vector3 RandomPosition =new Vector3(Random.Range(0f - 5f,
		                                                 0f + 5f),
		                                    Random.Range(2.5f - 2.5f,
		             						2.5f + 2.5f),
		                                    0.0f);
		GameObject WhichMonsterPrefab =  NormalSlimePrefab;
		GameObject NewMonster = Instantiate(WhichMonsterPrefab, RandomPosition, Quaternion.identity) as GameObject;
		ListOfAliveUnit.Add(NewMonster);
		NewMonster.transform.parent = MonstersParent.transform;
	}
	
	Vector3 RandomSpawnSpotWithinBoundary()
	{
		return new Vector3(Random.Range(PlayArea.transform.position.x - PlayArea.transform.localScale.x * 0.45f,
		                                PlayArea.transform.position.x + PlayArea.transform.localScale.x * 0.45f),
		                   Random.Range(PlayArea.transform.position.y - PlayArea.transform.localScale.y * 0.45f,
		             PlayArea.transform.position.y + PlayArea.transform.localScale.y * 0.45f),
		                   0.0f);
	}
	
	MonsterType GetRandomUnit()
	{
		List<MonsterType> TempList = new List<MonsterType>(); 
		foreach(MonsterType Type in ToSpawnOrNot.Keys)
		{
			switch(ToSpawnOrNot[Type])
			{
				case true:
				TempList.Add(Type);
				break;
			}				
		}
		return TempList[Random.Range(0,TempList.Count)];
	}
	
	public float GetScore()
	{	
		return TimeSoFar;
	}
}
