
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, SO_PlayerData data, string animName, bool playAnimAfterMove) : base(player, stateMachine, data, animName, playAnimAfterMove) { }

    #region Variables

    //Input
    private int xInput;
    private bool jumpStartInput;
    private bool jumpStopInput;
    private bool dashInput;
    private bool throwAndTeleportInput;

    //Collision Checks
    private bool isGrounded;
    private bool isWall;
    private bool isWallUp;
    private bool isWallMiddle;
    private bool isLedgeUp;
    private bool isLedgeMiddle;
    private bool isCeiling;

    //Data
    private bool isJump;
    private bool isWallJump;
    private bool coyoteTime;
    private float wallSlideStartTime;

    #endregion

    #region Base Methods

    public override void CheckInput()
    {
        base.CheckInput();

        xInput = InputController.InputX;
        jumpStartInput = InputController.JumpStartInput;
        jumpStopInput = InputController.JumpStopInput;
        dashInput = InputController.DashInput;
        throwAndTeleportInput = InputController.ThrowAndTeleportInput;
    }

    public override void CheckPhysics()
    {
        base.CheckPhysics();

        isGrounded = player.Core.Collision.CheckGround();
        isWall = player.Core.Collision.CheckWall(true);
        isWallUp = player.Core.Collision.CheckWallUp();
        isWallMiddle = player.Core.Collision.CheckWallMiddle();
        isLedgeUp = player.Core.Collision.CheckLedgeUp();
        isLedgeMiddle = player.Core.Collision.CheckLedgeMiddle();
        isCeiling = player.Core.Collision.CheckCeiling();

        if (isWallUp && !isLedgeUp) stateMachine.LedgeState.SetCornerPos(player.Core.Collision.GetCornerUpPos());
        else if (isWallMiddle && !isLedgeMiddle) stateMachine.LedgeState.SetCornerPos(player.Core.Collision.GetCornerMiddlePos());
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            ApplyJumpMultiplier();
            CheckCoyoteTime();

            if (isGrounded && player.Core.Movement.CurrentVelocity.y < 0.01f && xInput == 0) //LAND STATE
            {
                stateMachine.ChangeState(stateMachine.LandState);
            }

            else if (CanLedgeUp()) //LEDGE UP STATE
            {
                stateMachine.ChangeState(stateMachine.LedgeState);
            }

            else if (CanLedgeMiddle()) //LEDGE MIDDLE STATE
            {
                stateMachine.LedgeState.SetMiddleLedge();
                stateMachine.ChangeState(stateMachine.LedgeState);
            }

            else if (dashInput && stateMachine.DashState.CanDash) //DASH STATE
            {
                InputController.UseDashInput();
                stateMachine.ChangeState(stateMachine.DashState);
            }

            else if (throwAndTeleportInput && stateMachine.ThrowState.CanThrow()) //THROW STATE
            {
                stateMachine.ChangeState(stateMachine.ThrowState);
            }

            else if (throwAndTeleportInput && stateMachine.TeleportState.CanTeleport()) //TELEPORT STATE
            {
                stateMachine.ChangeState(stateMachine.TeleportState);
            }

            else if (jumpStartInput && stateMachine.JumpState.CheckCanJump()) //JUMP STATE
            {
                stateMachine.ChangeState(stateMachine.JumpState);
            }

            else if (CanWallSlide()) //WALL SLIDE STATE
            {
                stateMachine.ChangeState(stateMachine.WallSlideState);
            }

            else if (isGrounded && player.Core.Movement.CurrentVelocity.y < 0.01f && xInput != 0) //MOVE STATE
            {
                stateMachine.ChangeState(stateMachine.MoveState);
            }

            if (!isWallJump && !isExitingState)
            {
                player.Core.Movement.CheckFlip(xInput);
                player.Core.Movement.SetVelocityX(data.moveVelocity * xInput);
            }

            player.Core.Animator.SetFloat("yVelocity", player.Core.Movement.CurrentVelocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();

        isJump = false;
        isWallJump = false;
    }

    #endregion

    #region State Methods

    private void ApplyJumpMultiplier()
    {
        if (isExitingState) return;

        if (isJump)
        {
            if (jumpStopInput)
            {
                player.Core.Movement.SetVelocityY(player.Core.Movement.CurrentVelocity.y * data.jumpDecreaseMultiplier);
                isJump = false;
            }

            else if (player.Core.Movement.CurrentVelocity.y <= 0)
            {
                isJump = false;
            }
        }

        if (isWallJump)
        {
            player.Core.Movement.SetVelocityX(data.wallJumpVelocity * player.Core.Movement.Direction);

            bool input = xInput != 0 && xInput != player.Core.Movement.Direction && Time.time >= startTime + data.wallJumpMinTime;
            bool time = Time.time >= startTime + data.wallJumpMaxTime;

            if (input || time) isWallJump = false;
        }
    }

    #endregion

    #region Check Methods

    public bool CanWallSlide()
    {
        //TODO: Check if unlock the hability

        bool andBool = stateMachine.ThrowState.HasSword && isWall && player.Core.Movement.CurrentVelocity.y <= 0;
        bool orBool = xInput == player.Core.Movement.Direction || Time.time < wallSlideStartTime + 0.1f;

        return andBool && orBool;
    }

    private void CheckCoyoteTime()
    {
        if (coyoteTime && Time.time >= startTime + data.coyoteTime)
        {
            coyoteTime = false;
            stateMachine.JumpState.DecreaseAmountOfJumps();
        }
    }

    private bool CanLedgeUp() => player.ControlsBy == InputType.Player && !isCeiling && isWallUp && !isLedgeUp && player.Core.Movement.CurrentVelocity.y <= 0;
    private bool CanLedgeMiddle() => player.ControlsBy == InputType.Player && !isCeiling && isWallMiddle && !isLedgeMiddle && xInput == player.Core.Movement.Direction && player.Core.Movement.CurrentVelocity.y <= 0;

    #endregion

    #region Set Methods

    public void SetStartJump() => isJump = true;
    public void SetStartWallJump() => isWallJump = true;
    public void StartCoyoteTime() => coyoteTime = true;
    public void SetWallSlideStartTime() => wallSlideStartTime = Time.time;

    #endregion
}
