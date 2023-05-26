using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, SO_PlayerData data, string animName, bool playAnimAfterMove) : base(player, stateMachine, data, animName, playAnimAfterMove) { }

    #region Variables

    //Collision Checks
    protected bool isGrounded;
    protected bool isWallUp;
    protected bool isWallMiddle;
    protected bool isWallDown;

    //Input
    protected int inputX;
    protected bool jumpStartInput;

    //Data
    protected bool isAbilityDone;

    #endregion

    #region Base Methods

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void CheckInput()
    {
        base.CheckInput();

        inputX = InputController.InputX;
        jumpStartInput = InputController.JumpStartInput;
    }

    public override void CheckPhysics()
    {
        base.CheckPhysics();

        isGrounded = player.Core.Collision.CheckGround();
        isWallUp = player.Core.Collision.CheckWallUp();
        isWallMiddle = player.Core.Collision.CheckWallMiddle();
        isWallDown = player.Core.Collision.CheckWallDown();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAbilityDone && isGrounded && player.Core.Movement.CurrentVelocity.y < 0.01f) //IDLE STATE
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        else if (isAbilityDone && !isGrounded) //IN AIR STATE
        {
            stateMachine.ChangeState(stateMachine.InAirState);
        }
    }

    #endregion
}
