using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APlayerState
{
    protected PlayerStateMachineManager _stateManager;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    public abstract void Init(PlayerStateMachineManager stateManager, Animator animator, SpriteRenderer spriteRenderer);

    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();
}
