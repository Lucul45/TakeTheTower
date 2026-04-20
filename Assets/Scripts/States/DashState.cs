using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : APlayerState
{
    private float _dashDirection = 0;
    public override void Enter()
    {
        base.Enter();

        _playerController.JumpPressed += Jump;

        // reset the velocity
        _rb.velocity = Vector2.zero;

        _playerController.Dash(_playerController.TempMovementInput);
    }

    public override void Exit()
    {
        
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

        // if the joystick changes direction
        if ((_playerController.TempMovementInput.x <= -0.8f && _playerController.MovementInput.x >= 0.8f) || (_playerController.TempMovementInput.x >= 0.8f && _playerController.MovementInput.x <= -0.8f))
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.GROUNDSTART);
        }
        else if (StateFrame >= 15)
        {
            // if the joystick is still in the same direction
            if ((_playerController.TempMovementInput.x <= -0.8f && _playerController.MovementInput.x <= -0.8f) || (_playerController.TempMovementInput.x >= 0.8f && _playerController.MovementInput.x >= 0.8f))
            {
                _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.RUN);
            }
            else if (_playerController.MovementInput.x == 0)
            {
                _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.IDLE);
            }
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
