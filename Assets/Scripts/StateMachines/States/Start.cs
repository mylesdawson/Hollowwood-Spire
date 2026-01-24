

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
    }

    public override void UpdateState(GameManager mgr, float dt)
    {
    }

    public override void OnExit(GameManager mgr)
    {
        mgr.startGameCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}