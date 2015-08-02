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
	public GameObject[] GameOverObj;
	public GameObject MonstersParent;

	//Update Func
	void Update () 
    {
//        if (WaveList[curIndex].b_WaveCleared)
//        {
//            ++curIndex;
//            WaveList.Add(new Wave());
//            WaveList[curIndex] = Instantiate(WavePrefab, new Vector3(WaveList[curIndex - 1].transform.position.x + Offset,
//                                                                     WaveList[curIndex - 1].transform.position.y, 0),
//                                             Quaternion.identity) as Wave;
//            WaveList[curIndex - 1].b_ProceedToDestroy = true;
//        }

		if (WaveList.Count > 0 && WaveList[0].b_WaveCleared)
		{
			WaveList[0].doNotPan = (WaveList.Count == 1);
				
			++curIndex;
			WaveList[0].b_ProceedToDestroy = true;
			WaveList.RemoveAt(0);
			if (WaveList.Count > 0)
			{
				foreach(Transform child in WaveList[0].transform)
				{
					child.gameObject.SetActive(true);
				}
			}
		}
		
		if (WaveList.Count <= 0)
		{
			for (short i = 0; i < GameOverObj.Length; ++i)
				GameOverObj[i].SetActive(true);
		}

        if (WaveText != null)
            WaveText.text = "Wave: " + (curIndex + 1);
	}
}
