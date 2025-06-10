using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : APlayerState
{
    public override void Enter()
    {
        _stateManager.AttackPressed += Attack;
        _stateManager.ParryPressed += Parry;
        _stateManager.DashPressed += Dash;
        _stateManager.Move(Vector2.zero);
    }

    public override void Exit()
    {
        _stateManager.AttackPressed -= Attack;
        _stateManager.ParryPressed -= Parry;
        _stateManager.DashPressed -= Dash;
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
        if (_stateManager.PlayerDamageManager.CurrentHealth <= 0)
        {
            _stateManager.ChangeState(EPlayerState.DEAD);
        }
        if (_stateManager.MovementInput.x != 0f)
        {
            _stateManager.ChangeState(EPlayerState.MOVE);
        }
        if (_stateManager.PlayerDamageManager.CurrentHealth == 0)
        {
            _stateManager.ChangeState(EPlayerState.DEAD);
        }
    }

    private void Attack()
    {
        if(_stateManager.CanAttack)
        {
            _stateManager.ChangeState(EPlayerState.MELEE);
        }
    }

    private void Parry()
    {
        _stateManager.ChangeState(EPlayerState.PARRY);
    }

    private void Dash()
    {
        _stateManager.ChangeState(EPlayerState.DASH);
    }
}
