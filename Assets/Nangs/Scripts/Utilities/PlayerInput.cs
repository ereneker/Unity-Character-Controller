using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions inputActions { get; private set; }
    public PlayerInputActions.PlayerActions playerActions { get; private set; }

    private CancellationTokenSource _cts;
    private CancellationToken _token;

    private void Awake()
    {
        _cts = new CancellationTokenSource();
        _token = _cts.Token;
        inputActions = new PlayerInputActions();
        playerActions = inputActions.Player;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
        _cts.Cancel();
    }
    
    public async UniTaskVoid DisableAnyAction(InputAction action, float seconds)
    {
        action.Disable();
        
        _token.ThrowIfCancellationRequested();
        await UniTask.Delay(TimeSpan.FromSeconds(seconds), cancellationToken: _token);
        action.Enable();
    }
    
}
