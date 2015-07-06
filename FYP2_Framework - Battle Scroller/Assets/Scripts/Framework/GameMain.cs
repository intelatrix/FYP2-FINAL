using UnityEngine;
using System.Collections;

public class GameMain : MonoBehaviour 
{
    public GameObject[] GameOverObj;

    void Start()
    {
        StartCoroutine(CheckGameOver());
        StartCoroutine(DisplayGameOver());
    }

    IEnumerator CheckGameOver()
    {
        while (true)
        {
            Global.GameOver = (Movement.Instance.theUnit.Stats.HP <= 0.0f);
            yield return new WaitForSeconds(.2f);
        }
    }

    IEnumerator DisplayGameOver()
    {
        while (true)
        {
            for (short i = 0; i < GameOverObj.Length; ++i)
                GameOverObj[i].SetActive(Global.GameOver);

            yield return new WaitForSeconds(.2f);
        }
    }
}
