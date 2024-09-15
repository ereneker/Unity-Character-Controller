using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingState : PlayerGroundedState
{
    public PlayerMovingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AnimationStart(_stateMachine.PlayerControllerCustom.AnimationsData.MovingParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        
        AnimationStop(_stateMachine.PlayerControllerCustom.AnimationsData.MovingParameterHash);
    }
}
