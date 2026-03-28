using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IdleState : APlayerState
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
        _playerController.JumpPressed += Jump;
        _playerController.AttackPressed += Attack;
    }

    public override void Exit()
    {
        _playerController.JumpPressed -= Jump;
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
            // If the input isn't neutral
            else if (_playerController.MovementInput.x != 0f)
            {
                _stateManager.ChangeStateP1(EPlayerState.MOVE);
            }
        }
        else if ( _playerController.PlayerID == 2)
        {
            StateFrameP2++;
            if (_playerHealth.CurrentHealth <= 0)
            {
                _stateManager.ChangeStateP2(EPlayerState.DEAD);
            }
            // If the input isn't neutral
            else if (_playerController.MovementInput.x != 0f)
            {
                _stateManager.ChangeStateP2(EPlayerState.MOVE);
            }
        }
        _animator.SetBool("IsGrounded", _playerController.IsGrounded());
    }

    private void Attack()
    {
        if (_playerController.PlayerID == 1)
        {
            if (_playerController.CanAttack)
            {
                _stateManager.ChangeStateP1(EPlayerState.JAB);
            }
        }
        else if (_playerController.PlayerID == 2)
        {
            if (_playerController.CanAttack)
            {
                _stateManager.ChangeStateP2(EPlayerState.JAB);
            }
        }
    }

    private void Jump()
    {
        if (_playerController.PlayerID == 1)
        {
            if (_playerController.CanJump && _playerController.IsGrounded())
            {
                _stateManager.ChangeStateP1(EPlayerState.JUMPSTART);
            }
        }
        else if (_playerController.PlayerID == 2)
        {
            if (_playerController.CanJump && _playerController.IsGrounded())
            {
                _stateManager.ChangeStateP2(EPlayerState.JUMPSTART);
            }
        }
    }
}
