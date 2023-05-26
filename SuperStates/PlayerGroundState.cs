
public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine stateMachine, SO_PlayerData data, string animName, bool playAnimAfterMove) : base(player, stateMachine, data, animName, playAnimAfterMove) { }

    #region Variables

    //Services
    protected InputService InputService => GameService.Instance.InputService;

    //Input
    protected int xInput;
    protected bool jumpStartInput;
    protected bool dashInput;
    protected bool interactInput;
    protected bool throwAndTeleportInput;

    //Collision Check
    protected bool isGrounded;
    protected bool isWallMiddle;
    protected bool isLedgeMiddle;
    protected bool isCeiling;

    #endregion

    #region Base Methods

    public override void Enter()
    {
        base.Enter();

        stateMachine.JumpState.ResetAmoutOfJump();
        stateMachine.DashState.ResetCanDash();
        stateMachine.ThrowState.ResetAmountOfThrows();
    }

    public override void CheckInput()
    {
        base.CheckInput();

        xInput = InputController.InputX;
        jumpStartInput = InputController.JumpStartInput;
        dashInput = InputController.DashInput;
        interactInput = InputController.InteractInput;
        throwAndTeleportInput = InputController.ThrowAndTeleportInput;
    }

    public override void CheckPhysics()
    {
        base.CheckPhysics();

        isGrounded = player.Core.Collision.CheckGround();
        isWallMiddle = player.Core.Collision.CheckWallMiddle();
        isLedgeMiddle = player.Core.Collision.CheckLedgeMiddle();
        isCeiling = player.Core.Collision.CheckCeiling();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isGrounded && player.ControlsBy == InputType.Player) //IN AIR STATE
        {
            stateMachine.ChangeState(stateMachine.InAirState);
            stateMachine.InAirState.StartCoyoteTime();
        }

        else if (jumpStartInput) //JUMP STATE
        {
            stateMachine.ChangeState(stateMachine.JumpState);
        }

        else if (throwAndTeleportInput && stateMachine.ThrowState.CanThrow()) //THROW STATE
        {
            stateMachine.ChangeState(stateMachine.ThrowState);
        }

        else if (throwAndTeleportInput && stateMachine.TeleportState.CanTeleport()) //TELEPORT STATE
        {
            stateMachine.ChangeState(stateMachine.TeleportState);
        }

        else if (dashInput && stateMachine.DashState.CanDash) //DASH STATE
        {
            stateMachine.ChangeState(stateMachine.DashState);
        }

        else if (stateMachine.SaveState.CheckIfCanSave() && interactInput) //SAVE STATE
        {
            stateMachine.ChangeState(stateMachine.SaveState);
        }
    }

    #endregion
}
