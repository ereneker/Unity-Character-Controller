using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerAerialState
{
    private PlayerJumpData _playerJumpData;
    private bool isStartedFalling = false;
    
    public bool shouldRotate = true;
    

    public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        _playerJumpData = playerAerialData.PlayerJumpData;
    }

    #region IState Methods
    
    public override void Enter()
    {
        base.Enter();

        //_stateMachine.playerStateReusableData.speedModifier = 0f;

        shouldRotate = false;

        _stateMachine.playerStateReusableData.decelerationForce = _playerJumpData.decelerationForce;
        
        PlayerJump();
        
    }

    public override void Update()
    {
        base.Update();

        if (!isStartedFalling && IsMovingUp(0f))
        {
            isStartedFalling = true;
        }

        if (!isStartedFalling || GetPlayerVerticalVelocity().y > 0)
        {
            return;
        }

        _stateMachine.ChangeState(_stateMachine.PlayerFallingState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (IsMovingUp())
        {
            DecelerateVertically();
        }
    }

    public override void Exit()
    {
        base.Exit();

        shouldRotate = true;
        isStartedFalling = false;
    }

    #endregion

    #region Main Methods

    private void PlayerJump()
    {
        Vector3 playerJumpForce = _stateMachine.playerStateReusableData.playerCurrentJumpForce;

        Vector3 playerTransformForward = _stateMachine.PlayerControllerCustom.transform.forward;

        playerJumpForce.x *= playerTransformForward.x;
        playerJumpForce.z *= playerTransformForward.z;

        playerJumpForce = PlayerJumpOnSlope(playerJumpForce);
        
        ResetVelocity();
        
        _stateMachine.PlayerControllerCustom.RigidBody.AddForce(playerJumpForce, ForceMode.VelocityChange);
    }

    private Vector3 PlayerJumpOnSlope(Vector3 playerJumpForce)
    {
        Vector3 capsuleColliderCenterInWorldSpace = _stateMachine.PlayerControllerCustom.PlayerCapsuleColliderUtility.capsuleColliderData
            .capsuleCollider.bounds.center;
        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, _playerJumpData.jumpToSlopeRayDistance ,_stateMachine.PlayerControllerCustom.PlayerLayerData.groundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);
            Debug.Log("Ground Angle: " + groundAngle);

            if (IsMovingUp())
            {
                float forceModifier = _playerJumpData.JumpModifierOnSlopeUp.Evaluate(groundAngle);

                playerJumpForce.x *= forceModifier;
                playerJumpForce.z *= forceModifier;
            }

            if (IsMovingDown())
            {
                float forceModifier = _playerJumpData.JumpModifierOnSlopeDown.Evaluate(groundAngle);

                playerJumpForce.y *= forceModifier;
            }
        }

        return playerJumpForce;
    }
    
    #endregion
}
