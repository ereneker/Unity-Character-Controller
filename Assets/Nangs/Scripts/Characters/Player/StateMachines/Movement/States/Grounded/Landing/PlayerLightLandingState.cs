using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightLandingState : PlayerLandingState
{
    public PlayerLightLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        _stateMachine.playerStateReusableData.speedModifier = 0f;
        
        ResetVelocity();
        
        AnimationStart(_stateMachine.PlayerControllerCustom.AnimationsData.LandingParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        
        AnimationStop(_stateMachine.PlayerControllerCustom.AnimationsData.LandingParameterHash);
    }

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
    
    public override void OnAnimationTransitionEvent()
    {
        _stateMachine.ChangeState(_stateMachine.playerIdlingState);
    }

    #endregion
}
