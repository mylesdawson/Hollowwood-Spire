using UnityEngine;

public class GameManager: MonoBehaviour
{
    [HideInInspector] public LootManager lootManager;
    public GameStateMachine gameStateMachine;
    public GameObject startGameCanvas;
    public GameObject escMenuCanvas;
    public LootCanvas lootCanvas;
    public GameObject gameOverCanvas;
    public WaveSpawnCanvas waveSpawnCanvas;
    public CharacterController playerInputActions;

    void Awake()
    {
        gameStateMachine = new GameStateMachine();
        lootManager = GameObject.FindFirstObjectByType<LootManager>();
        playerInputActions = new CharacterController();
    }

    void Start()
    {
        playerInputActions.Player.Enable();

        playerInputActions.Player.Escape.performed += ctx =>
        {
            if(gameStateMachine.currentState.GetType().Name == "Start")
                return;
            escMenuCanvas.SetActive(!escMenuCanvas.activeSelf);
            Debug.Log("Escape pressed");
        };

        gameStateMachine.StartMachine(this);
    }

    void Update()
    {
        gameStateMachine.UpdateState(this);
    }
}