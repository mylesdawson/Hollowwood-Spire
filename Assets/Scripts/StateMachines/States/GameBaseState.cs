public abstract class GameBaseState
{
	public abstract void OnEnter(GameManager mgr);
	public abstract void UpdateState(GameManager mgr, float dt);
	public abstract void OnExit(GameManager mgr);
} 