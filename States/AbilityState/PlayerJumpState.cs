
public class PlayerJumpState : PlayerAbilityState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, SO_PlayerData data, string animName, bool playAnimAfterMove) : base(player, stateMachine, data, animName, playAnimAfterMove) 
    {
        amountOfJumpsLeft = data.amountOfJumps;
    }

    #region Variables

    //Data
    public float LastWallJumpTime { get; private set; }

    private int amountOfJumpsLeft;
    private bool wallJump;

    #endregion

    #region Base Methods

    public override void Enter()
    {
        base.Enter();

        InputController?.UseJumpStartInput();
        amountOfJumpsLeft--;

        if (wallJump) stateMachine.InAirState.SetStartWallJump();
        
        player.Core.Movement.SetVelocityY(data.jumpVelocity);
        stateMachine.InAirState.SetStartJump();

        isAbilityDone = true;
    }

    public override void Exit()
    {
        base.Exit();

        wallJump = false;
    }

    #endregion

    #region Check Methods

    public bool CheckCanJump() => amountOfJumpsLeft > 0;

    #endregion

    #region Set Methods

    public void ResetAmoutOfJump() => amountOfJumpsLeft = data.amountOfJumps;
    public void DecreaseAmountOfJumps() => amountOfJumpsLeft--;
    public void SetWallJump() => wallJump = true;

    #endregion
}
