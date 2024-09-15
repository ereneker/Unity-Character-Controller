using System;
using UnityEngine;

public class PlayerDefaultStoppingState : PlayerStoppingState
{
    public PlayerDefaultStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        _stateMachine.playerStateReusableData.decelerationForce =
            playerGroundedData.PlayerStopData.defaultDecelerationForce;

        _stateMachine.playerStateReusableData.playerCurrentJumpForce = playerAerialData.PlayerJumpData.sprintJumpForce;

        AnimationStart(_stateMachine.PlayerControllerCustom.AnimationsData.StoppingParameterHash);

    }

    public override void Exit()
    {
        base.Exit();

        Debug.Log("Exit");
        AnimationStop(_stateMachine.PlayerControllerCustom.AnimationsData.StoppingParameterHash);
    }
    
    public override void OnAnimationTransitionEvent()
    {
        if (_stateMachine.playerStateReusableData.movementInput == Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.playerIdlingState);
            return;
        }
    }

    #endregion

    #region Reusable Methods

    protected override void OnMove()
    {
        if (_stateMachine.playerStateReusableData.shouldWalk)
        {
            return;
        }

        _stateMachine.ChangeState(_stateMachine.PlayerRunState);
    }

    #endregion
    
    
}
