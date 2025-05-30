using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : APlayerState
{
    private float _dashForce = 10f;
    public float _dashCooldown = 1f;
    private float _dashTime = 0.2f;
    private bool _canDash = true;
    private Vector2 _recordedInput = Vector2.zero;
    public override void Enter()
    {
        _recordedInput = _stateManager.RecordInput();
        _animator.SetBool("IsDashing", true);
    }

    public override void Exit()
    {
        _animator.SetBool("IsDashing", false);
    }

    public override void Init(PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rb)
    {
        _stateManager = stateManager;
        _animator = animator;
        _spriteRenderer = spriteRenderer;
        _rb = rb;
    }

    public override void Update()
    {
        if (_animator.GetBool("IsDashing") && _animator.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                _stateManager.ChangeState(EPlayerState.IDLE);
            }
            else if (_canDash)
            {
                
            }
        }
    }

    public IEnumerator Dash()
    {
        _canDash = false;
        _rb.velocity = new Vector2(_recordedInput.normalized.x * _dashForce, 0);
        yield return new WaitForSeconds(_dashTime);
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }
}
