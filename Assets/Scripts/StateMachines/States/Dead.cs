

public class Dead: GameBaseState
{
    public override void OnEnter(GameManager mgr)
    {
        // Logic to execute when entering the Dead state
        // e.g., play death animation, disable player controls, etc.
    }

    public override void UpdateState(GameManager mgr, float dt)
    {
        // No update logic needed for Dead state
    }

    public override void OnExit(GameManager mgr)
    {
        // Logic to execute when exiting the Dead state
        // e.g., reset player stats, prepare for respawn, etc.
    }
}