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

    public override void Init(PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer, Rigidbody2D rb, WinMenuManager winManager)
    {
        _stateManager = stateManager;
        _animator = animator;
        _spriteRenderer = spriteRenderer;
        _rb = rb;
        _winManager = winManager;
    }

    public override void Update()
    {
        
    }
}
