using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wave : MonoBehaviour 
{
	// SIMPLE WAVE SYSTEM TO TEST CAMERA PANNING
    // ALLSON U CAN CHANGE THIS HOWEVER U LIKE
    public bool b_WaveCleared = false,
                b_ProceedToDestroy = false;
    public List<GameObject> ListOfEnemies = new List<GameObject>();

    void Start()
    {
        //Fire Coroutines
        StartCoroutine(CheckWave());
        StartCoroutine(CheckList());
    }

	//Check for Wave Clear
	IEnumerator CheckWave() 
    {
        while (true)
        {
            b_WaveCleared = (ListOfEnemies.Count == 0);

            if (b_ProceedToDestroy)
            {
                CameraAuto.Instance.doPan = true;
                b_WaveCleared = false;
                Destroy(this.gameObject);
            }

            yield return new WaitForSeconds(.2f);
        }
	}

    //Check for Empty Element
    IEnumerator CheckList()
    {
        while (true)
        {
            for (short i = 0; i < ListOfEnemies.Count; ++i)
            {
                if (ListOfEnemies[i] == null)
                {
                    ListOfEnemies[i] = new GameObject();
                    ListOfEnemies.RemoveAt(i);
                    break;
                }
            }

            yield return new WaitForSeconds(.1f);
        }
    }
}
