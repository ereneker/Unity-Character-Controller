using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintState : PlayerMovingState
{
    private PlayerSprintData _playerSprintData;

    private bool isSprinting;
    public PlayerSprintState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        _playerSprintData = playerGroundedData.PlayerSprintData;
    }

    public override void Enter()
    {
        base.Enter();

        _stateMachine.playerStateReusableData.speedModifier = _playerSprintData.speedModifier;
    }

    public override void Update()
    {
        base.Update();

        if (isSprinting)
        {
            return;
        }
        StopSprinting();
    }

    public override void Exit()
    {
        base.Exit();

        isSprinting = false;
    }

    #region Main Methods

    private void StopSprinting()
    {
        if (_stateMachine.playerStateReusableData.movementInput==Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.playerIdlingState);
            return;
        }

        _stateMachine.ChangeState(_stateMachine.PlayerRunState);
    }

    #endregion
    
    #region Reusable Methods

    protected override void AddInputActionsCallbacks()
    {
        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Sprint.performed += OnSprintPerformed;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Sprint.performed -= OnSprintPerformed;
    }

    #endregion

    #region Input Methods

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.PlayerDefaultStoppingState);
    }
    
    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        isSprinting = true;
    }

    #endregion
}
