using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlatformBehavior : MonoBehaviour
{
    private PlatformEffector2D _effector;

    // Start is called before the first frame update
    void Start()
    {
        _effector = GetComponent<PlatformEffector2D>();

        int layerP1 = LayerMask.NameToLayer("Player1"); // ici ça vaut 7 par ex
        int layerP2 = LayerMask.NameToLayer("Player2"); // ici ça vaut 8 par ex

        int layerP1Shifted = 1 << layerP1; //   1000 0000
        int layerP2Shifted = 1 << layerP2; // 1 0000 0000
    }

    // Update is called once per frame
    void Update()
    {
        /*
        int mask = 0;

        if (player1.IsLookingDown())
        {
            mask |= layerP1Shifted; // on OR ici, donc mask devient 1000 0000
        }
        if (player2.IsLookingDown())
        {
            mask |= layerP2Shifted; // on OR ici, donc mask devient 1 0000 0000 ou 1 1000 000
        }

        _effector.colliderMask = mask;
        */
    }
}
