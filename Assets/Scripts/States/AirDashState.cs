using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDashState : APlayerState
{

    public override void Enter()
    {
        base.Enter();

        _playerController.IsFastFalling = false;
        _playerController.AirDash(_playerController.MovementInput);
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

        if (StateFrame >= 40)
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
