using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput),typeof(Rigidbody),typeof(CapsuleCollider))]
public class PlayerControllerCustom : MonoBehaviour
{
    public Rigidbody RigidBody { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public Transform MainPlayerTransform { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    

    [field: Header("PlayerControllerCustom SO Reference")]
    [field: SerializeField] public PlayerSO playerData { get; private set; }
    
    
    
    [field: Header("PlayerControllerCustom Collider")]
    [field: SerializeField] public PlayerCapsuleColliderUtility PlayerCapsuleColliderUtility { get; private set; }
    [field: SerializeField] public PlayerLayerData PlayerLayerData { get; private set; }
    
    [field: Header("Camera")]
    [field: SerializeField] public PlayerCameraUtility CameraUtility { get; private set; }
    
    [field: Header("Animations")]
    [field: SerializeField] public PlayerAnimationsData AnimationsData { get; private set; }
    
    private PlayerMovementStateMachine _movementStateMachine;
    

#if UNITY_EDITOR
    private void OnValidate()
    {
        PlayerCapsuleColliderUtility.Initialize(gameObject);
        PlayerCapsuleColliderUtility.CalculateCapsuleColliderDimensions();
    }
#endif
    
    private void Awake()
    {
        CameraUtility.Initialize();
        RigidBody = GetComponent<Rigidbody>();
        PlayerInput = GetComponent<PlayerInput>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        
        _movementStateMachine = new PlayerMovementStateMachine(this);

        MainPlayerTransform = Camera.main.transform;
        
        PlayerCapsuleColliderUtility.Initialize(gameObject);
        PlayerCapsuleColliderUtility.CalculateCapsuleColliderDimensions();
        AnimationsData.Initialize();

    }

    private void Start()
    {
        _movementStateMachine?.ChangeState(_movementStateMachine.playerIdlingState);
    }

    private void OnTriggerEnter(Collider collider)
    {
        _movementStateMachine?.OnTriggerEnter(collider);
    }
    private void OnTriggerExit(Collider collider)
    {
        _movementStateMachine?.OnTriggerExit(collider);
    }

    private void Update()
    {
        _movementStateMachine?.HandleInput();
        _movementStateMachine?.Update();
        
        Debug.Log("Player Transform: " + MainPlayerTransform);
    }

    private void FixedUpdate()
    {
        _movementStateMachine?.PhysicsUpdate();
    }

    public void OnMovementStateAnimationEnterEvent()
    {
        _movementStateMachine?.OnAnimationEnterEvent();
    }
    
    public void OnMovementStateAnimationExitEvent()
    {
        _movementStateMachine?.OnAnimationExitEvent();
    }
    
    public void OnMovementStateAnimationTransitionEvent()
    {
        _movementStateMachine?.OnAnimationTransitionEvent();
    }
}
