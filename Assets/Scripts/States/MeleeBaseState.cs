using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeBaseState : APlayerState
{
    private bool _shouldCombo = false;
    private bool _canClank = false;
    private int _attackIndex = 1;
    public override void Enter()
    {
        if (_stateManager.LastState == EPlayerState.DASH)
        {
            _attackIndex = 0;
        }
        foreach (AttackData a in _stateManager.AttacksData)
        {
            if (a.AttackID == _attackIndex)
            {
                _stateManager.CurrentAttack = a;
            }
        }
        //_stateManager.Hitbox.tag = _stateManager.CurrentAttack.AnimationName;
        _animator.SetBool(_stateManager.CurrentAttack.AnimatorCondition, true);
        _stateManager.AttackPressed += Attack;
    }

    public override void Exit()
    {
        if (_canClank && !_shouldCombo)
        {
            ResetCombo();
        }
        //_stateManager.Hitbox.tag = "Attack";
        _stateManager.AttackPressed -= Attack;
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
        if (_stateManager.CurrentAttack.CanComboFrames.Contains<Sprite>(_spriteRenderer.sprite))
        {
            _shouldCombo = true;
        }
        else
        {
            _shouldCombo = false;
        }
        if (_animator.GetBool(_stateManager.CurrentAttack.AnimatorCondition) && _animator.GetCurrentAnimatorStateInfo(0).IsName(_stateManager.CurrentAttack.AnimationName))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !_animator.IsInTransition(0))
            {
                _canClank = false;
                ResetCombo();
                _stateManager.ChangeState(EPlayerState.IDLE);
            }
            else
            {
                _canClank = true;
            }
        }
    }

    private void ResetCombo()
    {
        _attackIndex = 1;
        _shouldCombo = false;
        _animator.SetBool("IsAttacking1", false);
        _animator.SetBool("IsAttacking2", false);
        _animator.SetBool("IsAttacking3", false);
        _animator.SetBool("IsAttackingDash", false);
    }

    private void Attack()
    {
        if (_shouldCombo && _attackIndex < 3)
        {
            _attackIndex++;
            _stateManager.ChangeState(EPlayerState.MELEE);
        }
    }
}
