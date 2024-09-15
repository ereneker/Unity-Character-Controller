using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSprintData
{
    [field: SerializeField]
    [field: Range(1f, 3f)]
    public float speedModifier { get; private set; } = 1.7f;
}
