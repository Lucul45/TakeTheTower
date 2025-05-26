using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : APlayerState
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
        if (_stateManager.MovementInput.x == 0)
        {
            _stateManager.ChangeState(EPlayerState.IDLE);
        }
        if (_stateManager.Attack)
        {
            _stateManager.ChangeState(EPlayerState.ATTACK1);
        }

        _stateManager.Move(_stateManager.MovementInput);
    }
}
