using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[Serializable]
public class PlayerCameraUtility
{
    [field: SerializeField] public CinemachineFreeLook CinemachineCamera { get; private set; }
    [field: SerializeField] public float DefaultHorizontalWaitTime { get; private set; } = 0f;
    [field: SerializeField] public float DefaultHorizontalRecenteringTime { get; private set; } = 4f;
    
    [field: SerializeField] public CinemachinePOV _cinemachinePov;

    public void Initialize()
    {
        //_cinemachinePov = CinemachineCamera.GetCinemachineComponent<CinemachinePOV>();
    }
    
    //public void EnableRecentering(float waitTime = -1f, float recenteringTime = -1f, float baseMovementSpeed = 1f, float movementSpeed = 1f)
    //{
    //    _cinemachinePov.m_HorizontalRecentering.m_enabled = true;
//
    //    _cinemachinePov.m_HorizontalRecentering.CancelRecentering();
//
    //    if (waitTime == -1f)
    //    {
    //        waitTime = DefaultHorizontalWaitTime;
    //    }
//
    //    if (recenteringTime == -1f)
    //    {
    //        recenteringTime = DefaultHorizontalRecenteringTime;
    //    }
//
    //    recenteringTime = recenteringTime * baseMovementSpeed / movementSpeed;
//
    //    _cinemachinePov.m_HorizontalRecentering.m_WaitTime = waitTime;
    //    _cinemachinePov.m_HorizontalRecentering.m_RecenteringTime = recenteringTime;
    //}
//
    //public void DisableRecentering()
    //{
    //    _cinemachinePov.m_HorizontalRecentering.m_enabled = false;
    //}
}
