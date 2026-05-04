using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDashState : APlayerState
{
    private float _originalGravity;

    public override void Enter()
    {
        base.Enter();

        _playerController.IsFastFalling = false;
        _playerController.AirDashUsed = true;

        // COUPER LA GRAVITÉ : On stocke la gravité de base et on la met à 0
        // pour que l'airdash soit une ligne droite parfaite.
        _originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;

        _playerController.AirDash(_playerController.MovementInput);
    }

    public override void Exit()
    {
        // On remet la gravité à la normale en sortant de l'état
        _rb.gravityScale = _originalGravity;
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

        // 1. LA TRANSITION WAVEDASH (Vérifiée à CHAQUE frame)
        if (_playerController.IsGrounded() && _rb.velocity.y < -0.1f)
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.WAVEDASH);
            return; // Très important de return ici pour couper l'Update de l'AirDash
        }

        // 2. LA FIN DE L'AIRDASH NORMAL
        // 20 frames est une bonne moyenne pour un airdash agressif.
        if (StateFrame >= 10)
        {
            if (_playerController.IsGrounded())
            {
                _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.IDLE);
            }
            else
            {
                _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.AIRBASE);
            }
            // Reset of the velocity to make the dash more agressive
            _rb.velocity = Vector2.zero;
        }

        _animator.SetBool("IsGrounded", _playerController.IsGrounded());
    }
}
