using UnityEngine;
using System.Collections;

public class CameraAuto : MonoBehaviour 
{
    // === Singleton Structure === //
    protected static CameraAuto mInstance;
    public static CameraAuto Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject tempObj = new GameObject();
                mInstance = tempObj.AddComponent<CameraAuto>();
                Destroy(tempObj);
            }
            return mInstance;
        }
    }

    // === Variables === //
    float ScrollAmt = 15.0f,
          DeltaScroll = 0.0f,
          ScrollSpeed = 5.0f;
    public static float ScrollBuffer = 2.0f;
    [HideInInspector]
    public bool doPan = false;
    public Transform[] Scroll_List;

	// === Initialisation === //
	void Start () 
    {
        if (mInstance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        mInstance = this;
	}

    // === Scroll Cam === //
    // -> Mode :: True = Pan X
    // -> Mode :: False = Pan Y
    public void PanCam(GameObject RefObj, bool Mode = true)
    {
        //Check if Unit is within Camera Bounds
        bool Proceed = true;
        if (Mode)
            Proceed = (RefObj.transform.position.x - ScrollBuffer > (Camera.main.transform.position.x - Camera.main.GetComponent<Collider>().bounds.size.x * 0.5f));
        else
            Proceed = (RefObj.transform.position.y - ScrollBuffer > (Camera.main.transform.position.y - Camera.main.GetComponent<Collider>().bounds.size.y * 0.5f));

        //Only Scroll for ScrollAmt
        if (DeltaScroll < ScrollAmt)
        {
            if (Proceed)
            {
                DeltaScroll += ScrollSpeed * Time.deltaTime;

                //Pan X
                if (Mode)
                {
                    //Pan Camera
                    Camera.main.transform.Translate(ScrollSpeed * Time.deltaTime, 0.0f, 0.0f);

                    //Pan Objects in Scroll_List
                    for (short i = 0; i < Scroll_List.Length; ++i)
                        Scroll_List[i].Translate(ScrollSpeed * Time.deltaTime, 0.0f, 0.0f);
                }

                //Pan Y
                else
                {
                    //Pan Camera
                    Camera.main.transform.Translate(0.0f, ScrollSpeed * Time.deltaTime, 0.0f);

                    //Pan Objects in Scroll_List
                    for (short i = 0; i < Scroll_List.Length; ++i)
                        Scroll_List[i].Translate(ScrollSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
            }
        }
        else
        {
            //Reset 
            doPan = false;
            DeltaScroll = 0.0f;
        }
    }

    // === Check if Object has reached Cam Borders === //
    // -> Mode :: True = Check X
    // -> Mode :: False = Check Y
    public static bool CheckCamBounds(Transform RefTrans, bool Left, bool Mode = true)
    {
        if (Mode)
        {
            if (Left)
                return (RefTrans.transform.position.x - ScrollBuffer < (Camera.main.transform.position.x - Camera.main.GetComponent<Collider>().bounds.size.x * 0.5f));
            else
                return (RefTrans.transform.position.x - ScrollBuffer > (Camera.main.transform.position.x - Camera.main.GetComponent<Collider>().bounds.size.x * 0.5f));
        }
        else
        {
            if (Left)
                return (RefTrans.transform.position.y - ScrollBuffer < (Camera.main.transform.position.y - Camera.main.GetComponent<Collider>().bounds.size.y * 0.5f));
            else
                return (RefTrans.transform.position.y - ScrollBuffer > (Camera.main.transform.position.y - Camera.main.GetComponent<Collider>().bounds.size.y * 0.5f));
        }
    }

    // === Update Func === //
    void Update()
    {
        //Pan Camera
        if (doPan)
            PanCam(Movement.Instance.theUnit.gameObject);

        //Test
        if (Input.GetKeyDown(KeyCode.P))
            doPan = true;
    }
}
