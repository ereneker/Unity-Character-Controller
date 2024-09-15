using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementState : IState
{
    protected PlayerMovementStateMachine _stateMachine;

    protected PlayerGroundedData playerGroundedData;
    protected PlayerAerialData playerAerialData;

    public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
    {
        _stateMachine = playerMovementStateMachine;

        playerGroundedData = _stateMachine.PlayerControllerCustom.playerData.PlayerGroundedData;
        playerAerialData = _stateMachine.PlayerControllerCustom.playerData.PlayerAerialData;
        
        InitializeData();
    }

    private void InitializeData()
    {
        SetBaseCameraRecenteringData();
        SetBaseRotationData();
    }

    #region State Methods

    public virtual void Enter()
    {
#if UNITY_EDITOR
        Debug.Log("State: " + GetType().Name);
#endif
        
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void Update()
    {
    }

    public virtual void PhysicsUpdate()
    {
        Move();
    }

    public virtual void OnAnimationEnterEvent()
    {
        
    }

    public virtual void OnAnimationExitEvent()
    {
        
    }

    public virtual void OnAnimationTransitionEvent()
    {
        
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        if (_stateMachine.PlayerControllerCustom.PlayerLayerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGround(collider);
            
            return;
        }
    }

    public virtual void OnTriggerExit(Collider collider)
    {
        if (_stateMachine.PlayerControllerCustom.PlayerLayerData.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGroundOff(collider);
            
            return;
        }
    }

    #endregion

    #region Input Methods

    private void Move()
    {
        if (_stateMachine.playerStateReusableData.movementInput == Vector2.zero || _stateMachine.playerStateReusableData.speedModifier == 0)
        {
            return;
        }

        _stateMachine.playerStateReusableData.movementDirection = GetMovementInputDirection();

        float targetRotationAngle = CameraRotate(_stateMachine.playerStateReusableData.movementDirection);

        Vector3 targetRotation = GetTargetRotation(targetRotationAngle);
        
        float _movementSpeed = GetMovementSpeed();
        _stateMachine.playerStateReusableData.currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
        _stateMachine.PlayerControllerCustom.RigidBody.AddForce(targetRotation * _movementSpeed - _stateMachine.playerStateReusableData.currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
    }

    private void ReadMovementInput()
    {
        _stateMachine.playerStateReusableData.movementInput = _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Movement.ReadValue<Vector2>();
    }

    private float CameraRotate(Vector3 direction)
    {
        float directionAngle = CameraRotation(direction);
        
        RotateTowardsTarget();

        return directionAngle;
    }
    
    protected float CameraRotation(Vector3 direction, bool isCameraRotationActive = true)
    {
        float directionAngle = GetDirectionAngle(direction);

        if (isCameraRotationActive)
        {
            directionAngle = GetCameraRotation(directionAngle);
        }
        
        if (directionAngle != _stateMachine.playerStateReusableData.currentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }
        
        return directionAngle;
    }

    #endregion

    #region Reusable Methods

    protected void AnimationStart(int animationHash)
    {
        _stateMachine.PlayerControllerCustom.PlayerAnimator.SetBool(animationHash, true);
    }
    
    protected void AnimationStop(int animationHash)
    {
        _stateMachine.PlayerControllerCustom.PlayerAnimator.SetBool(animationHash, false);
    }

    protected void AnimationBlend(string parameterName ,float animationHash)
    {
        _stateMachine.PlayerControllerCustom.PlayerAnimator.SetFloat(parameterName ,animationHash);
    }
    
    protected Vector3 GetPlayerHorizontalVelocity()
    {
        _stateMachine.playerStateReusableData.playerHorizontalVelocity = _stateMachine.PlayerControllerCustom.RigidBody.velocity;
        _stateMachine.playerStateReusableData.playerHorizontalVelocity.y = 0f;
        return _stateMachine.playerStateReusableData.playerHorizontalVelocity;
    }

    protected Vector3 GetPlayerVerticalVelocity()
    {
        return new Vector3(0f, _stateMachine.PlayerControllerCustom.RigidBody.velocity.y, 0f);
    }
    
    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(_stateMachine.playerStateReusableData.movementInput.x, 0f, _stateMachine.playerStateReusableData.movementInput.y);
    }

    protected float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        if (directionAngle < 0)
        {
            directionAngle += 360f;
        }

        return directionAngle;
    }

    protected float GetCameraRotation(float directionAngle)
    {
        directionAngle += _stateMachine.PlayerControllerCustom.MainPlayerTransform.eulerAngles.y;

        if (directionAngle > 360f)
        {
            directionAngle -= 360;
        }

        return directionAngle;
    }

    protected void RotateTowardsTarget()
    {
        float currentPlayerAngle = _stateMachine.PlayerControllerCustom.RigidBody.rotation.eulerAngles.y;

        if (currentPlayerAngle == _stateMachine.playerStateReusableData.currentTargetRotation.y)
        {
            return;
        }

        float smoothedPlayerAngle = Mathf.SmoothDampAngle(currentPlayerAngle,
            _stateMachine.playerStateReusableData.currentTargetRotation.y,
            ref _stateMachine.playerStateReusableData.dampedTargetRotationCurrentVelocity.y,
            _stateMachine.playerStateReusableData.timeToReachTargetRotation.y -
            _stateMachine.playerStateReusableData.dampedTargetRotationPassedTime.y);

        _stateMachine.playerStateReusableData.dampedTargetRotationPassedTime.y += Time.deltaTime;

        Quaternion targetRotation = Quaternion.Euler(0, smoothedPlayerAngle, 0);

        if (Mathf.Abs(_stateMachine.playerStateReusableData.movementInput.x) != 1)
        {
            _stateMachine.PlayerControllerCustom.RigidBody.MoveRotation(targetRotation);
        }

       
    }
    
    protected Vector3 GetTargetRotation(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    protected void UpdateTargetRotationData(float targetRotation)
    {
        _stateMachine.playerStateReusableData.currentTargetRotation.y = targetRotation;

        _stateMachine.playerStateReusableData.dampedTargetRotationPassedTime.y = 0f;
    }

    protected void ResetVelocity()
    {
        _stateMachine.PlayerControllerCustom.RigidBody.velocity = Vector3.zero;
    }

    protected void ResetVerticalVelocity()
    {
        Vector3 GetHorizontalVelocity = GetPlayerHorizontalVelocity();

        _stateMachine.PlayerControllerCustom.RigidBody.velocity = GetHorizontalVelocity;
    }
    
    protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
    {
        float movementSpeed = playerGroundedData.baseSpeed * _stateMachine.playerStateReusableData.speedModifier;

        if (shouldConsiderSlopes)
        {
            movementSpeed *= _stateMachine.playerStateReusableData.slopesSpeedModifier;
        }

        return movementSpeed;
    }

    protected virtual void AddInputActionsCallbacks()
    {
        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.MouseLook.started += OnMouseMovementStarted;
        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Movement.performed += OnMovementPerformed;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.MouseLook.started -= OnMouseMovementStarted;
        _stateMachine.PlayerControllerCustom.PlayerInput.playerActions.Movement.performed -= OnMovementPerformed;
    }

    protected virtual void OnMovementPerformed(InputAction.CallbackContext obj)
    {
        UpdateCameraRecentering(obj.ReadValue<Vector2>());
    }
    
    private void OnMouseMovementStarted(InputAction.CallbackContext obj)
    {
        UpdateCameraRecentering(_stateMachine.playerStateReusableData.movementInput);
    }
    
    protected void UpdateCameraRecentering(Vector2 movementInput)
    {
        if (movementInput == Vector2.zero)
        {
            return;
        }

        if (movementInput == Vector2.up)
        {
            DisableCameraRecentering();
            return;
        }

        float cameraVerticleAngle = _stateMachine.PlayerControllerCustom.MainPlayerTransform.eulerAngles.x;

        if (cameraVerticleAngle >= 270f)
        {
            cameraVerticleAngle -= 360f;
        }

        cameraVerticleAngle = Mathf.Abs(cameraVerticleAngle);

        if (movementInput == Vector2.down)
        {
            SetCameraRecenteringState(cameraVerticleAngle,_stateMachine.playerStateReusableData.BackwardsCameraRecenteringData);
            
            return;
        }
        
        SetCameraRecenteringState(cameraVerticleAngle, _stateMachine.playerStateReusableData.SidewaysCameraRecenteringData);
        
    }

    protected void SetCameraRecenteringState(float cameraVerticalAngle, List<PlayerCameraRecenteringData> cameraRecenteringData)
    {
        foreach (var recenteringData in cameraRecenteringData)
        {
            if (!recenteringData.IsWithinRange(cameraVerticalAngle))
            {
                continue;
            }

            EnableCameraRecentering(recenteringData.WaitTime, recenteringData.RecenteringTime);
            
            return;
        }
        
        DisableCameraRecentering();
    }
    
    protected void EnableCameraRecentering(float waitTime = -1f, float recenteringTime = -1f)
    {
        float movementSpeed = GetMovementSpeed();

        if (movementSpeed == 0f)
        {
            movementSpeed = playerGroundedData.baseSpeed;
        }

       // _stateMachine.PlayerControllerCustom.CameraUtility.EnableRecentering(waitTime, recenteringTime, playerGroundedData.baseSpeed, movementSpeed);
    }
    
    protected void DisableCameraRecentering()
    {
        //_stateMachine.PlayerControllerCustom.CameraUtility.DisableRecentering();
    }

    protected void DecelerateHorizontally()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        _stateMachine.PlayerControllerCustom.RigidBody.AddForce(
            -playerHorizontalVelocity * _stateMachine.playerStateReusableData.decelerationForce,
            ForceMode.Acceleration);
    }

    protected void DecelerateVertically()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

        _stateMachine.PlayerControllerCustom.RigidBody.AddForce(
            -playerVerticalVelocity * _stateMachine.playerStateReusableData.decelerationForce,
            ForceMode.Acceleration);
    }
    
    protected bool IsPlayerMovingHorizontally(float minimumMagnitude = 0.1f)
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);

        return playerHorizontalMovement.magnitude > minimumMagnitude;
    }

    protected virtual void OnContactWithGround(Collider collider) {}
    protected virtual void OnContactWithGroundOff(Collider collider) {}

    protected bool IsMovingUp(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y > minimumVelocity;
    }
    
    protected bool IsMovingDown(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y < -minimumVelocity;
    }

    protected void SetBaseRotationData()
    {
        _stateMachine.playerStateReusableData.PlayerRotationData = playerGroundedData.PlayerRotationData;
        _stateMachine.playerStateReusableData.timeToReachTargetRotation = _stateMachine.playerStateReusableData.PlayerRotationData.targetRotationReachTime;
    }

    protected void SetBaseCameraRecenteringData()
    {
        _stateMachine.playerStateReusableData.SidewaysCameraRecenteringData = playerGroundedData.SidewaysCameraRecenteringData;
        _stateMachine.playerStateReusableData.BackwardsCameraRecenteringData = playerGroundedData.BackwardsCameraRecenteringData;
    }
    
    #endregion

    
}
