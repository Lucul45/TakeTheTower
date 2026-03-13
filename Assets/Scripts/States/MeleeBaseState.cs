using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class MeleeBaseState : APlayerState
{
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
        FrameManager.Instance.FrameDataUI.ResetAdvantageCalculated();
        // Getting the current attack based on which index we are on
        foreach (AttackData a in _playerController.AttacksData)
        {
            if (a.AttackID == _playerController.AttackIndex)
            {
                _playerController.CurrentAttack = a;
            }
        }
        _animator.SetBool(_playerController.CurrentAttack.AnimatorCondition, true);
        _playerController.AttackPressed += Attack;
    }

    public override void Exit()
    {
        _playerController.IsHitting = false;
        _playerController.AttackPressed -= Attack;
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
        _playerController.ShouldCombo = false;
        _playerController.AttackIndex = 1;
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
        }
        else if (_playerController.PlayerID == 2)
        {
            StateFrameP2++;
            if (_playerHealth.CurrentHealth <= 0)
            {
                _stateManager.ChangeStateP2(EPlayerState.DEAD);
            }
        }
        // Making sure the character can't move while attacking
        _playerController.Move(Vector2.zero);
        // If the character attack is on a frame where he can combo
        if (_playerController.CurrentAttack.CanComboFrames.Contains<Sprite>(_spriteRenderer.sprite))
        {
            _playerController.ShouldCombo = true;
        }
        else
        {
            _playerController.ShouldCombo = false;
        }
        if (_playerController.PlayerID == 1)
        {
            // If the current state frame is greater or equal to the clip length in frames
            if (StateFrameP1 >= _playerController.CurrentAttack.Clip.length * 60)
            {
                _playerController.ResetCombo();
                _stateManager.ChangeStateP1(EPlayerState.IDLE);
            }
        }
        else
        {
            // If the current state frame is greater or equal to the clip length in frames
            if (StateFrameP2 >= _playerController.CurrentAttack.Clip.length * 60)
            {
                _playerController.ResetCombo();
                _stateManager.ChangeStateP2(EPlayerState.IDLE);
            }
        }
    }


    private void Attack()
    {
        if (_playerController.ShouldCombo && _playerController.AttackIndex < 3)
        {
            _playerController.AttackIndex++;
            if (_playerController.PlayerID == 1)
            {
                _stateManager.ChangeStateP1(EPlayerState.MELEE);
            }
            else
            {
                _stateManager.ChangeStateP2(EPlayerState.MELEE);
            }
        }
    }
}
