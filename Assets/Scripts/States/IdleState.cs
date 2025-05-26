using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : APlayerState
{
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Init(PlayerStateMachineManager stateManager, Animator animator)
    {
        _stateManager = stateManager;
    }

    public override void Update()
    {
        if (_stateManager.MovementInput.x != 0f)
        {
            _stateManager.ChangeState(EPlayerState.MOVE);
        }
        if (_stateManager.Attack)
        {
            _stateManager.ChangeState(EPlayerState.ATTACK1);
        }
    }
}
