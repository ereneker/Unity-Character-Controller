using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnimationsData
{
    /////////////////////////////////
    /// <State parameter strings  ///
    /// in animator and           /// 
    /// Converting them to Hash>  /// 
    /////////////////////////////////
    
    [Header("State Parameter Strings")]
    [SerializeField] private string groundedStateParameter = "Grounded";
    [SerializeField] private string airborneStateParameter = "Airborne";
    [SerializeField] private string movingStateParameter = "Moving";
    [SerializeField] private string stoppingStateParameter = "Stopping";
    [SerializeField] private string slidingStateParameter = "Sliding";
    [SerializeField] private string landingStateParameter = "Landing";

    [Header("Sub-State Parameter Strings")] 
    [SerializeField] private string isSprintingStateParameter = "isSprinting";
    [SerializeField] private string isRunningStateParameter = "isRunning";
    [SerializeField] private string isSlidingToStopStateParameter = "isSlidingToStop";
    [SerializeField] private string isHardLandingStateParameter = "isHardLanding";
    [SerializeField] private string isIdleStateParameter = "isIdle";
    [SerializeField] private string isFallingStateParameter = "isFalling";
    [SerializeField] private string isRunningLeftParameter = "isRunningLeft";
    [SerializeField] private string isRunningRightParameter = "isRunningRight";
    [SerializeField] private string isRunningForwardParameter = "isRunningForward";
    [SerializeField] private string isRunningBackwardParameter = "isRunningBackward";
    
    
    public string rotationXBlendParameter = "RotationX";
    public string rotationYBlendParameter = "RotationY";

    [field: Header("State/Substate Parameter Hash")] 
    public int GroundedParameterHash { get; private set; }
    public int AirborneParameterHash { get; private set; }
    public int MovingParameterHash   { get; private set; }
    public int StoppingParameterHash { get; private set; }
    public int SlidingParameterHash  { get; private set; }
    public int LandingParameterHash  { get; private set; }
    
    public int IsSprintingParameterHash         { get; private set; }
    public int IsRunningParameterHash           { get; private set; }
    public int IsSlidingToStopParameterHash     { get; private set; }
    public int IsHardLandingParameterHash       { get; private set; }
    public int IsIdleParameterHash              { get; private set; }
    public int IsFallingParameterHash           { get; private set; }

    
    
    public void Initialize()
    {
        GroundedParameterHash = Animator.StringToHash(groundedStateParameter);
        AirborneParameterHash = Animator.StringToHash(airborneStateParameter);
        MovingParameterHash = Animator.StringToHash(movingStateParameter);
        StoppingParameterHash = Animator.StringToHash(stoppingStateParameter);
        SlidingParameterHash = Animator.StringToHash(slidingStateParameter);
        LandingParameterHash = Animator.StringToHash(landingStateParameter);
        IsSprintingParameterHash = Animator.StringToHash(isSprintingStateParameter);
        IsRunningParameterHash = Animator.StringToHash(isRunningStateParameter);
        IsSlidingToStopParameterHash = Animator.StringToHash(isSlidingToStopStateParameter);
        IsHardLandingParameterHash = Animator.StringToHash(isHardLandingStateParameter);
        IsIdleParameterHash = Animator.StringToHash(isIdleStateParameter);
    }
}
