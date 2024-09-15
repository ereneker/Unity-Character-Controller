using UnityEngine;

public class PlayerAerialState : PlayerMovementState
{
    public PlayerAerialState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region MyRegion

    public override void Enter()
    {
        base.Enter();

        AnimationStart(_stateMachine.PlayerControllerCustom.AnimationsData.AirborneParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        AnimationStop(_stateMachine.PlayerControllerCustom.AnimationsData.AirborneParameterHash);
    }

    #endregion

    #region Main Methods

    protected override void OnContactWithGround(Collider collider)
    {
        _stateMachine.ChangeState(_stateMachine.PlayerLightLandingState);
    }

    #endregion
}
