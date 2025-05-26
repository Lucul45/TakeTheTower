using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3State : MeleeBaseState
{
    public override void Enter()
    {
        _attackIndex = 3;
        _duration = 0.75f;
        _animator.SetBool("IsAttacking3", true);
    }

    public override void Exit()
    {
        _animator.SetBool("IsAttacking1", false);
        _animator.SetBool("IsAttacking2", false);
        _animator.SetBool("IsAttacking3", false);
        _stateManager.FixedTime = 0;
    }

    public override void Init(PlayerStateMachineManager stateManager, Animator animator)
    {
        _stateManager = stateManager;
        _animator = animator;
    }

    public override void Update()
    {
        _stateManager.Move(Vector2.zero);
    }
}
