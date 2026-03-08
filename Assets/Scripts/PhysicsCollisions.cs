using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCollisions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(6, 7);
        Physics2D.IgnoreLayerCollision(8, 10);
        Physics2D.IgnoreLayerCollision(9, 11);
    }
}
