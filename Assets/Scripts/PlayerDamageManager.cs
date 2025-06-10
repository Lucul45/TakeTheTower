using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageManager : MonoBehaviour
{
    private PlayerStateMachineManager _stateMachineManager;
    [SerializeField] private HealthBar _healthBar;

    [Range(0, 0.5f)]
    [SerializeField] private float _freezeDuration = 0.5f;

    private bool _freezeEnabled = false;

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
        
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _stateMachineManager.OtherPlayer = collision.GetComponentInParent<PlayerStateMachineManager>().gameObject;
        if (tag == "Player1" && collision.transform.parent.gameObject.tag == "Player2")
        {
            if (!_stateMachineManager.IsParrying)
            {
                if (collision.GetComponentInParent<PlayerStateMachineManager>().CurrentAttack != null)
                {
                    _stateMachineManager.ChangeState(EPlayerState.HURT);
                    TakeDamage(collision.GetComponentInParent<PlayerStateMachineManager>().CurrentAttack.AttackDamage);
                    if (!_freezeEnabled)
                    {
                        StartCoroutine(Freeze());
                    }
                    Debug.Log("HIT " + name);
                }
                    
            }
        }
        if (tag == "Player2" && collision.transform.parent.gameObject.tag == "Player1")
        {
            if (!_stateMachineManager.IsParrying)
            {
                if (collision.GetComponentInParent<PlayerStateMachineManager>().CurrentAttack != null)
                {
                    _stateMachineManager.ChangeState(EPlayerState.HURT);
                    TakeDamage(collision.GetComponentInParent<PlayerStateMachineManager>().CurrentAttack.AttackDamage);
                    if (!_freezeEnabled)
                    {
                        StartCoroutine(Freeze());
                    }
                    Debug.Log("HIT " + name);
                }
                else
                {
                    Debug.Log("current Attack error");
                }
            }
        }
        else
        {
            Debug.Log("tag error");
        }
    }

    public IEnumerator Freeze()
    {
        _freezeEnabled = true;
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(_freezeDuration);

        Time.timeScale = 1;
        _freezeEnabled = false;
    }
}
