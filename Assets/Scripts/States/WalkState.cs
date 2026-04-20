using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WalkState : APlayerState
{
    public override void Enter()
    {
        base.Enter();

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
        base.Update();

        if (_playerHealth.CurrentHealth <= 0)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.DEAD);
        }
        // if we don't move, change to idle
        else if (_playerController.MovementInput.x == 0)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.IDLE);
        }
        // if the velocity direction isn't the same as the input direction
        else if ((_rb.velocity.x < -0.1f && _playerController.MovementInput.x > 0.8f) || (_rb.velocity.x > 0.1f && _playerController.MovementInput.x < -0.8f))
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.GROUNDSTART);
        }
        _animator.SetBool("IsGrounded", _playerController.IsGrounded());

        _playerController.Walk(_playerController.MovementInput);
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
