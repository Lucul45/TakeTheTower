using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : APlayerState
{
    public override void Enter()
    {
        DeathManager.Instance.ResetCooldown(_playerController.DeathCooldown);
        if (_playerController.PlayerID == 1)
        {
            StateFrameP1 = 0;
            PhysicsCollisions.Instance.DeadCollisionsP1();
        }
        else if (_playerController.PlayerID == 2)
        {
            StateFrameP2 = 0;
            PhysicsCollisions.Instance.DeadCollisionsP2();
        }
        _animator.SetBool("IsDead", true);
    }

    public override void Exit()
    {
        if (_playerController.PlayerID == 1)
        {
            PhysicsCollisions.Instance.AliveCollisionsP1();
        }
        else if (_playerController.PlayerID == 2)
        {
            PhysicsCollisions.Instance.AliveCollisionsP2();
        }
        DeathManager.Instance.Respawn(_playerController, _playerHealth);
        _playerController.DeathCooldown = DeathManager.Instance.ResetCooldown(_playerController.DeathCooldown);
        _animator.SetBool("IsDead", false);
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
        _playerController.DeathCooldown = DeathManager.Instance.UpdateCooldown(_playerController, _playerController.DeathCooldown);
        if (_playerController.PlayerID == 1)
        {
            StateFrameP1++;
            if (_playerController.DeathCooldown <= 0)
            {
                _stateManager.ChangeStateP1(EPlayerState.IDLE);
            }
        }
        else if (_playerController.PlayerID == 2)
        {
            StateFrameP2++;
            if (_playerController.DeathCooldown <= 0)
            {
                _stateManager.ChangeStateP2(EPlayerState.IDLE);
            }
        }
    }
}
