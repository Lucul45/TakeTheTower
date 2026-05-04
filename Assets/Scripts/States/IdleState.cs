using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class IdleState : APlayerState
{
    public override void Enter()
    {
        base.Enter();
        _playerController.JumpPressed += Jump;
        _playerController.AttackPressed += Attack;
        _playerController.CanDoubleJump = true;
        _playerController.CanAirDash = false;
        _playerController.AirDashUsed = false;
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
        base.Update();

        // If the input isn't neutral
        if (_playerController.MovementInput.x != 0f)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.GROUNDSTART);
        }
        else if (!_playerController.IsGrounded())
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.AIRBASE);
        }
        _animator.SetBool("IsGrounded", _playerController.IsGrounded());
    }

    private void Attack()
    {
        if (_playerController.CanAttack)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.JAB);
        }
    }

    private void Jump()
    {
        if (_playerController.CanJump && _playerController.IsGrounded())
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.JUMPSTART);
        }
    }
}
