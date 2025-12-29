

public class SpawningWave : GameBaseState
{
    public override void OnEnter(GameManager mgr)
    {
        WaveManager.Instance.StartNextWave();
        mgr.gameStateMachine.SwitchState(mgr, mgr.gameStateMachine.playing);
    }

    public override void UpdateState(GameManager mgr, float dt)
    {
        
    }

    public override void OnExit(GameManager mgr)
    {
        
    }
}