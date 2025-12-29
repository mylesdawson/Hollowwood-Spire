
public abstract class BaseState
{
	public abstract void OnEnter(ActionContext context, PlayerStateMachine stateMachine);
	public abstract void UpdateState(ActionContext context, PlayerStateMachine stateMachine, float dt);
	public abstract void OnExit(ActionContext context, PlayerStateMachine stateMachine);
} 