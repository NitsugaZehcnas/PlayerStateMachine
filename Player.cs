using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IEntityAnim
{
	#region Variables

    public static Player Instance { get; private set; }

    //Services
    private InputService InputService => GameService.Instance.InputService;

    //Components
    public CharacterCore Core { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public BaseInputController InputController { get; private set; }
    public Sword Sword { get; private set; }

    private PlayerEventListener eventListener;

    //Data
	[SerializeField] private SO_PlayerData data;

    public SO_PlayerData Data => data;
    public InputType ControlsBy { get; private set; }

    private bool playerInitialize;

    #endregion

    #region MonoBehavior Methods

    private void Update()
    {
        if (!playerInitialize) return;

        Core.LogicUpdate();
        InputController.UpdateInputs();
        Sword.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        if (!playerInitialize) return;

        Sword.PhysicsUpdate();
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void LateUpdate()
    {
        if (!playerInitialize) return;

        StateMachine.CurrentState.LastUpdate();
    }

    private void OnDestroy()
    {
        if (!playerInitialize) return;

        eventListener.Dispose();
        InputController.Dispose();
    }

    #endregion

    #region Initialize Methods

    public void Initialize(Sword sword)
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SetInputController(InputType.AI);

        Core = GetComponent<CharacterCore>();
        StateMachine = new();
        Sword = sword;
        eventListener = new();

        Core.Initialize();
        StateMachine.Initialize(this, data);
        Sword.Initialize(this);
        eventListener.Initialize(this);

        gameObject.SetActive(false);

        playerInitialize = true;
    }

    #endregion

    #region Animation Methods

    public void OnCurrentAnimationFinish() => StateMachine.CurrentState.OnAnimationFinish();
    public void OnCurrentAnimationTriggers() => StateMachine.CurrentState.OnAnimationTrigger();

    #endregion

    #region Input Methods

    public BaseInputController SetInputController(InputType type)
    {
        ControlsBy = type;

        InputController?.Dispose();
        InputController = InputService.CreateInputController(type);
        InputController.Initialize();

        return InputController;
    }

    #endregion
}
