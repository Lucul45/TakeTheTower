using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    [SerializeField] private int _towerID = 0;
    private int _currentHealth = 100;
    private int _maxHealth = 100;

    public int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
            if (_currentHealth <= 0)
            {
                if (_towerID == 1)
                {
                    WinCondition.Instance.P1TowersStanding -= 1;
                    Destroy(gameObject);
                }
                else if (_towerID == 2)
                {
                    WinCondition.Instance.P2TowersStanding -= 1;
                    Destroy(gameObject);
                }
            }
        }
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        
    }

    public void GetDamaged(int damage)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _maxHealth);
    }
}
