using UnityEngine;

public class Won: GameBaseState
{
    public override void OnEnter(GameManager mgr)
    {
        Time.timeScale = 0f;
        Debug.Log("Player has won the game!");
        mgr.gameWonCanvas.SetActive(true);
    }

    public override void UpdateState(GameManager mgr, float dt)
    {
        // No update logic needed for Won state
    }

    public override void OnExit(GameManager mgr)
    {
        mgr.gameWonCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}