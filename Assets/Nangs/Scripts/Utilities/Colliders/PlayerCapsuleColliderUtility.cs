using System;
using UnityEngine;

[Serializable]
public class PlayerCapsuleColliderUtility : CapsuleColliderUtility
{
    [field: SerializeField] public PlayerTriggerColliderData PlayerTriggerColliderData { get; private set; }
}
