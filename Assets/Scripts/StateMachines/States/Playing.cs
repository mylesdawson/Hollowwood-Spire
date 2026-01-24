

using System;
using UnityEngine;

public class Playing: GameBaseState
{
    GameManager mgr;
    public override void OnEnter(GameManager mgr)
    {
        this.mgr = mgr;
        EventBus.Instance.onEnemyDeath += OnEnemyDeath;
        EventBus.Instance.onLootTouched += OnLootTouched;
    }

    public override void UpdateState(GameManager mgr, float dt)
    {
    }

    public override void OnExit(GameManager mgr)
    {
        EventBus.Instance.onEnemyDeath -= OnEnemyDeath;
        EventBus.Instance.onLootTouched += OnLootTouched;

    }

    private void OnEnemyDeath(GameObject enemy)
    {
        var enemyCanvas = GameObject.Find("EnemyCanvas").GetComponent<EnemyCanvas>();
        enemyCanvas.gameObject.SetActive(false);

        if(mgr.waveManager.IsLastWave())
        {
            mgr.gameStateMachine.SwitchState(mgr, mgr.gameStateMachine.won);
            return;
        }

        mgr.lootManager.SpawnLoot(enemy);
    }

    private void OnLootTouched(GameObject loot)
    {
        mgr.gameStateMachine.SwitchState(mgr, mgr.gameStateMachine.looting);
    }
}