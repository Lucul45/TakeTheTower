using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class DashState : APlayerState
{
    private Vector2 _recordedInput = Vector2.zero;
    public override void Enter()
    {
        _stateManager.AttackPressed += Attack;
        _stateManager.ParryPressed += Parry;
        _recordedInput = _stateManager.RecordInput();
        if (_stateManager.CanDash)
        {
            _animator.SetBool("IsDashing", true);
        }
        else
        {
            _stateManager.ChangeState(EPlayerState.IDLE);
        }
    }

    public override void Exit()
    {
        _animator.SetBool("IsDashing", false);
        _stateManager.AttackPressed -= Attack;
        _stateManager.ParryPressed -= Parry;
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
        if (_animator.GetBool("IsDashing") && _animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                _stateManager.ChangeState(EPlayerState.IDLE);
            }
            else if (_stateManager.CanDash)
            {
                _stateManager.StartCoroutine(_stateManager.Dash());
            }
        }
    }

    private void Attack()
    {
        _stateManager.ChangeState(EPlayerState.MELEE);
    }

    private void Parry()
    {
        if (_stateManager.CanDash)
        {
            _stateManager.ChangeState(EPlayerState.PARRY);
        }
    }
}
