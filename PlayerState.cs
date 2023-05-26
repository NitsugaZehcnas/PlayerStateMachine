using UnityEngine;

/// <summary>
/// Base logic for a player state
/// </summary>
public class PlayerState : IState
{
    public PlayerState(Player player, PlayerStateMachine stateMachine, SO_PlayerData data, string animName, bool playAnimAfterMove)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.data = data;
        this.animName = animName;
        this.playAnimAfterMove = playAnimAfterMove;
    }

    #region Variables

    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected SO_PlayerData data;
    protected BaseInputController InputController => player.InputController;

    private bool isLastEnter;
    private string animName;

    protected bool isExitingState;
    protected bool isAnimFinish;
    protected float startTime;
    protected bool playAnimAfterMove;

    #endregion

    #region State Methods

    public virtual void Enter()
    {
        if (animName != "" && !playAnimAfterMove)
        {
            player.Core.Animator.PlayAnim(animName);
        }

        startTime = Time.time;

        isExitingState = false;
        isLastEnter = false;
        isAnimFinish = false;

        CheckPhysics();
        CheckInput();
    }

    public virtual void LastEnter()
    {
        if (isExitingState) return;

        isLastEnter = true;

        if (animName != "" && playAnimAfterMove)
        {
            player.Core.Animator.PlayAnim(animName);
        }
    }

    public virtual void CheckInput()
    {

    }

    public virtual void CheckPhysics()
    {

    }

    public virtual void LogicUpdate()
    {
        CheckInput();
    }

    public virtual void PhysicsUpdate()
    {
        CheckPhysics();
    }

    public virtual void LastUpdate()
    {
        if (!isLastEnter) LastEnter();
    }

    public virtual void Exit()
    {
        isExitingState = true;
    }

    public virtual void OnAnimationFinish()
    {
        isAnimFinish = true;
    }

    public virtual void OnAnimationTrigger()
    {

    }

    #endregion
}
