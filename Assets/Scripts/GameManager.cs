using Unity.Cinemachine;
using UnityEngine;

public class GameManager: MonoBehaviour
{
    [HideInInspector] public LootManager lootManager;
    public GameStateMachine gameStateMachine;
    public GameObject startGameCanvas;
    public GameObject escMenuCanvas;
    public LootCanvas lootCanvas;
    public GameObject gameOverCanvas;
    public GameObject gameWonCanvas;
    public WaveSpawnCanvas waveSpawnCanvas;
    public WaveManager waveManager;
    public CharacterController playerInputActions;
    public CinemachineCamera cineCamera;
    [SerializeField] GameObject playerPrefab;
    Transform mainGround;

    void Awake()
    {
        mainGround = GameObject.Find("MainGround").transform;
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

        EventBus.Instance.onStartGameClicked += OnStartGameClicked;
        gameStateMachine.StartMachine(this);
    }

    void OnDestroy()
    {
        EventBus.Instance.onStartGameClicked -= OnStartGameClicked;
    }

    void Update()
    {
        gameStateMachine.UpdateState(this);
    }

    public void OnStartGameClicked()
    {
        Time.timeScale = 1f;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(var enemy in enemies)
        {
            Destroy(enemy);
        }
        waveManager.currentWave = 0;

        var oldPlayer = GameObject.FindWithTag("Player");
        if(oldPlayer != null) Destroy(oldPlayer);
        var player = Instantiate(playerPrefab, mainGround);
        cineCamera.Follow = player.transform;
        cineCamera.LookAt = player.transform;

        gameStateMachine.SwitchState(this, this.gameStateMachine.spawningWave);
    }
}