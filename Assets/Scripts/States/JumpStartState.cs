using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStartState : APlayerState
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
        _playerController.IsFullHop = _playerController.IsFullHopping();
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
            if (StateFrameP1 >= 5)
            {
                _stateManager.ChangeStateP1(EPlayerState.JUMP);
            }
        }
        else if (_playerController.PlayerID == 2)
        {
            StateFrameP2++;
            if (StateFrameP2 >= 5)
            {
                _stateManager.ChangeStateP2(EPlayerState.JUMP);
            }
        }
        _animator.SetBool("IsGrounded", _playerController.IsGrounded());
    }
}
