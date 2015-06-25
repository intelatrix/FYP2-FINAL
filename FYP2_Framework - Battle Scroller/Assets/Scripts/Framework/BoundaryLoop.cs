using UnityEngine;
using System.Collections;

public class BoundaryLoop : MonoBehaviour
{
    public Collider Boundary;

    void Update()
    {
        float Left = Boundary.transform.position.x - Boundary.bounds.size.x * 0.5f,
              Right = Boundary.transform.position.x + Boundary.bounds.size.x * 0.5f,
              Up = Boundary.transform.position.y + Boundary.bounds.size.y * 0.5f,
              Down = Boundary.transform.position.y - Boundary.bounds.size.y * 0.5f;

        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, Left, Right), Mathf.Clamp(this.transform.position.y, Down, Up), this.transform.position.z);
    }
}
