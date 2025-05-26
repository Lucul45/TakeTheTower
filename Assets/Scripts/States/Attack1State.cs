using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1State : MeleeBaseState
{
    public override void Enter()
    {
        //_attackIndex = 1;
        //_duration = 0.5f;
        _animator.SetBool("IsAttacking1", true);
    }

    public override void Exit()
    {
        //if (!_shouldCombo)
        {
            _animator.SetBool("IsAttacking1", false);
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
        /*if (_stateManager.FixedTime >= _duration)
        {
            if (_shouldCombo)
            {
                _stateManager.ChangeState(EPlayerState.ATTACK2);
            }
            else
            {
                if (_stateManager.MovementInput.x == 0)
                {
                    _stateManager.ChangeState(EPlayerState.IDLE);
                }
            }
        }*/
    }
}
