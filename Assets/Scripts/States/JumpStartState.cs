using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStartState : APlayerState
{
    public override void Enter()
    {
        base.Enter();

        _playerController.CanAirDash = true;
        _playerController.AirDashPressed += AirDash;
    }

    public override void Exit()
    {
        _playerController.IsFullHop = _playerController.IsFullHopping();
        _playerController.CanAirDash = false;
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

        if (StateFrame >= 5)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.JUMP);
        }
        _animator.SetBool("IsGrounded", _playerController.IsGrounded());
    }

    private void AirDash()
    {
        if (_playerController.CanAirDash && !_playerController.AirDashUsed)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.AIRDASH);
        }
    }
}
