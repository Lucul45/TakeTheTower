using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDashState : APlayerState
{
    // Durée fixe du wavedash en frames (comme dans Melee, souvent entre 10 et 15 frames)
    private int _wavedashFrames = 12;

    public override void Enter()
    {
        base.Enter();

        // On ignore la vélocité actuelle qui peut être corrompue par le sol
        // On définit une vitesse de glisse constante basée sur l'input
        float inputDirX = Mathf.Sign(_playerController.MovementInput.x);

        // On vérifie si le joueur maintient bien une direction latérale
        if (Mathf.Abs(_playerController.MovementInput.x) > 0.1f)
        {
            // On applique une vitesse brutale et propre
            // Utilise une valeur comme 18f ou 20f pour un wavedash "long"
            _rb.velocity = new Vector2(inputDirX * _playerController.WavedashLength, 0f);
        }
        else
        {
            // Wavedash sur place (Dead down)
            _rb.velocity = Vector2.zero;
        }

        // 2. Optionnel : Lancer l'animation de Wavedash (ou de Landing)
        //_animator.SetBool("IsWavedashing", true);

        // 3. On peut ajouter des particules de poussière au sol ici !
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

        // On applique la friction de glissade à chaque frame
        _playerController.WaveDashFriction();

        // Condition de sortie : soit le temps est écoulé, soit on a perdu presque toute notre vitesse
        if (StateFrame >= _wavedashFrames || Mathf.Abs(_rb.velocity.x) < 0.5f)
        {
            if (Mathf.Abs(_playerController.MovementInput.x) > 0.3f)
            {
                _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.RUN);
            }
            else
            {
                _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.IDLE);
            }
            return;
        }

        // Permettre de tomber d'une plateforme pendant le wavedash (Wavedrop / Edge slip)
        if (!_playerController.IsGrounded())
        {
            _stateManager.ChangeState(_playerController.PlayerID, EPlayerState.AIRBASE);
        }
    }
}
