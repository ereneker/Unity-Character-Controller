using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MoveIKTarget : PlayerGroundedState
{
    [SerializeField] private TwoBoneIKConstraint _ikConstraint;
    
    [SerializeField] private Transform headIK;
    [SerializeField] private Transform ribIK;
    [SerializeField] private Transform leftLegIK;
    [SerializeField] private Transform rightLegIK;
    
    public Vector3 MousePos { get; private set; }
    
    public MoveIKTarget(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    


    private void Start()
    {
        
    }

}
