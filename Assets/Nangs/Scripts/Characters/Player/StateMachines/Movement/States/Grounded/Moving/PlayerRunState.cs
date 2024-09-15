using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerMovingState
{
    public PlayerRunState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        _stateMachine.playerStateReusableData.speedModifier = playerGroundedData.PlayerRunData.speedModifier;

        _stateMachine.playerStateReusableData.playerCurrentJumpForce = playerAerialData.PlayerJumpData.sprintJumpForce;

        AnimationStart(_stateMachine.PlayerControllerCustom.AnimationsData.IsRunningParameterHash);
    }

    public override void Update()
    {
        base.Update();
        CheckMovementDirection();
    }

    public override void Exit()
    {
        base.Exit();

        AnimationStop(_stateMachine.PlayerControllerCustom.AnimationsData.IsRunningParameterHash);
    }

    public override void OnAnimationTransitionEvent()
    {
        if (_stateMachine.playerStateReusableData.movementInput == Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.playerIdlingState);
            return;
        }
    }

    protected void CheckMovementDirection()
    {
        _stateMachine.playerStateReusableData.movementInput = _stateMachine.playerStateReusableData.movementInput.normalized;
        Debug.Log("Movement direction: " + _stateMachine.playerStateReusableData.movementInput);
        
        AnimationBlend(_stateMachine.PlayerControllerCustom.AnimationsData.rotationXBlendParameter, _stateMachine.playerStateReusableData.movementInput.x);
        AnimationBlend(_stateMachine.PlayerControllerCustom.AnimationsData.rotationYBlendParameter, _stateMachine.playerStateReusableData.movementInput.y);
    }

    #endregion

    #region Input Methods

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.PlayerDefaultStoppingState);
    }

    #endregion
}