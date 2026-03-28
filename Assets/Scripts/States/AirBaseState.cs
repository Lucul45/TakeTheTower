using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBaseState : APlayerState
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
        // if the player inputs down at the moment the character is at the top of its jump or less then you can fast fall
        if (_playerController.IsFastFalling)
        {
            _playerController.FastFall();
        }
        if (_playerController.PlayerID == 1)
        {
            StateFrameP1++;
            if (_playerHealth.CurrentHealth <= 0)
            {
                _stateManager.ChangeStateP1(EPlayerState.DEAD);
            }
            else if (_playerController.IsGrounded())
            {
                _playerController.IsFastFalling = false;
                _stateManager.ChangeStateP1(EPlayerState.IDLE);
            }
            else if (_playerController.MovementInput != Vector2.zero && _stateManager.EnumCurrentStateP1 != EPlayerState.AIRMOVE)
            {
                _stateManager.ChangeStateP1(EPlayerState.AIRMOVE);
            }
        }
        else if (_playerController.PlayerID == 2)
        {
            StateFrameP2++;
            if (_playerHealth.CurrentHealth <= 0)
            {
                _stateManager.ChangeStateP2(EPlayerState.DEAD);
            }
            else if (_playerController.IsGrounded())
            {
                _playerController.IsFastFalling = false;
                _stateManager.ChangeStateP2(EPlayerState.IDLE);
            }
            else if (_playerController.MovementInput != Vector2.zero && _stateManager.EnumCurrentStateP2 != EPlayerState.AIRMOVE)
            {
                _stateManager.ChangeStateP2(EPlayerState.AIRMOVE);
            }
        }
        _animator.SetBool("IsGrounded", _playerController.IsGrounded());
    }
}
