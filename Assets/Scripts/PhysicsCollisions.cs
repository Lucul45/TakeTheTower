using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCollisions : Singleton<PhysicsCollisions>
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        Physics2D.IgnoreLayerCollision(8, 10, true);
        Physics2D.IgnoreLayerCollision(9, 11, true);
    }

    public void DeadCollisions(int playerIndex)
    {
        if (playerIndex == 1)
        {
            Physics2D.IgnoreLayerCollision(6, 8, true);
            Physics2D.IgnoreLayerCollision(6, 9, true);
            Physics2D.IgnoreLayerCollision(6, 10, true);
            Physics2D.IgnoreLayerCollision(6, 11, true);
        }
        else if (playerIndex == 2)
        {
            Physics2D.IgnoreLayerCollision(7, 8, true);
            Physics2D.IgnoreLayerCollision(7, 9, true);
            Physics2D.IgnoreLayerCollision(7, 10, true);
            Physics2D.IgnoreLayerCollision(7, 11, true);
        }
    }

    public void AliveCollisions(int playerIndex)
    {
        if (playerIndex == 1)
        {
            Physics2D.IgnoreLayerCollision(6, 0, false);
            Physics2D.IgnoreLayerCollision(6, 8, false);
            Physics2D.IgnoreLayerCollision(6, 9, false);
            Physics2D.IgnoreLayerCollision(6, 10, false);
            Physics2D.IgnoreLayerCollision(6, 11, false);
            Physics2D.IgnoreLayerCollision(6, 12, false);
        }
        else if (playerIndex == 2)
        {
            Physics2D.IgnoreLayerCollision(7, 0, false);
            Physics2D.IgnoreLayerCollision(7, 8, false);
            Physics2D.IgnoreLayerCollision(7, 9, false);
            Physics2D.IgnoreLayerCollision(7, 10, false);
            Physics2D.IgnoreLayerCollision(7, 11, false);
            Physics2D.IgnoreLayerCollision(7, 12, false);
        }
    }
}
