using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : APlayerState
{
    public override void Enter()
    {
        base.Enter();
        DeathManager.Instance.ResetCooldown(_playerController.DeathCooldown);
        PhysicsCollisions.Instance.DeadCollisions(_playerController.PlayerID);
        _animator.SetBool("IsDead", true);
        FreezeFrameManager.Instance.StartCoroutine(FreezeFrameManager.Instance.Freeze(0.5f));
    }

    public override void Exit()
    {
        PhysicsCollisions.Instance.AliveCollisions(_playerController.PlayerID);
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
        base.Update();
        _playerController.DeathCooldown = DeathManager.Instance.UpdateCooldown(_playerController, _playerController.DeathCooldown);
        if (_playerController.DeathCooldown <= 0)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.IDLE);
        }
    }
}
