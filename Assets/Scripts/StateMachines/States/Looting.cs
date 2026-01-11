
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Looting: GameBaseState
{
    GameManager mgr;
    public override void OnEnter(GameManager mgr)
    {
        this.mgr = mgr;
        mgr.lootCanvas.Initialize(mgr.lootManager.generatedLoot);
        mgr.lootCanvas.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mgr.lootCanvas.lootContainer.GetChild(0).gameObject);
        EventBus.Instance.onAbilityLooted += OnAbilityLooted;
        Time.timeScale = 0f; // Pause the game
    }

    public override void UpdateState(GameManager mgr, float dt)
    {
        // No update logic needed for Looting state
    }

    public override void OnExit(GameManager mgr)
    {
        Time.timeScale = 1f; // Resume the game
        EventBus.Instance.onAbilityLooted -= OnAbilityLooted;
    }

    private void OnAbilityLooted(Ability ability)
    {
        mgr.lootCanvas.gameObject.SetActive(false);
        mgr.gameStateMachine.SwitchState(mgr, mgr.gameStateMachine.spawningWave);
    }

}