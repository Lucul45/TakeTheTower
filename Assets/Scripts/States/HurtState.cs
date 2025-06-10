using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : APlayerState
{
    private float _fixedTime = 0;
    public override void Enter()
    {
        _stateManager.Knockback(_stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CurrentAttack.KnockbackForce, 0.5f);
        _animator.SetBool("IsHurt", true);
        _stateManager.IsHurt = true;
        _fixedTime = 0;
    }

    public override void Exit()
    {
        _animator.SetBool("IsHurt", false);
    }

    public override void Init(PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rb, WinMenuManager winManager)
    {
        _stateManager = stateManager;
        _animator = animator;
        _spriteRenderer = spriteRenderer;
        _rb = rb;
        _winManager = winManager;
    }

    public override void Update()
    {
        _fixedTime += Time.deltaTime;
        if (_stateManager.PlayerDamageManager.CurrentHealth <= 0)
        {
            _stateManager.ChangeState(EPlayerState.DEAD);
        }
        if (_fixedTime >= _stateManager.OtherPlayer.GetComponent<PlayerStateMachineManager>().CurrentAttack.HurtTime)
        {
            _stateManager.IsHurt = false;
        }
        if (!_stateManager.IsHurt)
        {
            _stateManager.ChangeState(EPlayerState.IDLE);
        }
    }
}
