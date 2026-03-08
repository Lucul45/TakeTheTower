using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : Singleton<WinCondition>
{
    private int _p1TowersStanding = 3;
    private int _p2TowersStanding = 3;

    public int P1TowersStanding
    {
        get
        {
            return _p1TowersStanding;
        }
        set
        {
            _p1TowersStanding = value;
            if (value <= 0)
            {
                P2Wins();
            }
        }
    }
    public int P2TowersStanding
    {
        get
        {
            return _p2TowersStanding;
        }
        set
        {
            _p2TowersStanding = value;
            if (value <= 0)
            {
                P1Wins();
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void P1Wins()
    {
        WinMenuManager.Instance.WinScreen("Player1");
    }

    public void P2Wins()
    {
        WinMenuManager.Instance.WinScreen("Player2");
    }
}
