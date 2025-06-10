using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : APlayerState
{
    public override void Enter()
    {
        _animator.SetBool("IsDead", true);
    }

    public override void Exit()
    {
        
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
        if (_animator.GetBool("IsDead") && _animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !_animator.IsInTransition(0))
            {
                _winManager.WinScreen(_stateManager.gameObject.name);
            }

        }
    }
}
