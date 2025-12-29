


using UnityEngine;

public class GameStateMachine
{
    public GameBaseState start = new Start();
    public GameBaseState dead = new Dead();
    public GameBaseState won = new Won();
    public GameBaseState playing = new Playing();
    public GameBaseState looting = new Looting();
    public GameBaseState spawningWave = new SpawningWave();

    public GameBaseState currentState;
    public GameBaseState previousState;

    public void StartMachine(GameManager mgr)
    {
        currentState = start;
        currentState.OnEnter(mgr);    
    }

    public void UpdateState(GameManager mgr)
    {
        currentState.UpdateState(mgr, Time.deltaTime);
    }

    public void SwitchState(GameManager mgr, GameBaseState newState)
    {
        previousState = currentState;
        currentState.OnExit(mgr);
        currentState = newState;
        currentState.OnEnter(mgr);
    }
}