using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, SO_PlayerData data, string animName, bool playAnimAfterMove) : base(player, stateMachine, data, animName, playAnimAfterMove) { }

    #region Variables

    private Vector2 startPosition = Vector2.zero;

    #endregion

    #region Base Methods

    public override void Enter()
    {
        base.Enter();

        player.Core.Movement?.SetVelocityX(0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (xInput != 0) //MOVE STATE
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
    }

    public override void LastEnter()
    {
        base.LastEnter();

        if (startPosition != Vector2.zero) 
            player.transform.transform.position = startPosition;
    }

    public override void Exit()
    {
        base.Exit();

        startPosition = Vector2.zero;
    }

    #endregion

    #region Set Methods

    public void SetStartPosition(Vector2 startOffest) => startPosition = startOffest;

    #endregion
}
