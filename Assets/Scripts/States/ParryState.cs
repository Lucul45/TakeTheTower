using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryState : APlayerState
{
    public override void Enter()
    {
        _animator.SetBool("IsParrying", true);
        _stateManager.IsParrying = true;
    }

    public override void Exit()
    {
        _animator.SetBool("IsParrying", false);
        _stateManager.IsParrying = false;
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
        _stateManager.Move(Vector2.zero);
        if (_stateManager.PerfectParryFrame == _spriteRenderer.sprite)
        {
            _stateManager.PerfectParry = true;
        }
        else
        {
            _stateManager.PerfectParry = false;
        }
        if (_animator.GetBool("IsParrying") && _animator.GetCurrentAnimatorStateInfo(0).IsName("Parry"))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                _stateManager.CanParry = true;
                _stateManager.ChangeState(EPlayerState.IDLE);
            }
        }
    }
}
