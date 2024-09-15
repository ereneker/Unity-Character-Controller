using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateReusableData
{
    public bool shouldWalk { get; set; } = false;
    
    public Vector2 movementInput { get; set; }
    public Vector2 playerCurrentJumpForce { get; set; }
    public float speedModifier { get; set; } = 1f;
    public float slopesSpeedModifier { get; set; } = 1f;
    public float decelerationForce { get; set; } = 1f;

    public List<PlayerCameraRecenteringData> SidewaysCameraRecenteringData { get; set; }
    public List<PlayerCameraRecenteringData> BackwardsCameraRecenteringData { get; set; }
    
    private Vector3 _playerHorizontalVelocity;
    private Vector3 _movementDirection;
    private Vector3 _currentPlayerHorizontalVelocity;
    private Vector3 _currentTargetRotation;
    private Vector3 _timeToReachTargetRotation;
    private Vector3 _dampedTargetRotationCurrentVelocity;
    private Vector3 _dampedTargetRotationPassedTime;

    public ref Vector3 playerHorizontalVelocity
    {
        get
        {
            return ref _playerHorizontalVelocity;
        }
    }

    public ref Vector3 movementDirection
    {
        get
        {
            return ref _movementDirection;
        }
    }

    public ref Vector3 currentPlayerHorizontalVelocity
    {
        get
        {
            return ref _currentPlayerHorizontalVelocity;
        }
    }

    public ref Vector3 currentTargetRotation
    {
        get
        {
            return ref _currentTargetRotation;
        }
    }
    
    public ref Vector3 timeToReachTargetRotation
    {
        get
        {
            return ref _timeToReachTargetRotation;
        }
    }

    public ref Vector3 dampedTargetRotationCurrentVelocity
    {
        get
        {
            return ref _dampedTargetRotationCurrentVelocity;
        }
    }
    public ref Vector3 dampedTargetRotationPassedTime
    {
        get
        {
            return ref _dampedTargetRotationPassedTime;
        }
    }
    
    public PlayerRotationData PlayerRotationData { get; set; }
}
