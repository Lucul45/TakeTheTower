using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackClank : MonoBehaviour
{
    private PlayerStateMachineManager _stateMachineManager;
    private PlayerDamageManager _damageManager;

    private void Start()
    {
        _stateMachineManager = GetComponentInParent<PlayerStateMachineManager>();
        _damageManager = GetComponentInParent<PlayerDamageManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.name == "AttackHitbox")
            {
                _stateMachineManager.Clank();
                //_stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().Clank();
            }
            else if (tag == "AttackP1" && collision.transform.parent.gameObject.tag == "Player2")
            {
                if (!_stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().IsParrying)
                {
                    _stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().ChangeState(EPlayerState.HURT);
                    _stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().PlayerDamageManager.TakeDamage(collision.GetComponentInParent<PlayerStateMachineManager>().CurrentAttack.AttackDamage);
                    if (!_damageManager.FreezeEnabled)
                    {
                        StartCoroutine(_damageManager.Freeze());
                    }

                }
            }
            else if (tag == "AttackP2" && collision.transform.parent.gameObject.tag == "Player1")
            {
                if (!_stateMachineManager.IsParrying)
                {
                    if (collision.GetComponentInParent<PlayerStateMachineManager>().CurrentAttack != null)
                    {
                        _stateMachineManager.ChangeState(EPlayerState.HURT);
                        _damageManager.TakeDamage(collision.GetComponentInParent<PlayerStateMachineManager>().CurrentAttack.AttackDamage);
                        if (!_damageManager.FreezeEnabled)
                        {
                            StartCoroutine(_damageManager.Freeze());
                        }
                    }
                }
            }
        }
    }
}
