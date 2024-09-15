using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventTrigger : MonoBehaviour
{
    private PlayerControllerCustom _playerController;

    private void Awake()
    {
        _playerController = transform.parent.GetComponent<PlayerControllerCustom>();
    }

    public void TriggerMovementAnimationEnter()
    {
        _playerController?.OnMovementStateAnimationEnterEvent();
    }

    public void TriggerMovementAnimationExit()
    {
        _playerController?.OnMovementStateAnimationExitEvent();
    }

    public void TriggerMovementAnimationTransition()
    {
        _playerController?.OnMovementStateAnimationTransitionEvent();
    }
}
