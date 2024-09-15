using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerAerialState
{
    private PlayerFallData _playerFallData;

    private Vector3 _playerPositionOnEnter;
    
    public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        _playerFallData = playerAerialData.PlayerFallData;
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();
        
        AnimationStart(_stateMachine.PlayerControllerCustom.AnimationsData.IsFallingParameterHash);

        _playerPositionOnEnter = _stateMachine.PlayerControllerCustom.transform.position;
        _stateMachine.playerStateReusableData.speedModifier = 0f;
        ResetVerticalVelocity();

    }

    public override void Exit()
    {
        base.Exit();

        AnimationStop(_stateMachine.PlayerControllerCustom.AnimationsData.IsFallingParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        LimitFallSpeed();
    }

    #endregion

    #region Reusable Methods

    protected override void OnContactWithGround(Collider collider)
    {
        float distanceToGround =
            Mathf.Abs(_playerPositionOnEnter.y - _stateMachine.PlayerControllerCustom.transform.position.y);
        if (distanceToGround < _playerFallData.MinimumDistToHardFall)
        {
            _stateMachine.ChangeState(_stateMachine.PlayerLightLandingState);
        }
    }

    #endregion

    #region Main Methods

    private void LimitFallSpeed()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

        if (playerVerticalVelocity.y >= -_playerFallData.fallSpeedLimit)
        {
            return;
        }

        Vector3 velocityLimit = new Vector3(0f, -_playerFallData.fallSpeedLimit - playerVerticalVelocity.y, 0f);

        _stateMachine.PlayerControllerCustom.RigidBody.AddForce(velocityLimit);
    }

    #endregion
}
