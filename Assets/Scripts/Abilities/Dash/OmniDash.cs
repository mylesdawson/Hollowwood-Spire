using System.Collections.Generic;
using UnityEngine;

public class OmniDash : DashBehavior
{
    public OmniDash(List<DashStatMutation> dashMutations) : base(dashMutations)
    {
        this.dashConfig = new DashConfig();
    }

    public override void OnDashStart(ActionContext ctx)
    {
        currentDashTimer = dashConfig.GetDashDuration(dashMutations);
        this.dashConfig.DecrementRemainingDashes();
        // SoundEffectsManager.Instance.PlayEffect("Dash");
    }

    public override bool UpdateDash(ActionContext ctx, float dt)
    {
        var input = ctx.MovementInput;
        var normalized = input.normalized;
        var direction = ctx.CurrentDirection;
        var normX = normalized.x == 0 ? direction : normalized.x;

        var dashSpeed = this.dashConfig.GetDashSpeed(dashMutations);

        ctx.Velocity = new Vector2(normX * dashSpeed, normalized.y * (dashSpeed * .7f));

        currentDashTimer -= dt;

        // Return false to continue dashing, true when duration is complete
        bool shouldContinueDashing = currentDashTimer > 0;
        return shouldContinueDashing;
    }

    public override void OnDashEnd(ActionContext ctx)
    {
    }
}