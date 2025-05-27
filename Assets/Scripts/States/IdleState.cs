using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : APlayerState
{
    public override void Enter()
    {
        _stateManager.AttackPressed += Attack;
        _stateManager.ParryPressed += Parry;
    }

    public override void Exit()
    {
        _stateManager.AttackPressed -= Attack;
        _stateManager.ParryPressed -= Parry;
    }

    public override void Init(PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer)
    {
        _stateManager = stateManager;
        _animator = animator;
        _spriteRenderer = spriteRenderer;
    }

    public override void Update()
    {
        if (_stateManager.MovementInput.x != 0f)
        {
            _stateManager.ChangeState(EPlayerState.MOVE);
        }
    }

    private void Attack()
    {
        _stateManager.ChangeState(EPlayerState.MELEE);
    }

    private void Parry()
    {
        _stateManager.ChangeState(EPlayerState.PARRY);
    }
}
