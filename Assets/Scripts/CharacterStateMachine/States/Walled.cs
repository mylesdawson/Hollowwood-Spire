using UnityEngine;

public class Walled: BaseState
{
	public override void OnEnter(ActionContext ctx, PlayerStateMachine sm)
	{
		ctx.ResetMovementAbilities();
		ctx.GravityConfig.SetGravityDownMultiplier(.8f);
	}

	public override void UpdateState(ActionContext context, PlayerStateMachine stateMachine, float dt)
	{
		var grounded = context.IsGrounded;
		if(grounded)
		{
			stateMachine.SwitchState(context, stateMachine.grounded);
			return;
		}

		var wStatus = context.WalledStatus;
		if(wStatus == WalledStatus.None)
        {
            stateMachine.SwitchState(context, stateMachine.airborne);
			return;
        }
	}

	public override void OnExit(ActionContext ctx, PlayerStateMachine sm)
    {
        ctx.GravityConfig.SetGravityDownMultiplier(1f);
    }
}