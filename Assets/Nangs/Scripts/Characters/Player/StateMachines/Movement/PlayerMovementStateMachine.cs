using UnityEngine;

public class PlayerMovementStateMachine : StateMachine
{
    public PlayerControllerCustom PlayerControllerCustom { get; }
    public PlayerIdlingState playerIdlingState { get; }
    public PlayerWalkingState playerWalkingState { get; }
    public PlayerSprintState PlayerSprintState { get; }
    public PlayerRunState PlayerRunState { get; }
    public PlayerStoppingState PlayerStoppingState { get; }
    public PlayerDefaultStoppingState PlayerDefaultStoppingState { get; }
    public PlayerDashState PlayerDashState { get; }
    public PlayerJumpingState PlayerJumpingState { get; }
    public PlayerFallingState PlayerFallingState { get; }
    public PlayerLightLandingState PlayerLightLandingState { get; }
    public PlayerStateReusableData playerStateReusableData { get; }

    public PlayerMovementStateMachine(PlayerControllerCustom playerControllerCustom)
    {
        this.PlayerControllerCustom = playerControllerCustom;
        playerStateReusableData = new PlayerStateReusableData();
        
        playerIdlingState = new PlayerIdlingState(this);
        playerWalkingState = new PlayerWalkingState(this);
        PlayerSprintState = new PlayerSprintState(this);
        PlayerRunState = new PlayerRunState(this);
        PlayerStoppingState = new PlayerStoppingState(this);
        PlayerDefaultStoppingState = new PlayerDefaultStoppingState(this);
        PlayerDashState = new PlayerDashState(this);
        PlayerFallingState = new PlayerFallingState(this);
        PlayerJumpingState = new PlayerJumpingState(this);
        PlayerLightLandingState = new PlayerLightLandingState(this);
    }
}
