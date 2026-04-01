using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : APlayerState
{
    public override void Enter()
    {
        base.Enter();
        _playerController.CanJump = false;
        _playerController.Jump(_playerController.IsFullHop);
        _animator.SetBool("IsJumping", true);
    }

    public override void Exit()
    {
        _playerController.CanJump = true;
        _animator.SetBool("IsJumping", false);
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
        else if (!_playerController.IsGrounded())
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.AIRBASE);
        }
        _animator.SetBool("IsGrounded", _playerController.IsGrounded());
    }
}
