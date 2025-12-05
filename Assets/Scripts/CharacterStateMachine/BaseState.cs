
public abstract class BaseState
{
	public abstract void OnEnter(ActionContext context, GameStateMachine stateMachine);
	public abstract void UpdateState(ActionContext context, GameStateMachine stateMachine, float dt);
	public abstract void OnExit(ActionContext context, GameStateMachine stateMachine);
} 