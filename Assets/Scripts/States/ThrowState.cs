using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowState : APlayerState
{
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
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
        
    }
}
