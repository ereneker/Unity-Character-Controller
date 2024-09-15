using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerGroundedData
{
    [field: SerializeField] [field: Range(0f, 25f)] public float baseSpeed { get; private set; } = 5f;
    [field: SerializeField] [field: Range(0f, 5f)] public float distanceToGround { get; private set; } = 1f;
    [field: SerializeField] public List<PlayerCameraRecenteringData> SidewaysCameraRecenteringData { get; private set; }
    [field: SerializeField] public List<PlayerCameraRecenteringData> BackwardsCameraRecenteringData { get; private set; }
    [field: SerializeField] public PlayerRotationData PlayerRotationData { get; private set; }
    [field: SerializeField] public PlayerDashData PlayerDashData { get; private set; }
    [field: SerializeField] public PlayerRunData PlayerRunData { get; private set; }
    [field: SerializeField] public PlayerSprintData PlayerSprintData { get; private set; }
    [field: SerializeField] public PlayerStopData PlayerStopData { get; private set; }
    [field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }
}
