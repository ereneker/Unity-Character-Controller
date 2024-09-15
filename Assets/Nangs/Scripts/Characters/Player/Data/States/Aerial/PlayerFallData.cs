using System;
using UnityEngine;

[Serializable]
public class PlayerFallData
{
    [field: SerializeField] [field: Range(1f, 15f)] public float fallSpeedLimit { get; private set; } = 10f;
    [field: SerializeField] [field: Range(0f, 100f)] public float MinimumDistToHardFall { get; private set; } = 3f;
}
