using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    private SlopeData slopeData;

    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        slopeData = _stateMachine.PlayerControllerCustom.PlayerCapsuleColliderUtility.slopeData;
    }

    #region IState Methods

    public override void Enter()
    {
        base.Enter();

        AnimationStart(_stateMachine.PlayerControllerCustom.AnimationsData.GroundedParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        AnimationStop(_stateMachine.PlayerControllerCustom.AnimationsData.GroundedParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        CapsuleFloat();
    }

    #endregion

    #region Main Methods

    protected void CapsuleFloat()
    {
        Vector3 capsuleColliderCenterInWorldSpace = _stateMachine.PlayerControllerCustom.PlayerCapsuleColliderUtility.capsuleColliderData
            .capsuleCollider.bounds.center;
        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);
        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, slopeData.floatRayDistance,_stateMachine.PlayerControllerCustom.PlayerLayerData.groundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);
            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (slopeSpeedModifier == 0f)
            {
                return;
            }
            
            float distanceToFloatingPoint = _stateMachine.PlayerControllerCustom.PlayerCapsuleColliderUtility.capsuleColliderData
                .colliderCenterInLocalSpace.y * _stateMachine.PlayerControllerCustom.transform.localScale.y - hit.distance;

            if (distanceToFloatingPoint == 0f)
            {
                return;
            }

            float amountToLift = distanceToFloatingPoint * slopeData.stepReachForce - GetPlayerVerticalVelocity().y;

            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);
            _stateMachine.PlayerControllerCustom.RigidBody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    private float SetSlopeSpeedModifierOnAngle(float angle)
    {
        float slopeSpeedModifier = playerGroundedData.SlopeSpeedAngles.Evaluate(angle);
        _stateMachine.playerStateReusableData.slopesSpeedModifier = slopeSpeedModifier;

        return slopeSpeedModifier;
    }

    private bool GroundUnderneathCheck()
    {
        BoxCollider boxCollider = _stateMachine.PlayerControllerCustom.PlayerCapsuleColliderUtility
            .PlayerTriggerColliderData.BoxCollider;
        
        Vector3 boxColliderCenterInWorldSpace = boxCollider.bounds.center;
        Vector3 boxColliderExtentsInWorldSpace = boxCollider.bounds.extents;

        Collider[] hitColliders = Physics.OverlapBox(boxColliderCenterInWorldSpace, boxColliderExtentsInWorldSpace,
            boxCollider.transform.rotation, _stateMachine.PlayerControllerCustom.PlayerLayerData.groundLayer,
            QueryTriggerInteraction.Ignore);

        return hitColliders.Length > 0;
    }
    
    #endregion
    
    #region Reusable Methods

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();

        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Movement.canceled += OnMovementCanceled;
        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Dash.started += OnDashStarted;
        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Jump.started += OnJumpStarted;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();

        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Movement.canceled -= OnMovementCanceled;
        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Dash.started -= OnDashStarted;
        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Jump.started -= OnJumpStarted;
    }

    protected virtual void OnMove()
    {
        _stateMachine.ChangeState(_stateMachine.PlayerRunState);
    }

    protected override void OnContactWithGroundOff(Collider collider)
    {
        base.OnContactWithGroundOff(collider);

        if (GroundUnderneathCheck())
        {
            return;
        }
        IsJumpDistanceFar();
    }
    
    private void IsJumpDistanceFar()
    {
        Vector3 capsuleColliderCenterInWorldSpace = _stateMachine.PlayerControllerCustom.PlayerCapsuleColliderUtility.capsuleColliderData
            .capsuleCollider.bounds.center;
        Ray downwardsRayFromCapsuleBottom =
            new Ray(
                capsuleColliderCenterInWorldSpace - _stateMachine.PlayerControllerCustom.PlayerCapsuleColliderUtility
                    .capsuleColliderData.colliderVerticalExtents, Vector3.down);

        if (!Physics.Raycast(downwardsRayFromCapsuleBottom,out _, playerGroundedData.distanceToGround, _stateMachine.PlayerControllerCustom.PlayerLayerData.groundLayer, QueryTriggerInteraction.Ignore))
        {
            OnFallStarted();
        }
    }

    protected void OnFallStarted()
    {
        _stateMachine.ChangeState(_stateMachine.PlayerFallingState);
    }

    #endregion

    #region Input Methods

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.playerIdlingState);
    }

    protected virtual void OnDashStarted(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.PlayerDashState);
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        _stateMachine.ChangeState(_stateMachine.PlayerJumpingState);
    }

    protected override void OnMovementPerformed(InputAction.CallbackContext obj)
    {
        base.OnMovementPerformed(obj);

        CameraRotation(GetMovementInputDirection());
    }

    #endregion
}