

public class Start: GameBaseState
{
    public override void OnEnter(GameManager mgr)
    {
        // TODO: actual logic
        mgr.gameStateMachine.SwitchState(mgr, mgr.gameStateMachine.spawningWave);
    }

    public override void UpdateState(GameManager mgr, float dt)
    {
    }

    public override void OnExit(GameManager mgr)
    {
    }
}