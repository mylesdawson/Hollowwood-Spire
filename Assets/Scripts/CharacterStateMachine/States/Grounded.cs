public class Grounded: BaseState
{
	public override void OnEnter(ActionContext context, GameStateMachine stateMachine)
    {
        context.ResetMovementAbilities();
    }

	public override void UpdateState(ActionContext context, GameStateMachine stateMachine, float dt)
	{
		context.ResetMovementAbilities();

		var grounded = context.IsGrounded;
		if (!grounded)
		{
			stateMachine.SwitchState(context, stateMachine.airborne);
			return;
		}
	}
	
	public override void OnExit(ActionContext context, GameStateMachine stateMachine)
    {
        
    }
}