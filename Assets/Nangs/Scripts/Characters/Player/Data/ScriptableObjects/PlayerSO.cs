using UnityEngine;

[CreateAssetMenu(fileName = "PlayerControllerCustom", menuName = "Custom/Characters/PlayerControllerCustom")]
public class PlayerSO : ScriptableObject
{
    [field: InspectorName("PlayerControllerCustom Grounded Data")]
    [field: SerializeField] 
    public PlayerGroundedData PlayerGroundedData { get; private set; }
    
    [field: InspectorName("PlayerControllerCustom Aerial Data")]
    [field: SerializeField]
    public PlayerAerialData PlayerAerialData { get; private set; }
}
