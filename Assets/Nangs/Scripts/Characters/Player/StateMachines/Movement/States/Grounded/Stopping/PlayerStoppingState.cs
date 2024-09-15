using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStoppingState : PlayerGroundedState
{
    public PlayerStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        _stateMachine.playerStateReusableData.speedModifier = 0f;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!IsPlayerMovingHorizontally())
        {
            return;
        }
        
        DecelerateHorizontally();
    }

    public override void OnAnimationEnterEvent()
    {
        _stateMachine.ChangeState(_stateMachine.playerIdlingState);
    }

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        
        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Movement.started += OnMovementStarted;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        
        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Movement.started -= OnMovementStarted;
    }

    

    #endregion

    #region Input Methods

    protected override void OnMovementCanceled(InputAction.CallbackContext context) {}

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }
    
    #endregion
}
