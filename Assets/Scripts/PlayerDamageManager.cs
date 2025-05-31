using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageManager : MonoBehaviour
{
    private PlayerStateMachineManager _stateMachineManager;
    [SerializeField] private HealthBar _healthBar;

    private int _maxHealth = 100;
    private int _currentHealth = 100;

    public int MaxHealth
    {
        get { return _maxHealth; }
    }
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set 
        { 
            _currentHealth = value;
            _healthBar.SetHealthValue(_currentHealth);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _stateMachineManager = GetComponent<PlayerStateMachineManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHealth == 0)
        {
            Debug.Log("DEAD " + name);
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tag == "Player1" && collision.gameObject.tag == "AttackP2")
        {
            if (!_stateMachineManager.IsParrying)
            {
                _stateMachineManager.ChangeState(EPlayerState.HURT);
                TakeDamage(10);
                Debug.Log("HIT");
            }
        }
        if (tag == "Player2" && collision.gameObject.tag == "AttackP1")
        {
            if (!_stateMachineManager.IsParrying)
            {
                _stateMachineManager.ChangeState(EPlayerState.HURT);
                TakeDamage(10);
                Debug.Log("HIT");
            }
        }
    }
}
