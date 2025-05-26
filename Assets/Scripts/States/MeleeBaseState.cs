using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBaseState : APlayerState
{
    protected bool _shouldCombo = false;
    protected int _attackIndex;
    protected float _duration;

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Init(PlayerStateMachineManager stateManager, Animator animator)
    {
        _stateManager = stateManager;
        _animator = animator;
    }

    public override void Update()
    {
        if (_stateManager.Attack)
        {
            _shouldCombo = true;
        }
    }
}
