using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnaroundState : APlayerState
{
    public override void Enter()
    {
        base.Enter();

        _playerController.JumpPressed += Jump;
        _playerController.ReverseRotation();
        _animator.SetBool("IsTurnarounding", true);
    }

    public override void Exit()
    {
        _animator.SetBool("IsTurnarounding", false);
        _playerController.JumpPressed -= Jump;
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

        _playerController.Turnaround(_playerController.MovementInput);

        if (_playerHealth.CurrentHealth <= 0)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.DEAD);
        }
        if (StateFrame >= _animator.GetCurrentAnimatorStateInfo(0).length * 60)
        {
            // if we don't move, change to idle
            if (_playerController.MovementInput.x == 0)
            {
                _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.IDLE);
            }
            else if (_playerController.MovementInput.x != 0)
            {
                _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.RUN);
            }
        }
        _animator.SetBool("IsGrounded", _playerController.IsGrounded());
    }

    private void Jump()
    {
        if (_playerController.CanJump && _playerController.IsGrounded())
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.JUMPSTART);
        }
    }
}
