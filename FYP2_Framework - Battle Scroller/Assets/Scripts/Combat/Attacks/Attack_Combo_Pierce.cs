using UnityEngine;
using System.Collections;

public class Attack_Combo_Pierce : AttackScript
{
    public BulletPierce BulletPrefab;
    bool b_Instantiated = false;

    //Use this for initialization
    void Start()
    {
        //Set Combo Flag
        this.isCombo = true;

        //Init from Parent
        this.Init();

        //Set Type
        this.AttackType = AType.ATTACK_COMBO_FIRE;

        //Set Keys (Combo Chains)
        ListOfKeys.Add(KeyCode.X);
        ListOfKeys.Add(KeyCode.Z);
        ListOfKeys.Add(KeyCode.Z);
        ListOfKeys.Add(KeyCode.X);
        ListOfKeys.Add(KeyCode.Z);

        //Set Damage
        this.Damage = 45.0f;

        //Set Anim Index
        this.AnimationIndex = 2; //6

        //Set Anim Time
        this.AnimationTimer.Time = 0.2f;

        // -- Fire Coroutines
        StartCoroutine(ComboFire());
    }

    //Update is called once per frame
    void Update()
    {
        //Update from Parent
        this.StaticUpdate();
    }

    // === Firing Combo === //
    IEnumerator ComboFire()
    {
        while (this.gameObject.activeSelf)
        {
            // -- Combo Executing
            if (this.isAnimating && !b_Instantiated && BulletPrefab != null)
            {
                BulletPierce TempBullet = Instantiate(BulletPrefab, PlayerInstance.Instance.transform.position,
                                                      Quaternion.identity) as BulletPierce;
                float Negate = 1;
                if (Movement.Instance.facingLeft)
                    Negate *= -1;
                TempBullet.Dir = new Vector2(1.0f * Negate, 0.0f);
                TempBullet.f_Speed = 8.0f;
                TempBullet.transform.parent = GameObject.FindGameObjectWithTag("ASPECT_RATIO").transform;
                b_Instantiated = true;
            }

            // -- Reset Instantiate Flag
            if (!this.isAnimating)
                b_Instantiated = false;

            yield return null;
        }
    }
}