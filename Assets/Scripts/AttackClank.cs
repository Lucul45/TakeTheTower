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
            /*if (/*collision.gameObject.name == "AttackHitbox" _stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CanClank)
            {
                StartCoroutine(_stateMachineManager.Clank());
                StartCoroutine(_stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().Clank());
                return;
            }
            else if (_stateMachineManager.CurrentAttack != null && tag == "AttackP1" && collision.transform.parent.gameObject.tag == "Player2" && (!_stateMachineManager.CanClank || !_stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CanClank))
            {
                if (!_stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().IsParrying)
                {
                    _stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().ChangeState(EPlayerState.HURT);
                    _stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().PlayerDamageManager.TakeDamage(_stateMachineManager.CurrentAttack.AttackDamage);
                    if (!_damageManager.FreezeEnabled)
                    {
                        StartCoroutine(_damageManager.Freeze());
                    }

                }
            }
            else if (_stateMachineManager.CurrentAttack != null && tag == "AttackP2" && collision.transform.parent.gameObject.tag == "Player1" && (!_stateMachineManager.CanClank || !_stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CanClank))
            {
                if (!_stateMachineManager.IsParrying)
                {
                    _stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().ChangeState(EPlayerState.HURT);
                    _stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().PlayerDamageManager.TakeDamage(_stateMachineManager.CurrentAttack.AttackDamage);
                    if (!_damageManager.FreezeEnabled)
                    {
                        StartCoroutine(_damageManager.Freeze());
                    }
                }
            }*/
            if (_stateMachineManager.CurrentAttack != null && ((tag == "AttackP1" && collision.transform.parent.gameObject.tag == "Player2") || (tag == "AttackP2" && collision.transform.parent.gameObject.tag == "Player1")))
            {
                if (_stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CanClank)
                {
                    StartCoroutine(_stateMachineManager.Clank());
                    StartCoroutine(_stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().Clank());
                }
                else if (!_stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().IsParrying)
                {
                    _stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().ChangeState(EPlayerState.HURT);
                    _stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().PlayerDamageManager.TakeDamage(_stateMachineManager.CurrentAttack.AttackDamage);
                    if (!_damageManager.FreezeEnabled)
                    {
                        StartCoroutine(_damageManager.Freeze());
                    }
                    Debug.Log(_stateMachineManager.CanClank + "/" + _stateMachineManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CanClank);
                }
            }
            
            
        }
    }
}
