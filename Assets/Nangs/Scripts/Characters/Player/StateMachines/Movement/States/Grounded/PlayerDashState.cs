using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashState : PlayerGroundedState
{
    private PlayerDashData _playerDashData;

    private float startTime;

    private int consecutiveDashUsed;
    
    public PlayerDashState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        _playerDashData = playerGroundedData.PlayerDashData;
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        _stateMachine.playerStateReusableData.speedModifier = _playerDashData.speedModifier;
        
        _stateMachine.playerStateReusableData.playerCurrentJumpForce = playerAerialData.PlayerJumpData.dashJumpForce;
        
        AddForceOnIdlingToDash();
        UpdateConsecutiveDashes();

        startTime = Time.time;

        AnimationStart(_stateMachine.PlayerControllerCustom.AnimationsData.SlidingParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        AnimationStop(_stateMachine.PlayerControllerCustom.AnimationsData.SlidingParameterHash);
    }

    public override void OnAnimationTransitionEvent()
    {
        if (_stateMachine.playerStateReusableData.movementInput == Vector2.zero)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerDefaultStoppingState);
            return;
        }

        _stateMachine.ChangeState(_stateMachine.PlayerRunState);
    }

    #endregion

    #region Main Methods

    private void AddForceOnIdlingToDash()
    {
        if (_stateMachine.playerStateReusableData.movementInput != Vector2.zero)
        {
            return;
        }

        Vector3 characterDirection = _stateMachine.PlayerControllerCustom.transform.forward;
        characterDirection.y = 0f;

        _stateMachine.PlayerControllerCustom.RigidBody.velocity = characterDirection * GetMovementSpeed();
    }

    private void UpdateConsecutiveDashes()
    {
        if (!IsConsecutive())
        {
            consecutiveDashUsed = 0;
        }

        ++consecutiveDashUsed;

        if (consecutiveDashUsed == _playerDashData.consecutiveDashLimit)
        {
            consecutiveDashUsed = 0;
            _stateMachine.PlayerControllerCustom.PlayerInput.DisableAnyAction(_stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Dash,
                _playerDashData.dashLimitCooldown).Forget();
        }
    }

    private bool IsConsecutive()
    {
#if UNITY_EDITOR
        Debug.Log("Time: " + Time.time);
#endif
        return Time.time < startTime + _playerDashData.dashLimitReachedCooldown;
    }
    
    #endregion

    #region Input Methods

    protected override void OnMovementCanceled(InputAction.CallbackContext context) {}

    //Parent class does necessary state changes. This method is to avoid doing unnecessary method call.
    protected override void OnDashStarted(InputAction.CallbackContext context) {}

    #endregion
}
