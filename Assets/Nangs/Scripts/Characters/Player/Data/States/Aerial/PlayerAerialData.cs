using System;
using UnityEngine;

[Serializable]
public class PlayerAerialData 
{
    [field: SerializeField] public PlayerJumpData PlayerJumpData { get; private set; }
    [field: SerializeField] public PlayerFallData PlayerFallData { get; private set; }
}
