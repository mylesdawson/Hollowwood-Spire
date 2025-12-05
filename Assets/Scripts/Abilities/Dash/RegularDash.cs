using System.Collections.Generic;
using UnityEngine;

public class RegularDash : DashBehavior
{
    readonly float downwardDashInputThreshold = -0.60f;
    float initialYInput = 0f;

    public RegularDash(List<DashStatMutation> dashMutations) : base(dashMutations)
    {
        this.dashConfig = new DashConfig();
    }

    public override void OnDashStart(ActionContext ctx)
    {
        ctx.GravityConfig.SetGravityDownMultiplier(0);
        currentDashTimer = dashConfig.GetDashDuration(dashMutations);
        this.dashConfig.DecrementRemainingDashes();
        initialYInput = ctx.MovementInput.y;
        SoundEffectsManager.Instance.PlayEffect("Dash");
    }

    public override bool UpdateDash(ActionContext ctx, float dt)
    {
        var dashSpeed = this.dashConfig.GetDashSpeed(dashMutations);

        // Apply downward velocity if stick is pushed down
        if(initialYInput < downwardDashInputThreshold)
        {
            ctx.VelocityY = -dashSpeed;
        } else
        {
            float dashAmount = ctx.CurrentDirection > 0 ? dashSpeed : -dashSpeed;
            ctx.Velocity = new Vector2(dashAmount, 0);
        }

        currentDashTimer -= dt;

        // Return false to continue dashing, true when duration is complete
        bool shouldContinueDashing = currentDashTimer > 0;
        return shouldContinueDashing;
    }

    public override void OnDashEnd(ActionContext ctx)
    {
        ctx.GravityConfig.SetGravityDownMultiplier(1);
    }
}