using System;
using UnityEngine;

[Serializable]
public class PlayerStopData
{
    [field: SerializeField]
    [field: Range(0f, 15f)]
    public float defaultDecelerationForce { get; private set; } = 5f;
}
