using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEntryState : APlayerState
{
    public override void Enter()
    {
        //if is grounded
        _stateManager.ChangeState(EPlayerState.MELEE);
    }

    public override void Exit()
    {
        
    }

    public override void Init(PlayerStateMachineManager stateManager, Animator animator)
    {
        
    }

    public override void Update()
    {
        
    }
}
