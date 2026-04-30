using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBaseState : APlayerState
{
    public override void Enter()
    {
        base.Enter();
        _playerController.CanAirDash = true;
        _playerController.JumpPressed += DoubleJump;
        _playerController.AirDashPressed += AirDash;
    }

    public override void Exit()
    {
        _playerController.JumpPressed -= DoubleJump;
        _playerController.AirDashPressed -= AirDash;
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

        if (Mathf.Abs(_playerController.MovementInput.x) <= 0.1f)
        {
            _playerController.AirFriction();
        }
        // if the player inputs down at the moment the character is at the top of its jump or less then you can fast fall
        if (_playerController.IsFastFalling)
        {
            _playerController.FastFall();
        }
        if (_playerController.IsGrounded())
        {
            _playerController.IsFastFalling = false;
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.IDLE);
        }
        else if (_playerController.MovementInput != Vector2.zero && (_playerController.PlayerID == 1 ? _stateManager.EnumCurrentStateP1 : _stateManager.EnumCurrentStateP2) != EPlayerState.AIRMOVE)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.AIRMOVE);
        }
        _animator.SetBool("IsGrounded", _playerController.IsGrounded());
    }

    private void DoubleJump()
    {
        if (_playerController.CanDoubleJump)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.AIRJUMP);
        }
    }

    private void AirDash()
    {
        if (_playerController.CanAirDash)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.AIRDASH);
        }
    }
}
