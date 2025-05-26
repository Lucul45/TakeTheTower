using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2State : MeleeBaseState
{
    public override void Enter()
    {
        //_attackIndex = 2;
        //_duration = 0.5f;
        _animator.SetBool("IsAttacking2", true);
    }

    public override void Exit()
    {
        //if (!_shouldCombo)
        {
            _animator.SetBool("IsAttacking1", false);
            _animator.SetBool("IsAttacking2", false);
        }
        _stateManager.FixedTime = 0;
    }

    public override void Init(PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer)
    {
        _stateManager = stateManager;
        _animator = animator;
        _spriteRenderer = spriteRenderer;
    }

    public override void Update()
    {
        _stateManager.Move(Vector2.zero);
        /*if (_stateManager.FixedTime >= _duration)
        {
            if (_shouldCombo)
            {
                _stateManager.ChangeState(EPlayerState.ATTACK3);
            }
            else
            {
                if (_stateManager.MovementInput.x == 0)
                {
                    _stateManager.ChangeState(EPlayerState.IDLE);
                }
                else if (_stateManager.MovementInput.x != 0f)
                {
                    _stateManager.ChangeState(EPlayerState.MOVE);
                }
            }
        }*/
    }
}
