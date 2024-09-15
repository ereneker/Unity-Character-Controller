using System;
using UnityEngine;

[Serializable]
public class PlayerJumpData
{
    [field: SerializeField] [field: Range(0f, 5f)] public float jumpToSlopeRayDistance { get; private set; } = 2f;
    [field: SerializeField] [field: Range(0f, 10f)] public float decelerationForce { get; private set; } = 1.5f;
    [field: SerializeField] public AnimationCurve JumpModifierOnSlopeUp { get; private set; }
    [field: SerializeField] public AnimationCurve JumpModifierOnSlopeDown { get; private set; }
    [field: SerializeField] public Vector3 idleJumpForce { get; private set; }
    [field: SerializeField] public Vector3 sprintJumpForce { get; private set; }
    [field: SerializeField] public Vector3 dashJumpForce { get; private set; }
    [field: SerializeField] public PlayerRotationData playerRotationData { get; private set; }
}
