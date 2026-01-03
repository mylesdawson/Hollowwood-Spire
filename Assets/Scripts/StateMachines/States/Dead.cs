using UnityEngine;

public class Dead: GameBaseState
{
    GameManager mgr;

    public override void OnEnter(GameManager mgr)
    {
        mgr.gameOverCanvas.SetActive(true);
        Debug.Log("Player died, game over!");
        // Logic to execute when entering the Dead state
        // e.g., play death animation, disable player controls, etc.

        Time.timeScale = 0f;
        this.mgr = mgr;
        EventBus.Instance.onStartGameClicked += OnStartGameClicked;
    }

    public override void UpdateState(GameManager mgr, float dt)
    {
        // No update logic needed for Dead state
    }

    public override void OnExit(GameManager mgr)
    {
        mgr.gameOverCanvas.SetActive(false);
        EventBus.Instance.onStartGameClicked -= OnStartGameClicked;
        Time.timeScale = 1f;
        // Logic to execute when exiting the Dead state
        // e.g., reset player stats, prepare for respawn, etc.
    }

    private void OnStartGameClicked()
    {
        this.mgr.gameStateMachine.SwitchState(mgr, mgr.gameStateMachine.spawningWave);
    }

}