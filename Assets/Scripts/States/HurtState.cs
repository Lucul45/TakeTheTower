using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HurtState : APlayerState
{
    private uint _hitAttackFrame = 0;
    public override void Enter()
    {
        if (_playerController.PlayerID == 1)
        {
            StateFrameP1 = 0;
        }
        else if (_playerController.PlayerID == 2)
        {
            StateFrameP2 = 0;
        }
        if (FrameManager.Instance.PlayersActionFrames[FrameManager.Instance.ElapsedFrames][0].PlayerID != _playerController.PlayerID)
        {
            _hitAttackFrame = FrameManager.Instance.PlayersActionFrames[FrameManager.Instance.ElapsedFrames][0].StateFrame;
        }
        else
        {
            _hitAttackFrame = FrameManager.Instance.PlayersActionFrames[FrameManager.Instance.ElapsedFrames][1].StateFrame;
        }
        FrameManager.Instance.FrameDataUI.ResetAdvantageCalculated();
        _playerController.ResetCombo();
        _animator.SetBool("IsHurt", true);
        // Take damage
        _playerHealth.TakeDamage(_opponent.CurrentAttack.AttackDamage);
        // Freeze the screen a few time to make the hits seem more impactful
        FreezeFrameManager.Instance.StartCoroutine(FreezeFrameManager.Instance.Freeze());
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
        if (_playerController.PlayerID == 1)
        {
            StateFrameP1++;
            if (_playerHealth.CurrentHealth <= 0)
            {
                _stateManager.ChangeStateP1(EPlayerState.DEAD);
            }
            // If the frame on the current is greater or equal than hitstun, then change state to idle
            if (StateFrameP1 >= (int)(_opponent.CurrentAttack.Clip.length * 60) - _hitAttackFrame + _opponent.CurrentAttack.AdvantageFrames)
            {
                _stateManager.ChangeStateP1(EPlayerState.IDLE);
            }
        }
        else if (_playerController.PlayerID == 2)
        {
            StateFrameP2++;
            if (_playerHealth.CurrentHealth <= 0)
            {
                _stateManager.ChangeStateP2(EPlayerState.DEAD);
            }
            // If the frame on the current is greater or equal than hitstun, then change state to idle
            if (StateFrameP2 >= (int)(_opponent.CurrentAttack.Clip.length * 60) - _hitAttackFrame + _opponent.CurrentAttack.AdvantageFrames)
            {
                _stateManager.ChangeStateP2(EPlayerState.IDLE);
            }
        }
    }
}
