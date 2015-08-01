using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SurvivalSystem : MonoBehaviour {

	List<Unit> ListOfAliveUnit = new List<Unit>();
	
	float TillNextMonster = 0;
	float TimeSoFar = 0;
	// Use this for initialization
	void Start () 
	{
		
		this.gameObject.SetActive(false); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		TimeSoFar += Time.deltaTime;
	}
	
	void SpawnNewUnit()
	{
		
	}
}
