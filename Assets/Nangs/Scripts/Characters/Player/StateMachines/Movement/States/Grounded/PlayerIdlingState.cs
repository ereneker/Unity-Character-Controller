using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState
{
    public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        
    }

    #region IState Methods
    public override void Enter()
    {
        base.Enter();

        _stateMachine.playerStateReusableData.speedModifier = 0f;

        _stateMachine.playerStateReusableData.playerCurrentJumpForce = playerAerialData.PlayerJumpData.idleJumpForce;
        
        ResetVelocity();

        AnimationStart(_stateMachine.PlayerControllerCustom.AnimationsData.IsIdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        AnimationStop(_stateMachine.PlayerControllerCustom.AnimationsData.IsIdleParameterHash);
    }

    //Will add several random encounters when playerControllerCustom stay idle
    public override void Update()
    {
        base.Update();

        if (_stateMachine.playerStateReusableData.movementInput == Vector2.zero)
        {
            return;
        }
        
        OnMove();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!IsPlayerMovingHorizontally())
        {
            return;
        }
        
        ResetVelocity();
    }

    #endregion
}
