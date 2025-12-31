

using UnityEngine;

public class SpawningWave : GameBaseState
{
    public override void OnEnter(GameManager mgr)
    {
        Time.timeScale = 0f;
        WaveManager.Instance.StartNextWave();
        mgr.waveSpawnCanvas.gameObject.SetActive(true);
        mgr.waveSpawnCanvas.Initialize(WaveManager.Instance.currentWave);
        
        mgr.waveSpawnCanvas.onCountdownFinished = () =>
        {
            Time.timeScale = 1f;
            mgr.gameStateMachine.SwitchState(mgr, mgr.gameStateMachine.playing);
        };
    }

    public override void UpdateState(GameManager mgr, float dt)
    {
        
    }

    public override void OnExit(GameManager mgr)
    {
        Time.timeScale = 1f;
        mgr.waveSpawnCanvas.gameObject.SetActive(false);
    }
}