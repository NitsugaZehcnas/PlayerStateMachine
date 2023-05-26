/// <summary>
/// Controls the player states
/// </summary>
public class PlayerStateMachine
{
	#region Variables

	//Current State
	public PlayerState CurrentState { get; private set; }

	//States
	public PlayerIdleState IdleState { get; private set; }
	public PlayerMoveState MoveState { get; private set; }
	public PlayerStopState StopState { get; private set; }
	public PlayerLandState LandState { get; private set; }

	public PlayerJumpState JumpState { get; private set; }	
	public PlayerInAirState InAirState { get; private set; }

	#endregion

	#region StateMachine Methods

	public void Initialize(Player player, SO_PlayerData data)
	{
		GroundInitialize(player, data);
		AbilitiesInitialize(player, data);
		OtherStatesInitialize(player, data);

		CurrentState = IdleState;
		CurrentState.Enter();
	}

	public void ChangeState(PlayerState newState)
	{
		CurrentState.Exit();
		CurrentState = newState;
		CurrentState.Enter();
	}

    #endregion

    #region Initialize Methods

    private void GroundInitialize(Player player, SO_PlayerData data)
	{
		IdleState = new(player, this, data, "Idle", false);
	 	MoveState = new(player, this, data, "Move", true);
		StopState = new(player, this, data, "Stop", true);
		LandState = new(player, this, data, "Land", false);
	}

	private void AbilitiesInitialize(Player player, SO_PlayerData data)
	{
		JumpState = new(player, this, data, "", false);
    }

	private void OtherStatesInitialize(Player player, SO_PlayerData data)
	{
		InAirState = new(player, this, data, "InAir", false);
	}

	#endregion
}
