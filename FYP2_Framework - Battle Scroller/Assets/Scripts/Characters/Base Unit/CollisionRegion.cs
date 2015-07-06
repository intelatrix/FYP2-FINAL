using UnityEngine;
using System.Collections;

public class CollisionRegion : MonoBehaviour 
{
    //Flag to check if Unit has made contact with another Collider
	public bool inRegion = false;

    //Region's Type
    public enum RegionType
    {
        REGION_UNWALKABLE,
        REGION_BLOB_ATTACK
        
    } public RegionType Type;

	void OnTriggerStay(Collider col)
    {
		switch(Type)
		{
		case RegionType.REGION_UNWALKABLE:
			if (col.gameObject.tag == "UNWALKABLE" || col.gameObject.tag == "PLAYER")
		    inRegion = true;
		    break;
		case RegionType.REGION_BLOB_ATTACK:
	   		break;
		}
    }

    void OnTriggerExit(Collider col)
    {
		switch(Type)
		{
		case RegionType.REGION_UNWALKABLE:
			if (col.gameObject.tag == "UNWALKABLE" || col.gameObject.tag == "PLAYER")
				inRegion = false;
			break;
		case RegionType.REGION_BLOB_ATTACK:
			break;		
		}
    }
}