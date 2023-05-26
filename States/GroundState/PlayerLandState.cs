
public class PlayerLandState : PlayerGroundState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, SO_PlayerData data, string animName, bool playAnimAfterMove) : base(player, stateMachine, data, animName, playAnimAfterMove) { }

    #region Base Methods

    public override void Enter()
    {
        base.Enter();

        player.Core.Movement.SetVelocityX(0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (xInput != 0) //MOVE STATE
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
    }

    public override void OnAnimationFinish()
    {
        base.OnAnimationFinish();

        stateMachine.ChangeState(stateMachine.IdleState); //IDLE STATE
    }

    #endregion
}
