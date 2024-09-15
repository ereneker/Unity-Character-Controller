using System;
using UnityEngine;

[Serializable]
public class PlayerDashData
{
    [field: SerializeField]
    [field: Range(1f, 3f)]
    public float speedModifier { get; private set; } = 2f;
    
    [field: SerializeField]
    [field: Range(1f, 3f)]
    public float dashLimitCooldown { get; private set; } = 1f;

    [field: SerializeField]
    [field: Range(0f, 5f)]
    public float dashLimitReachedCooldown { get; private set; } = 1.75f;

    [field: SerializeField]
    [field: Range(1f, 2f)]
    public int consecutiveDashLimit { get; private set; } = 1;
}
