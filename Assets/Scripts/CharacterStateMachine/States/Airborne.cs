using UnityEngine;

public class Airborne: BaseState
{
	float wallClingMovementThreshold = 0.2f;

	public override void OnEnter(ActionContext ctx, PlayerStateMachine sm)
	{
	}
	public override void UpdateState(ActionContext ctx, PlayerStateMachine sm, float dt)
	{
		var grounded = ctx.IsGrounded;
		if(grounded)
		{
			sm.SwitchState(ctx, sm.grounded);
			return;
		}

		var wStatus = ctx.WalledStatus;
		var isFalling = ctx.VelocityY <= 0;
		var xValue = ctx.VelocityX;
        var clingLeft = wStatus == WalledStatus.Left && xValue < -wallClingMovementThreshold;
        var clingRight = wStatus == WalledStatus.Right && xValue > wallClingMovementThreshold;
        var canAttachToWall = isFalling && (clingLeft || clingRight);
		if(canAttachToWall)
		{
			sm.SwitchState(ctx, sm.walled);
			return;
		}


	}
	public override void OnExit(ActionContext ac, PlayerStateMachine stateMachine)
    {
    }
}