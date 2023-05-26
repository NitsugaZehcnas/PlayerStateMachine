
public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, SO_PlayerData data, string animName, bool playAnimAfterMove) : base(player, stateMachine, data, animName, playAnimAfterMove) { }

    #region Base Methods

    public override void CheckPhysics()
    {
        base.CheckPhysics();

        if (isWallMiddle && !isLedgeMiddle) stateMachine.LedgeState.SetCornerPos(player.Core.Collision.GetCornerMiddlePos());
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (xInput == 0) //STOP STATE
            {
                stateMachine.ChangeState(stateMachine.StopState);
            }

            player.Core.Movement?.CheckFlip(xInput);
            player.Core.Movement?.SetVelocityX(data.moveVelocity * xInput);
        }
    }

    #endregion
}
