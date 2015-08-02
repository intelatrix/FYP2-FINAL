using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour 
{
    // === Singleton Structure === //
    protected static Tutorial mInstance;
    public static Tutorial Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject tempObj = new GameObject();
                mInstance = tempObj.AddComponent<Tutorial>();
                Destroy(tempObj);
            }
            return mInstance;
        }
    }

    [HideInInspector]
    public bool b_TutorialOver = false;
    public bool b_isTutorialStage = false;
    bool ProceedToStop = false;

    public static bool isTut()
    {
        if (mInstance == null)
            return false;
        return mInstance.b_isTutorialStage;
    }

    // -- List of Tut Text
    public GameObject TextBundle,
                      AfterAttack,
                      AfterCombo;
    List<Text> TextList = new List<Text>();
    public Image theScreen;
    public GameObject ScreenObject;
    public int CurIndex = 0;

	// === Initialisation === //
	void Start () 
    {
        // -- Set Instance
        mInstance = this;

        // -- Populate
        Populate(TextBundle);
	}

    // === Populate List === //
    void Populate(GameObject TextB)
    {
        // -- Populate TextList
        TextList.Clear();
        if (TextB.GetComponentsInChildren<Text>() != null)
        {
            foreach (Text text in TextB.GetComponentsInChildren<Text>())
                TextList.Add(text);
        }
        CurIndex = 0;
        ProceedToStop = b_TutorialOver = false;
        StartCoroutine(ScreenLoop());
    }

    // === Increment Index === //
    void IncrementIndex()
    {
        if (CurIndex < TextList.Count - 1)
            ++CurIndex;
        else
            ProceedToStop = true;
    }

    // === Set Screen ON / OFF === //
    void SetScreenActive(bool Mode)
    {
        ScreenObject.SetActive(Mode);
        //if (theScreen.GetComponentInParent<GameObject>() != null)
        //    theScreen.GetComponentInParent<GameObject>().SetActive(Mode);
    }

    // === Tutorial Screens (Displays / Instructions) === //
    IEnumerator ScreenLoop()
    {
        // -- Start Transiting
        StartCoroutine(ScreenTransit());

        // -- Screen Loop
        while (!b_TutorialOver)
        {
            // -- Set Text
            if (theScreen.GetComponentInChildren<Text>() != null)
            {
                if (theScreen.GetComponentInChildren<Text>().text != TextList[CurIndex].text)
                    theScreen.GetComponentInChildren<Text>().text = TextList[CurIndex].text;
            }

            // -- Check for Tutorial Over 
            if (ProceedToStop)
            {
                StopCoroutine(ScreenTransit());
                SetScreenActive(false);
                b_TutorialOver = true;
                ProceedToStop = false;

                if (CombatManager.AttackCount == 0)
                    StartCoroutine(CheckAttack());
                else if (!CombatManager.BBAExecuted)
                    StartCoroutine(CheckCombo());
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    // === Screen Transition === //
    IEnumerator ScreenTransit()
    {
        SetScreenActive(true);
        while (!ProceedToStop)
        {
            // -- Check if User has touched down
            bool b_Incre = false;
#if UNITY_EDITOR
            b_Incre = (Input.GetMouseButtonUp(0));
#elif UNITY_ANDROID
            foreach (Touch touch in Input.touches)
                b_Incre = (touch.phase == TouchPhase.Ended);
#endif
            // -- Increment to next Screen 
            if (b_Incre)
                IncrementIndex();

            yield return null;
        }
    }

    // === Check for Attack === //
    IEnumerator CheckAttack()
    {
        StopCoroutine(ScreenTransit());
        while (true)
        {
            if (CombatManager.AttackCount >= 4)
            {
                Populate(AfterAttack);
                break;
            }

            yield return new WaitForSeconds(.1f);
        }
    }

    // === Check for Combo === //
    IEnumerator CheckCombo()
    {
        StopCoroutine(ScreenTransit());
        while (true)
        {
            if (CombatManager.BBAExecuted)
            {
                Populate(AfterCombo);
                break;
            }

            yield return new WaitForSeconds(.1f);
        }
    }
}
