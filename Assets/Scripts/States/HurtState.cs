using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HurtState : APlayerState
{
    private int _hitAttackFrame = 0;
    public override void Enter()
    {
        base.Enter();
        if (_opponent.PlayerID == 1)
        {
            _hitAttackFrame = (int)PlayerStateMachineManager.Instance.CurrentStateP1.StateFrame;
        }
        else
        {
            _hitAttackFrame = (int)PlayerStateMachineManager.Instance.CurrentStateP2.StateFrame;
        }
        FrameManager.Instance.FrameDataUI.ResetAdvantageCalculated();
        _playerController.ResetCombo();
        _animator.SetBool("IsHurt", true);

        // Take damage and knockback
        _playerController.TakeHit(_opponent.CurrentAttack, _opponent.transform.position.x, _playerController.MovementInput);

        // Freeze the screen a few time to make the hits seem more impactful
        FreezeFrameManager.Instance.StartCoroutine(FreezeFrameManager.Instance.Freeze(0.2f));
    }

    public override void Exit()
    {
        _animator.SetBool("IsHurt", false);
    }

    public override void Init(PlayerController opponent, PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rb, PlayerController playerController, PlayerHealth playerHealth)
    {
        _opponent = opponent;
        _stateManager = stateManager;
        _animator = animator;
        _spriteRenderer = spriteRenderer;
        _rb = rb;
        _playerController = playerController;
        _playerHealth = playerHealth;
    }

    public override void Update()
    {
        base.Update();

        int hitstunDuration = _opponent.CurrentAttack.AttackTotalTime - _hitAttackFrame + _opponent.CurrentAttack.AdvantageFrames;

        // If the frame on the current is greater or equal than hitstun, then change state to idle
        if (StateFrame >= hitstunDuration)
        {
            if (_playerController.IsGrounded())
            {
                _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.IDLE);
            }
            else
            {
                _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.AIRBASE);
            }
        }

        _animator.SetBool("IsGrounded", _playerController.IsGrounded());
    }
}
