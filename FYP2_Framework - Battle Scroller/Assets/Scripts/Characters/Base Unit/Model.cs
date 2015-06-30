using UnityEngine;
using System.Collections;

public class Model : MonoBehaviour 
{
    //Every Model has it's own Collision Regions
    public ColliderManager CollisionRegions;
    public bool isAnimated = true;
    public short CurAnimationIndex = 0;
    Animator ThisAnimator;

    //Returns own Animator
    public Animator GetAnimation()
    {
        if (isAnimated)
            return this.GetComponent<Animator>();
        return null;
    }

    //Set Animation
    public void SetAnimation(short AnimationIndex)
    {
        if (isAnimated)
        {
            this.GetComponent<Animator>().SetInteger("Type", AnimationIndex);
            CurAnimationIndex = AnimationIndex;
        }
    }

	//Check if the animation type is same as the number pass in
	public bool IsAnimation(short IndexCheck)
	{
	
		return IndexCheck == CurAnimationIndex;
	}
	
	public void SetTrigger(string Trigger)
	{
		this.GetComponent<Animator>().SetTrigger(Trigger);
	}
	
	//Use this for initialization
	void Start () 
    {
        //Default to first Animation
        SetAnimation(0);
	}
	
	//Update is called once per frame
	void Update () 
    {

	}
}
