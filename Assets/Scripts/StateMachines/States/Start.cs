

using System;
using UnityEngine;

public class Start: GameBaseState
{
    GameManager mgr;
    public override void OnEnter(GameManager mgr)
    {
        Time.timeScale = 0f;
        this.mgr = mgr;
        mgr.startGameCanvas.gameObject.SetActive(true);
        EventBus.Instance.onStartGameClicked += OnStartGameClicked;
    }

    private void OnStartGameClicked()
    {
        this.mgr.gameStateMachine.SwitchState(mgr, mgr.gameStateMachine.spawningWave);
    }

    public override void UpdateState(GameManager mgr, float dt)
    {
    }

    public override void OnExit(GameManager mgr)
    {
        mgr.startGameCanvas.gameObject.SetActive(false);
        EventBus.Instance.onStartGameClicked -= OnStartGameClicked;
        Time.timeScale = 1f;
    }
}