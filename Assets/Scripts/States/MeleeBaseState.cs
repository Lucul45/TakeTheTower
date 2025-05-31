using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeBaseState : APlayerState
{
    private bool _shouldCombo = false;
    private int _attackIndex = 1;
    private AttackData _attackData;
    public override void Enter()
    {
        foreach (AttackData a in _stateManager.AttacksData)
        {
            if (a.AttackID == _attackIndex)
            {
                _attackData = a;
            }
        }
        Debug.Log(_attackData.ToString());
        _animator.SetBool(_attackData.AnimatorCondition, true);
        _stateManager.AttackPressed += Attack;
    }

    public override void Exit()
    {
        _stateManager.AttackPressed -= Attack;
    }

    public override void Init(PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rb)
    {
        _stateManager = stateManager;
        _animator = animator;
        _spriteRenderer = spriteRenderer;
        _rb = rb;
    }

    public override void Update()
    {
        _stateManager.Move(Vector2.zero);
        if (_attackData.CanComboFrames.Contains<Sprite>(_spriteRenderer.sprite))
        {
            _shouldCombo = true;
        }
        if (_animator.GetBool(_attackData.AnimatorCondition) && _animator.GetCurrentAnimatorStateInfo(0).IsName(_attackData.AnimationName))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !_animator.IsInTransition(0))
            {
                float cooldown = 0f;
                switch (_attackIndex)
                {
                    case 1:
                        cooldown = 1f;
                        break;
                    case 2:
                        cooldown = 2f;
                        break;
                    case 3:
                        cooldown = 3f;
                        break;
                }
                ResetCombo();
                _stateManager.ChangeState(EPlayerState.IDLE);
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
