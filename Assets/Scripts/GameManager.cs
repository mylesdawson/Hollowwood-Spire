using UnityEngine;

public class GameManager: MonoBehaviour
{
    [HideInInspector] public LootManager lootManager;
    public GameStateMachine gameStateMachine;
    [HideInInspector] public LootUI lootUI;

    void Awake()
    {
        gameStateMachine = new GameStateMachine();
        lootUI = GameObject.FindFirstObjectByType<LootUI>();
        lootManager = GameObject.FindFirstObjectByType<LootManager>();
    }

    void Start()
    {
        gameStateMachine.StartMachine(this);
    }

    void Update()
    {
        gameStateMachine.UpdateState(this);
    }
}