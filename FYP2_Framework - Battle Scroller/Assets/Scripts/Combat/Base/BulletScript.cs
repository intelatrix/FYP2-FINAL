using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour 
{
    // === Variables === //
    [HideInInspector]
    public Vector2 Dir = Vector2.zero;
    [HideInInspector]
    public float f_Speed = 5.0f;

    // === Initialisation === //
    void Start()
    {
        StartCoroutine(MoveBullet());
    }

    // === Collision Detection === //
    void OnTriggerEnter(Collider Col)
    {
        if (Col.tag == "UNIT")
        {
            Unit Enemy = Col.GetComponentInParent<Unit>();
            Unit Player = PlayerInstance.Instance.gameObject.GetComponent<Unit>();
            Enemy.Stats.TakePhysicalDamage(Player.Stats, 10.0f);
            Destroy(this.gameObject);
        }
    }

    // === Bullet Movement === //
    IEnumerator MoveBullet()
    {
        while (f_Speed > 0.0f)
        {
            this.transform.Translate(new Vector3(Dir.x * Time.deltaTime * f_Speed,
                                                 Dir.y * Time.deltaTime * f_Speed, 0.0f));

            if (!Camera.main.GetComponent<BoxCollider>().bounds.Contains(this.transform.position))
                Destroy(this.gameObject);

            yield return null;
        }
    }
}
