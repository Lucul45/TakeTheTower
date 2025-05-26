using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APlayerState
{
    protected PlayerStateMachineManager _stateManager;
    protected Animator _animator;

    public abstract void Init(PlayerStateMachineManager stateManager, Animator animator);

    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();
}
