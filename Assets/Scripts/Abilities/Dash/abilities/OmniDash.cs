using System.Collections.Generic;
using UnityEngine;

public class OmniDash : DashBehavior
{
    public override string AbilityName => "Omni Dash";
    public override string AbilityDescription => "Dash in any direction, allowing for quick maneuvers.";
    public override AbilityType AbilityType => AbilityType.Dash;

    public OmniDash()
    {
        this.dashConfig = new DashConfig();
    }

    public override void OnStart(ActionContext ctx, List<AbilityStatMutation> mutations)
    {
        currentDashTimer = dashConfig.GetStat(AbilityStat.dashDuration, mutations);
        this.dashConfig.DecrementRemainingDashes();
        // SoundEffectsManager.Instance.PlayEffect("Dash");
    }

    public override bool OnUpdate(ActionContext ctx, float dt, List<AbilityStatMutation> mutations)
    {
        var input = ctx.MovementInput;
        var normalized = input.normalized;
        var direction = ctx.CurrentDirection;
        var normX = normalized.x == 0 ? direction : normalized.x;

        var dashSpeed = this.dashConfig.GetStat(AbilityStat.dashSpeed, mutations);

        ctx.Velocity = new Vector2(normX * dashSpeed, normalized.y * (dashSpeed * .7f));

        currentDashTimer -= dt;

        // Return false to continue dashing, true when duration is complete
        bool shouldContinueDashing = currentDashTimer > 0;
        return shouldContinueDashing;
    }

    public override void OnEnd(ActionContext ctx, List<AbilityStatMutation> mutations)
    {
    }
}