using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaveSystem : MonoBehaviour 
{
    public Wave WavePrefab;
    public List<Wave> WaveList = new List<Wave>();
    float Offset = 15.0f;
    short curIndex = 0;
    public Text WaveText;

	//Update Func
	void Update () 
    {
        if (WaveList[curIndex].b_WaveCleared)
        {
            ++curIndex;
            WaveList.Add(new Wave());
            WaveList[curIndex] = Instantiate(WavePrefab, new Vector3(WaveList[curIndex - 1].transform.position.x + Offset,
                                                                     WaveList[curIndex - 1].transform.position.y, 0),
                                             Quaternion.identity) as Wave;
            WaveList[curIndex - 1].b_ProceedToDestroy = true;
        }

        if (WaveText != null)
            WaveText.text = "Wave: " + (curIndex + 1);
	}
}
