using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeBaseState : APlayerState
{
    private bool _shouldCombo = false;
    private int _attackIndex = 1;

    public override void Enter()
    {
        switch (_attackIndex)
        {
            case 1:
                _animator.SetBool("IsAttacking1", true);
                Debug.Log("Play Attack 1");
                break;
            case 2:
                _animator.SetBool("IsAttacking2", true);
                break;
            case 3:
                _animator.SetBool("IsAttacking3", true);
                break;
        }
        _stateManager.AttackPressed += Attack;
        _stateManager.CanAttack = true;
        //set hitbox attack
    }

    public override void Exit()
    {
        _stateManager.CanAttack = false;
        _stateManager.AttackPressed -= Attack;
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
        Debug.Log(_stateManager.Attack1CanCombo.Contains<Sprite>(_spriteRenderer.sprite) || _stateManager.Attack2CanCombo.Contains<Sprite>(_spriteRenderer.sprite));
        Debug.Log(_attackIndex);
        if (_stateManager.Attack1CanCombo.Contains<Sprite>(_spriteRenderer.sprite) || _stateManager.Attack2CanCombo.Contains<Sprite>(_spriteRenderer.sprite))
        {
            _shouldCombo = true;
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Debug.Log("Reset");
            ResetCombo();
            _stateManager.ChangeState(EPlayerState.IDLE);
        }
    }

    private void ResetCombo()
    {
        _attackIndex = 1;
        _shouldCombo = false;
        _animator.SetBool("IsAttacking1", false);
        _animator.SetBool("IsAttacking2", false);
        _animator.SetBool("IsAttacking3", false);
    }

    private void Attack()
    {
        if (_shouldCombo)
        {
            _attackIndex++;
            _stateManager.ChangeState(EPlayerState.MELEE);
        }
    }
}
