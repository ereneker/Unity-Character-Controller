using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerLayerData 
{
    [field: SerializeField] public LayerMask groundLayer { get; private set; }

    public bool IsContainLayer(LayerMask layerMask, int layer)
    {
        return (1 << layer & layerMask) != 0;
    }

    public bool IsGroundLayer(int layer)
    {
        return IsContainLayer(groundLayer, layer);
    }
}
