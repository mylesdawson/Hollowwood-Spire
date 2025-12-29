

public class Won: GameBaseState
{
    public override void OnEnter(GameManager mgr)
    {
        // Logic to execute when entering the Won state
        // e.g., display victory screen, play celebration animation, etc.
    }

    public override void UpdateState(GameManager mgr, float dt)
    {
        // No update logic needed for Won state
    }

    public override void OnExit(GameManager mgr)
    {
        // Logic to execute when exiting the Won state
        // e.g., reset game for new session, etc.
    }
}